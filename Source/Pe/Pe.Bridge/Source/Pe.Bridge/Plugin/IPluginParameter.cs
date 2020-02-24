using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Bridge.Plugin
{
    public interface IPluginParameter
    {
        #region property

        IPlatformTheme PlatformTheme { get; }
        IDispatcherWrapper DispatcherWrapper { get; }
        ILoggerFactory LoggerFactory { get; }

        #endregion
    }
}
