using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// 無効なパーセント(0-1)進行状況の更新のプロバイダー。
    /// </summary>
    public sealed class NullDoubleProgress: IProgress<double>
    {
        public NullDoubleProgress(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        private ILogger Logger { get; }

        #endregion

        #region IProgress

        public void Report(double value)
        {
            Logger.LogDebug("{0}", value);
        }

        #endregion
    }

    /// <summary>
    /// 無効な文字列進行状況の更新のプロバイダー。
    /// </summary>
    public sealed class NullStringProgress: IProgress<string>
    {
        public NullStringProgress(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        private ILogger Logger { get; }

        #endregion

        #region IProgress

        public void Report(string value)
        {
            Logger.LogDebug("{0}", value);
        }

        #endregion
    }
}
