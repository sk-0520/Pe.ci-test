using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    /// <summary>
    /// インストール対象プラグイン。
    /// </summary>
    public class PluginInstallItemElement: ElementBase
    {
        public PluginInstallItemElement(PluginInstallData data, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Data = data;
        }

        #region property

        public PluginInstallData Data { get; }

        #endregion

        #region function

        #endregion

        #region ElementBase

        protected override Task InitializeCoreAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}
