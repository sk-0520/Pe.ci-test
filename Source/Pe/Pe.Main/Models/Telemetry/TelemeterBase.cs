using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Telemetry
{
    /// <summary>
    /// 追跡用基底処理。
    /// </summary>
    /// <remarks>
    /// <para>名前ほど大層なことではないけどどこで何が起き方を知っておきたい。</para>
    /// </remarks>
    public abstract class TelemeterBase
    {
        protected TelemeterBase(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
        }

        #region property

        /// <inheritdoc cref="ILoggerFactory"/>
        protected ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        protected ILogger Logger { get; }

        #endregion
    }
}
