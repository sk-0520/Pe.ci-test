using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
{
    public sealed class NullLogger : ILogger
    {
        #region ILogger

        public void LogDebug(string formattedSql, object param)
        { }

        #endregion
    }

    public sealed class NullLoggerFactory : ILoggerFactory
    {
        #region ILogger

        public ILogger CreateLogger(Type type) => new NullLogger();

        #endregion
    }

}
