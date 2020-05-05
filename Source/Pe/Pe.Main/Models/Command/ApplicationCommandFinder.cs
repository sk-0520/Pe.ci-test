using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Command
{
    sealed class ApplicationCommandParameter
    {
        public ApplicationCommandParameter(string header, string description, Action<IconBox> iconGetter, Action<IScreen, bool> executor)
        {
            Header = header ?? throw new ArgumentNullException(nameof(header));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            IconGetter = iconGetter ?? throw new ArgumentNullException(nameof(iconGetter));
            Executor = executor ?? throw new ArgumentNullException(nameof(executor));
        }

        #region property

        public string Header { get; }
        public string Description { get; }
        public Action<IconBox> IconGetter { get; }
        public Action<IScreen, bool> Executor { get; }

        #endregion
    }

    sealed class ApplicationCommandFinder: DisposerBase, ICommandFinder
    {
        #region variable

        bool _isInitialize;

        #endregion

        public ApplicationCommandFinder(IReadOnlyList<ApplicationCommandParameter> parameters, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            Parameters = parameters;
        }

        #region property

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

            throw new NotImplementedException();
        }

        #endregion
    }
}
