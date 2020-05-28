using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.PluginBase.Attributes;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.PluginBase.Abstract
{
    public abstract class PluginBase: IPlugin
    {
        protected PluginBase(ILoggerFactory loggerFactory)
        {
            var type = GetType();
            Logger = loggerFactory.CreateLogger(type);
            var interfaces = type.GetInterfaces();
            HasAddon = interfaces.Any(i => i == typeof(IAddon));
            HasTheme = interfaces.Any(i => i == typeof(ITheme));
        }

        #region property

        protected ILogger Logger { get; }

        IPluginInformations? Informations { get; set; }

        protected bool HasAddon { get; }
        protected bool HasTheme { get; }

        protected bool IsLoadedAddon { get; private set; }
        protected bool IsLoadedTheme { get; private set; }

        internal virtual AddonBase Addon { get => throw new NotImplementedException(); }
        internal virtual ThemeBase Theme { get => throw new NotImplementedException(); }

        #endregion

        #region function

        protected abstract void InitializeImpl(IPluginInitializeContext pluginInitializeContext);
        protected abstract void UninitializeImpl(IPluginUninitializeContext pluginUninitializeContext);

        protected abstract void LoadAddonImpl(IPluginContext pluginContext);
        protected abstract void LoadThemeImpl(IPluginContext pluginContext);

        protected abstract void UnloadAddonImpl(IPluginContext pluginContext);
        protected abstract void UnloadThemeImpl(IPluginContext pluginContext);

        protected virtual IPluginInformations CreateInformations()
        {
            static string CreateRandomText(string format, int count)
            {
                var rand = new Random();
                var randomValues = new byte[16];
                rand.NextBytes(randomValues);
                return string.Format(format, BitConverter.ToString(randomValues));
            }

            var assembly = Assembly.GetExecutingAssembly();
            var assemblyName = assembly.GetName();
            var assemblyType = assembly.GetType();

            var pluginIdentifiersAttr = assembly.GetCustomAttribute<PluginIdentifiersAttibute>();
            if(pluginIdentifiersAttr == null) {
                pluginIdentifiersAttr = new PluginIdentifiersAttibute(CreateRandomText("DUMMY-PLUGIN-{0}", 16), Guid.NewGuid().ToString());
                Logger.LogWarning("{0} の取得に失敗したためダミー値にて処理: {1}, {2}", nameof(PluginIdentifiersAttibute), pluginIdentifiersAttr.PluginName, pluginIdentifiersAttr.PluginId);
            }


            var supportVersionsAttr = assemblyType.GetCustomAttribute<SupportVersionsAttibute>();
            if(supportVersionsAttr == null) {
                Logger.LogWarning("{0} の取得に失敗したため最低バージョンで補正", nameof(SupportVersionsAttibute));
                supportVersionsAttr = new SupportVersionsAttibute();
            }

            var pluginAuthorsAttr = assembly.GetCustomAttribute<PluginAuthorsAttribute>();
            if(pluginAuthorsAttr == null) {
                pluginAuthorsAttr = new PluginAuthorsAttribute(CreateRandomText("NAME-{0}", 4), PluginLicense.Unknown);
                Logger.LogWarning("{0} の取得に失敗したためダミー値にて処理: {1}, {2}", nameof(PluginIdentifiersAttibute), pluginAuthorsAttr.Name, pluginAuthorsAttr.License);
            }

            var pluginIdentifiers = new PluginIdentifiers(pluginIdentifiersAttr.PluginId, pluginIdentifiersAttr.PluginName);
            var pluginVersions = new PluginVersions(assemblyName.Version!, supportVersionsAttr.MinimumVersion, supportVersionsAttr.MaximumVersion);
            var pluginAuthors = new PluginAuthors(new Author(pluginAuthorsAttr.Name), pluginAuthorsAttr.License);

            return new PluginInformations(pluginIdentifiers, pluginVersions, pluginAuthors);
        }

        [Conditional("DEBUG")]
        private void LoggingNotSupportAddon()
        {
            if(!HasTheme) {
                Logger.LogWarning("このプラグインはアドオンがサポートされていない");
            }
        }

        [Conditional("DEBUG")]
        private void LoggingNotSupportTheme()
        {
            if(!HasTheme) {
                Logger.LogWarning("このプラグインはテーマがサポートされていない");
            }
        }

        private TTheme BuildSupporttedAddon<TArgument, TTheme>(AddonKind addonKind, string methodName, TArgument argument, Func<TArgument, TTheme> build)
        {
            if(!Addon.IsSupported(addonKind)) {
                Logger.LogWarning("{0} はサポートされていない", addonKind);
                throw new NotSupportedException();
            }

            try {
                return build(argument);
            } catch(NotImplementedException) {
                Logger.LogError("{0} の実装が必要({1})", methodName, addonKind);
                throw;
            }
        }

        private TTheme BuildSupporttedTheme<TArgument, TTheme>(ThemeKind themeKind, string methodName, TArgument argument, Func<TArgument, TTheme> build)
        {
            if(!Theme.IsSupported(themeKind)) {
                Logger.LogWarning("{0} はサポートされていない", themeKind);
                throw new NotSupportedException();
            }

            try {
                return build(argument);
            } catch(NotImplementedException) {
                Logger.LogError("{0} の実装が必要({1})", nameof(BuildGeneralTheme), themeKind);
                throw;
            }
        }

        #endregion

        #region IPlugin

        /// <inheritdoc cref="IPlugin.PluginInformations"/>
        public IPluginInformations PluginInformations => Informations ??= CreateInformations();

        /// <inheritdoc cref="IPlugin.IsInitialized"/>
        public bool IsInitialized { get; private set; }

        /// <inheritdoc cref="IPlugin"/>
        public void Initialize(IPluginInitializeContext pluginInitializeContext)
        {
            if(IsInitialized) {
                throw new InvalidOperationException(nameof(IsInitialized));
            }

            InitializeImpl(pluginInitializeContext);
            IsInitialized = true;
        }

        /// <inheritdoc cref="IPlugin.Uninitialize(IPluginUninitializeContext)"/>
        public void Uninitialize(IPluginUninitializeContext pluginUninitializeContext)
        {
            UninitializeImpl(pluginUninitializeContext);
            // 例外で死んだ場合は再初期化を避けるため補正しない
            IsInitialized = true;
        }

        /// <inheritdoc cref="IPlugin.Load(PluginKind, IPluginContext)"/>
        public void Load(PluginKind pluginKind, IPluginContext pluginContext)
        {
            switch(pluginKind) {
                case PluginKind.Addon:
                    if(HasAddon) {
                        if(IsLoadedAddon) {
                            throw new InvalidOperationException(nameof(IsLoadedAddon));
                        }
                        LoadAddonImpl(pluginContext);
                        IsLoadedAddon = true;
                    } else {
                        throw new NotSupportedException();
                    }
                    break;

                case PluginKind.Theme:
                    if(HasTheme) {
                        if(IsLoadedTheme) {
                            throw new InvalidOperationException(nameof(IsLoadedTheme));
                        }
                        LoadThemeImpl(pluginContext);
                        IsLoadedTheme = true;
                    } else {
                        throw new NotSupportedException();
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
        /// <inheritdoc cref="IPlugin.Unload(PluginKind, IPluginContext)"/>
        public void Unload(PluginKind pluginKind, IPluginContext pluginContext)
        {
            switch(pluginKind) {
                case PluginKind.Addon:
                    if(HasAddon) {
                        if(!IsLoadedAddon) {
                            throw new InvalidOperationException(nameof(IsLoadedAddon));
                        }
                        UnloadAddonImpl(pluginContext);
                        IsLoadedAddon = false;
                    } else {
                        throw new NotSupportedException();
                    }
                    break;

                case PluginKind.Theme:
                    if(HasTheme) {
                        if(!IsLoadedTheme) {
                            throw new InvalidOperationException(nameof(IsLoadedTheme));
                        }
                        UnloadThemeImpl(pluginContext);
                        IsLoadedAddon = false;
                    } else {
                        throw new NotSupportedException();
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
        /// <inheritdoc cref="IPlugin.IsLoaded(PluginKind)"/>
        public bool IsLoaded(PluginKind pluginKind)
        {
            switch(pluginKind) {
                case PluginKind.Addon:
                    return IsLoadedAddon;

                case PluginKind.Theme:
                    return IsLoadedTheme;

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion

        #region IAddon

        /// <inheritdoc cref="IAddon.IsSupported(AddonKind)"/>
        public bool IsSupported(AddonKind addonKind)
        {
            LoggingNotSupportAddon();

            return Addon.IsSupported(addonKind);
        }

        /// <inheritdoc cref="IAddon.BuildCommandFinder(IAddonParameter)"/>
        public ICommandFinder BuildCommandFinder(IAddonParameter parameter)
        {
            return BuildSupporttedAddon(AddonKind.CommandFinder, nameof(BuildCommandFinder), parameter, p => Addon.BuildCommandFinder(p));
        }


        #endregion

        #region ITheme

        /// <inheritdoc cref="ITheme.IsSupported(ThemeKind)"/>
        public bool IsSupported(ThemeKind themeKind)
        {
            LoggingNotSupportTheme();

            return Theme.IsSupported(themeKind);
        }

        /// <inheritdoc cref="ITheme.BuildGeneralTheme(IThemeParameter)"/>
        public IGeneralTheme BuildGeneralTheme(IThemeParameter parameter)
        {
            return BuildSupporttedTheme(ThemeKind.General, nameof(BuildGeneralTheme), parameter, p => Theme.BuildGeneralTheme(p));
        }
        /// <inheritdoc cref="ITheme.BuildLauncherGroupTheme(IThemeParameter)"/>
        public ILauncherGroupTheme BuildLauncherGroupTheme(IThemeParameter parameter)
        {
            return BuildSupporttedTheme(ThemeKind.LauncherGroup, nameof(BuildLauncherGroupTheme), parameter, p => Theme.BuildLauncherGroupTheme(p));
        }
        /// <inheritdoc cref="ITheme.BuildLauncherToolbarTheme(IThemeParameter)"/>
        public ILauncherToolbarTheme BuildLauncherToolbarTheme(IThemeParameter parameter)
        {
            return BuildSupporttedTheme(ThemeKind.LauncherToolbar, nameof(BuildLauncherToolbarTheme), parameter, p => Theme.BuildLauncherToolbarTheme(p));
        }
        /// <inheritdoc cref="ITheme.BuildNoteTheme(IThemeParameter)"/>
        public INoteTheme BuildNoteTheme(IThemeParameter parameter)
        {
            return BuildSupporttedTheme(ThemeKind.Note, nameof(BuildNoteTheme), parameter, p => Theme.BuildNoteTheme(p));
        }
        /// <inheritdoc cref="ITheme.BuildCommandTheme(IThemeParameter)"/>
        public ICommandTheme BuildCommandTheme(IThemeParameter parameter)
        {
            return BuildSupporttedTheme(ThemeKind.Command, nameof(BuildCommandTheme), parameter, p => Theme.BuildCommandTheme(p));
        }
        /// <inheritdoc cref="ITheme.BuildNotifyLogTheme(IThemeParameter)"/>
        public INotifyLogTheme BuildNotifyLogTheme(IThemeParameter parameter)
        {
            return BuildSupporttedTheme(ThemeKind.Notify, nameof(BuildNotifyLogTheme), parameter, p => Theme.BuildNotifyLogTheme(p));
        }

        #endregion

    }
}
