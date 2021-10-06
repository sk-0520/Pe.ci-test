using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public sealed class StringProgress: IProgress<string>
    {
        public StringProgress(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        ILogger Logger { get; }

        #endregion

        #region IProgress

        public void Report(string value)
        {
            Logger.LogDebug("{0}", value);
        }

        #endregion
    }

    public sealed class DoubleProgress: IProgress<double>
    {
        public DoubleProgress(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        ILogger Logger { get; }

        #endregion

        #region IProgress

        public void Report(double value)
        {
            Logger.LogDebug("{0}", value);
        }

        #endregion
    }
}
