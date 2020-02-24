using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    public class AddonContainer
    {
        public AddonContainer(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
        }

        #region property

        ILogger Logger { get; }
        ILoggerFactory LoggerFactory { get; }

        ISet<IAddon> Addons { get; } = new HashSet<IAddon>();

        #endregion

    }
}
