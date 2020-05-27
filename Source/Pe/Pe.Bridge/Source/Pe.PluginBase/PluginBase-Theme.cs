using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.PluginBase
{
    partial class PluginBase
    {
        #region property

        /// <summary>
        /// サポートするテーマ機能を一括定義。
        /// </summary>
        protected abstract IReadOnlyCollection<ThemeKind> SupportedThemeKinds { get; }

        #endregion

        #region function

        private TTheme BuildSupporttedTheme<TArgument, TTheme>(ThemeKind themeKind, string methodName, TArgument argument, Func<TArgument, TTheme> build)
        {
            if(!SupportedThemeKinds.Contains(themeKind)) {
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


        /// <inheritdoc cref="ITheme.BuildGeneralTheme(IThemeParameter)"/>
        protected virtual IGeneralTheme BuildGeneralThemeImpl(IThemeParameter parameter) => throw new NotImplementedException();
        /// <inheritdoc cref="ITheme.BuildLauncherGroupTheme(IThemeParameter)"/>
        protected virtual ILauncherGroupTheme BuildLauncherGroupThemeImpl(IThemeParameter parameter) => throw new NotImplementedException();
        /// <inheritdoc cref="ITheme.BuildLauncherToolbarTheme(IThemeParameter)"/>
        protected virtual ILauncherToolbarTheme BuildLauncherToolbarThemeImpl(IThemeParameter parameter) => throw new NotImplementedException();
        /// <inheritdoc cref="ITheme.BuildNoteTheme(IThemeParameter)"/>
        protected virtual INoteTheme BuildNoteThemeImpl(IThemeParameter parameter) => throw new NotImplementedException();
        /// <inheritdoc cref="ITheme.BuildCommandTheme(IThemeParameter)"/>
        protected virtual ICommandTheme BuildCommandThemeImpl(IThemeParameter parameter) => throw new NotImplementedException();
        /// <inheritdoc cref="ITheme.BuildNotifyLogTheme(IThemeParameter)"/>
        protected virtual INotifyLogTheme BuildNotifyLogThemeImpl(IThemeParameter parameter) => throw new NotImplementedException();

        #endregion

        #region ITheme

        /// <inheritdoc cref="ITheme.IsSupported(ThemeKind)"/>
        public bool IsSupported(ThemeKind themeKind)
        {
            LoggingNotSupportTheme();

            return SupportedThemeKinds.Contains(themeKind);
        }

        /// <inheritdoc cref="ITheme.BuildGeneralTheme(IThemeParameter)"/>
        public IGeneralTheme BuildGeneralTheme(IThemeParameter parameter)
        {
            return BuildSupporttedTheme(ThemeKind.General, nameof(BuildGeneralTheme), parameter, p => BuildGeneralThemeImpl(p));
        }
        /// <inheritdoc cref="ITheme.BuildLauncherGroupTheme(IThemeParameter)"/>

        public ILauncherGroupTheme BuildLauncherGroupTheme(IThemeParameter parameter)
        {
            return BuildSupporttedTheme(ThemeKind.LauncherGroup, nameof(BuildLauncherGroupTheme), parameter, p => BuildLauncherGroupThemeImpl(p));
        }
        /// <inheritdoc cref="ITheme.BuildLauncherToolbarTheme(IThemeParameter)"/>
        public ILauncherToolbarTheme BuildLauncherToolbarTheme(IThemeParameter parameter)
        {
            return BuildSupporttedTheme(ThemeKind.LauncherToolbar, nameof(BuildLauncherToolbarTheme), parameter, p => BuildLauncherToolbarThemeImpl(p));
        }
        /// <inheritdoc cref="ITheme.BuildNoteTheme(IThemeParameter)"/>
        public INoteTheme BuildNoteTheme(IThemeParameter parameter)
        {
            return BuildSupporttedTheme(ThemeKind.Note, nameof(BuildNoteTheme), parameter, p => BuildNoteThemeImpl(p));
        }
        /// <inheritdoc cref="ITheme.BuildCommandTheme(IThemeParameter)"/>
        public ICommandTheme BuildCommandTheme(IThemeParameter parameter)
        {
            return BuildSupporttedTheme(ThemeKind.Command, nameof(BuildCommandTheme), parameter, p => BuildCommandThemeImpl(p));
        }
        /// <inheritdoc cref="ITheme.BuildNotifyLogTheme(IThemeParameter)"/>
        public INotifyLogTheme BuildNotifyLogTheme(IThemeParameter parameter)
        {
            return BuildSupporttedTheme(ThemeKind.Notify, nameof(BuildNotifyLogTheme), parameter, p => BuildNotifyLogThemeImpl(p));
        }

        #endregion
    }
}
