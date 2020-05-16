using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels
{
    public abstract class ElementViewModelBase<TElement> : SingleModelViewModelBase<TElement>
        where TElement : ElementBase
    {
        protected ElementViewModelBase(TElement model, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            // 初期化完了済みかはデバッグ時に検知すべし
            Debug.Assert(Model.IsInitialized);

            UserTracker = userTracker;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        protected IUserTracker UserTracker { get; }
        protected IDispatcherWrapper DispatcherWrapper { get; }

        #endregion
    }
}
