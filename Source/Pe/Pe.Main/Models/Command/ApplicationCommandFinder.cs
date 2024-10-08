using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Element.Command;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Library.Base;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using ContentTypeTextNet.Pe.Library.Base.Linq;

namespace ContentTypeTextNet.Pe.Main.Models.Command
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    internal class CommandDescriptionAttribute: Attribute
    {
        public CommandDescriptionAttribute(string resourceName)
            : this(resourceName, false)
        { }


        public CommandDescriptionAttribute(string resourceName, bool isExtend)
        {
            ResourceName = resourceName;
            IsExtend = isExtend;
        }

        #region property

        public string ResourceName { get; }
        public bool IsExtend { get; }

        #endregion
    }

    public enum ApplicationCommand
    {
        /// <summary>
        /// コマンドウィンドウを閉じる。
        /// </summary>
        [CommandDescription(nameof(Properties.Resources.String_ApplicationCommand_Description_Close))]
        Close,
        /// <summary>
        /// アプリケーションの終了。
        /// </summary>
        /// <remarks>
        /// <para>通常はアップデートが可能であればアップデート行う</para>
        /// <para>拡張機能: アップデートがあっても終了する。</para>
        /// </remarks>
        [CommandDescription(nameof(Properties.Resources.String_ApplicationCommand_Description_Exit), false)]
        [CommandDescription(nameof(Properties.Resources.String_ApplicationCommand_Description_Exit_Extend), true)]
        Exit,
        /// <summary>
        /// 再起動。
        /// </summary>
        /// <remarks>
        /// <para>アップデートがあっても再起動。</para>
        /// </remarks>
        [CommandDescription(nameof(Properties.Resources.String_ApplicationCommand_Description_Reboot))]
        Reboot,
        [CommandDescription(nameof(Properties.Resources.String_ApplicationCommand_Description_About))]
        About,
        [CommandDescription(nameof(Properties.Resources.String_ApplicationCommand_Description_Setting))]
        Setting,
        [CommandDescription(nameof(Properties.Resources.String_ApplicationCommand_Description_GarbageCollection), false)]
        [CommandDescription(nameof(Properties.Resources.String_ApplicationCommand_Description_GarbageCollection_Extend), true)]
        GarbageCollection,
        [CommandDescription(nameof(Properties.Resources.String_ApplicationCommand_Description_CopyInformation), false)]
        [CommandDescription(nameof(Properties.Resources.String_ApplicationCommand_Description_CopyInformation_Extend), true)]
        CopyInformation,
        [CommandDescription(nameof(Properties.Resources.String_ApplicationCommand_Description_Proxy))]
        Proxy,
        [CommandDescription(nameof(Properties.Resources.String_ApplicationCommand_Description_Help))]
        Help,
        [CommandDescription(nameof(Properties.Resources.String_ApplicationCommand_Description_Exception))]
        Exception,
    }

    internal class ApplicationCommandParameterFactory
    {
        public ApplicationCommandParameterFactory(CommandConfiguration commandConfiguration, IDispatcherWrapper dispatcherWrapper)
        {
            CommandConfiguration = commandConfiguration;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        private CommandConfiguration CommandConfiguration { get; }
        private IDispatcherWrapper DispatcherWrapper { get; }

        #endregion

        #region function

        private string ToHeader(ApplicationCommand applicationCommand)
        {
            var rawValue = CommandConfiguration.Application.Mapping[applicationCommand];
            var joinedValue = string.Join(CommandConfiguration.Application.Separator, rawValue.Split());
            return CommandConfiguration.Application.Prefix + joinedValue;
        }

        private (string narmal, string extend) ToDescriptions(ApplicationCommand applicationCommand)
        {
            // テスト側でもろもろ担保
            var descriptionAttributes = applicationCommand.GetType().GetField(applicationCommand.ToString())!.GetCustomAttributes<CommandDescriptionAttribute>().ToList();
            if(descriptionAttributes.Count == 1) {
                var singleValue = Properties.Resources.ResourceManager.GetString(descriptionAttributes[0].ResourceName, CultureInfo.InvariantCulture)!;
                return (singleValue, singleValue);
            } else {
                var a = Properties.Resources.ResourceManager.GetString(descriptionAttributes[0].ResourceName, CultureInfo.InvariantCulture)!;
                var b = Properties.Resources.ResourceManager.GetString(descriptionAttributes[1].ResourceName, CultureInfo.InvariantCulture)!;
                if(descriptionAttributes[1].IsExtend) {
                    return (a, b);
                } else {
                    return (b, a);
                }
            }
        }

        public ApplicationCommandParameter CreateParameter(ApplicationCommand applicationCommand, Action<ICommandExecuteParameter> executor)
        {
            DispatcherWrapper.VerifyAccess();

            var descriptions = ToDescriptions(applicationCommand);
            return new ApplicationCommandParameter(ToHeader(applicationCommand), descriptions.narmal, descriptions.extend, (in IconScale iconScale) => {
                var control = new Control();
                using(Initializer.Begin(control)) {
                    control.Template = (ControlTemplate)Application.Current.Resources["App-Image-Command"];
                    control.Style = iconScale.Box switch {
                        IconBox.Small => (Style)Application.Current.Resources["Image-Small"],
                        IconBox.Normal => (Style)Application.Current.Resources["Image-Normal"],
                        IconBox.Big => (Style)Application.Current.Resources["Image-Big"],
                        IconBox.Large => (Style)Application.Current.Resources["Image-Large"],
                        _ => throw new NotImplementedException()
                    };
                }
                return control;

            }, executor);
        }

        #endregion
    }

    public delegate object IconGetter(in IconScale iconScale);

    public class ApplicationCommandParameter
    {
        public ApplicationCommandParameter(string header, string description, string extendDescription, IconGetter iconGetter, Action<ICommandExecuteParameter> executor)
        {
            Header = header ?? throw new ArgumentNullException(nameof(header));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            ExtendDescription = extendDescription ?? throw new ArgumentNullException(nameof(extendDescription));
            IconGetter = iconGetter ?? throw new ArgumentNullException(nameof(iconGetter));
            Executor = executor ?? throw new ArgumentNullException(nameof(executor));
        }

        #region property

        public string Header { get; }
        public string Description { get; }
        public string ExtendDescription { get; }
        public IconGetter IconGetter { get; }
        public Action<ICommandExecuteParameter> Executor { get; }

        #endregion
    }

    public class ApplicationCommandFinder: DisposerBase, ICommandFinder
    {
        #region variable

        private bool _isInitialize;

        #endregion

        public ApplicationCommandFinder(IReadOnlyList<ApplicationCommandParameter> parameters, CommandConfiguration commandConfiguration, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            DispatcherWrapper = dispatcherWrapper;

            Parameters = parameters;
            CommandConfiguration = commandConfiguration;
        }

        #region property

        /// <inheritdoc cref="ILoggerFactory"/>
        private ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        private ILogger Logger { get; }
        /// <inheritdoc cref="IDispatcherWrapper"/>
        private IDispatcherWrapper DispatcherWrapper { get; }
        /// <inheritdoc cref="CommandConfiguration"/>
        private CommandConfiguration CommandConfiguration { get; }
        private IReadOnlyList<ApplicationCommandParameter> Parameters { get; }

        #endregion

        #region function

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {

            }

            base.Dispose(disposing);
        }
        #endregion

        #region ICommandFinder

        public bool IsInitialized
        {
            get => this._isInitialize;
            private set => this._isInitialize = value;
        }

        public void Initialize()
        {
            if(IsInitialized) {
                throw new InvalidOperationException(nameof(IsInitialized));
            }

            IsInitialized = true;
        }

        public void Refresh(IPluginContext pluginContext)
        {
            Debug.Assert(pluginContext.GetType() == typeof(NullPluginContext));

            if(!IsInitialized) {
                throw new InvalidOperationException(nameof(IsInitialized));
            }
        }


        public async IAsyncEnumerable<ICommandItem> EnumerateCommandItemsAsync(string inputValue, Regex inputRegex, IHitValuesCreator hitValuesCreator, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if(!IsInitialized) {
                throw new InvalidOperationException(nameof(IsInitialized));
            }

            if(string.IsNullOrWhiteSpace(inputValue)) {
                foreach(var parameter in Parameters) {
                    var element = new ApplicationCommandItemElement(parameter, DispatcherWrapper, LoggerFactory);
                    await element.InitializeAsync(cancellationToken);
                    element.EditableScore = hitValuesCreator.GetScore(ScoreKind.Initial, hitValuesCreator.NoBonus) - 1;
                    yield return element;
                }
                yield break;
            }


            if(!inputValue.StartsWith(CommandConfiguration.Application.Prefix, StringComparison.Ordinal)) {
                yield break;
            }

            foreach(var parameter in Parameters) {
                var parameterMatches = hitValuesCreator.GetMatches(parameter.Header, inputRegex);
                if(parameterMatches.Any()) {
                    var ranges = hitValuesCreator.ConvertRanges(parameterMatches);
                    var hitValue = hitValuesCreator.ConvertHitValues(parameter.Header, ranges);

                    var element = new ApplicationCommandItemElement(parameter, DispatcherWrapper, LoggerFactory);
                    await element.InitializeAsync(cancellationToken);

                    element.EditableHeaderValues.SetRange(hitValue);
                    element.EditableScore = hitValuesCreator.CalcScore(parameter.Header, element.EditableHeaderValues);
                    yield return element;
                }
            }
        }

        #endregion
    }
}
