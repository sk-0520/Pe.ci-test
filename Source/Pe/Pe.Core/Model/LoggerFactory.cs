using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Model
{
    public class LoggerFactory : ILoggerFactory
    {
        public void AddProvider(ILoggerProvider provider)
        {
            throw new System.NotImplementedException();
        }

        public ILogger CreateLogger(string categoryName)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }

    public static class Lf
    {
        public static ILoggerFactory Create() => new LoggerFactory();
    }

}
