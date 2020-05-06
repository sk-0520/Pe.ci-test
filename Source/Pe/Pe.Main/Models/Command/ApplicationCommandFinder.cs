using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.Command;
using Microsoft.Extensions.Logging;
using NLog.Fluent;

namespace ContentTypeTextNet.Pe.Main.Models.Command
{
    internal enum ApplicationCommand
    {
        /// <summary>
        /// コマンドウィンドウを閉じる。
        /// </summary>
        Close,
        /// <summary>
        /// アプリケーションの終了。
        /// <para>アップデートが可能であればアップデート行う</para>
        /// </summary>
        Exit,
        /// <summary>
        /// アプリケーションの終了。
        /// <para>アップデートがあっても終了。</para>
        /// </summary>
        Shutdown,
        /// <summary>
        /// 再起動。
        /// <para>アップデートがあっても再起動。</para>
        /// </summary>
        Reboot,
        About,
        Setting,
        GarbageCollection,
        GarbageCollectionFull,
        CopyShortInformation,
        CopyLongInformation,
        Help,
    }

    internal class ApplicationCommandParameterFactory
    {
        public ApplicationCommandParameterFactory(CommandConfiguration commandConfiguration)
        {
            CommandConfiguration = commandConfiguration;
        }

        #region property

        CommandConfiguration CommandConfiguration { get; }

        #endregion
        #region function

        public ApplicationCommandParameter CreateParameter(ApplicationCommand applicationCommand, Action<IScreen, bool> executor)
        {
            return new ApplicationCommandParameter("header:" + applicationCommand, "desc:" + applicationCommand, iconBox => "icon", executor);
        }

        #endregion
    }

    public class ApplicationCommandParameter
    {
        public ApplicationCommandParameter(string header, string description, Func<IconBox, object> iconGetter, Action<IScreen, bool> executor)
        {
            Header = header ?? throw new ArgumentNullException(nameof(header));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            IconGetter = iconGetter ?? throw new ArgumentNullException(nameof(iconGetter));
            Executor = executor ?? throw new ArgumentNullException(nameof(executor));
        }

        #region property

        public string Header { get; }
        public string Description { get; }
        public Func<IconBox, object> IconGetter { get; }
        public Action<IScreen, bool> Executor { get; }

        #endregion
    }

    public class ApplicationCommandFinder: DisposerBase, ICommandFinder
    {
        #region variable

        bool _isInitialize;

        #endregion

        public ApplicationCommandFinder(IReadOnlyList<ApplicationCommandParameter> parameters, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            Parameters = parameters;
        }

        #region property

        ILoggerFactory LoggerFactory { get; }
        ILogger Logger { get; }

        IReadOnlyList<ApplicationCommandParameter> Parameters { get; }

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

        public bool IsInitialize
        {
            get => this._isInitialize;
            private set => this._isInitialize = value;
        }

        public void Initialize()
        {
            if(IsInitialize) {
                throw new InvalidOperationException(nameof(IsInitialize));
            }

            IsInitialize = true;
        }

        public void Refresh()
        {
            if(!IsInitialize) {
                throw new InvalidOperationException(nameof(IsInitialize));
            }
        }


        public IEnumerable<ICommandItem> ListupCommandItems(string inputValue, Regex inputRegex, IHitValuesCreator hitValuesCreator, CancellationToken cancellationToken)
        {
            if(!IsInitialize) {
                throw new InvalidOperationException(nameof(IsInitialize));
            }

            if(string.IsNullOrWhiteSpace(inputValue)) {
                foreach(var parameter in Parameters) {
                    var element = new ApplicationCommandUtemElement(parameter, LoggerFactory);
                    element.EditableScore = hitValuesCreator.GetScore(ScoreKind.Initial, hitValuesCreator.NoBonus) - 1;
                    yield return element;
                }
            }
        }

        #endregion
    }
}
