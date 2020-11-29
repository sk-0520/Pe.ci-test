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
        public PluginInstallItemElement(PluginInstallBasicData data, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Data = data;
        }

        #region property

        public PluginInstallBasicData Data { get; }

        #endregion

        #region function

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        { }

        #endregion
    }
}
