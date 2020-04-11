using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Manager.Setting
{
    public interface ISettingNotifyManager
    {
        #region event

        #endregion

        #region function

        #endregion
    }

    internal class SettingNotifyManager: ISettingNotifyManager
    {
        public SettingNotifyManager(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        ILogger Logger { get; }

        #endregion

        #region ISettingNotifyManager


        #endregion
    }
}
