using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Telemetry
{
    /// <summary>
    /// 追跡用基底処理。
    /// <para>名前ほど大層なことではないけどどこで何が起き方を知っておきたい。</para>
    /// </summary>
    public abstract class TelemeterBase
    {
        protected TelemeterBase(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
        }

        #region property

        protected ILoggerFactory LoggerFactory { get; }
        protected ILogger Logger { get; }

        #endregion
    }
}
