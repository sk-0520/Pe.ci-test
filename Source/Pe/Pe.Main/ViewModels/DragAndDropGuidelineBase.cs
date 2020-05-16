using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels
{
    /// <summary>
    /// たぶんめちゃくちゃになっていくD&D処理のメソッドを整理したかったのではないかと。。。
    /// </summary>
    public abstract class DragAndDropGuidelineBase
    {
        protected DragAndDropGuidelineBase(IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            DispatcherWrapper = dispatcherWrapper;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        protected IDispatcherWrapper DispatcherWrapper { get; }
        protected ILogger Logger { get; }

        #endregion;
    }
}
