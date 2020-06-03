using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using ContentTypeTextNet.Pe.Bridge.Models;

namespace ContentTypeTextNet.Pe.Bridge.Plugin
{
    /// <summary>
    /// 本体設定機能をサポート。
    /// </summary>
    public interface IPluginSetting
    {
        #region function

        UserControl CreateSettingUI(IPluginContext pluginContext);
        void SaveSetting(UserControl settingControl);

        #endregion
    }
}
