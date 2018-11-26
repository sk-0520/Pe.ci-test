using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Shared.Link.Model
{
    public interface ILogger
    {
        void LogDebug(string formattedSql, object param);
    }

    public interface ILoggerFactory
    {
        ILogger CreateLogger(Type type);
    }
}
