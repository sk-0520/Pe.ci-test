using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    /// <summary>
    /// インストール対象プラグイン。
    /// </summary>
    public class PluginInstallItemElement: ElementBase
    {
        public PluginInstallItemElement(IPluginInformations pluginInformations, PluginInstallMode pluginInstallMode, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Informations = pluginInformations;
            InstallMode = pluginInstallMode;
        }

        #region property

        public IPluginInformations Informations { get; }
        public PluginInstallMode InstallMode { get; }

        #endregion

        #region function

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
