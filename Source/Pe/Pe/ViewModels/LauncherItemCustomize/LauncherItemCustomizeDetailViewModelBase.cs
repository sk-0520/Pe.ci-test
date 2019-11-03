using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize
{
    public abstract class LauncherItemCustomizeDetailViewModelBase : SingleModelViewModelBase<LauncherItemCustomizeElement>, ILauncherItemId
    {
        public LauncherItemCustomizeDetailViewModelBase(LauncherItemCustomizeElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property
        #endregion

        #region command
        #endregion

        #region function

        protected abstract void InitializeImpl();

        public void Initialize()
        {
            InitializeImpl();
        }

        protected string ExpandPath(string? path) => Environment.ExpandEnvironmentVariables(path ?? string.Empty);
        protected DialogFilterItem CreateAllFilter() => new DialogFilterItem("all", string.Empty, "*");
        /// <summary>
        /// ファイル選択。
        /// </summary>
        /// <param name="requestSender"></param>
        /// <param name="path"></param>
        /// <param name="isFile"></param>
        /// <param name="filters"></param>
        /// <param name="response"><see cref="LauncherFileSelectRequestResponse.ResponseIsCancel"/>は真。</param>
        protected void SelectFile(RequestSender requestSender, string path, bool isFile, IEnumerable<DialogFilterItem> filters, Action<LauncherFileSelectRequestResponse> response)
        {
            var parameter = new LauncherFileSelectRequestParameter() {
                FilePath = path,
                IsFile = isFile,
            };

            if(filters.Any()) {
                parameter.Filter.SetRange(filters);
            }

            requestSender.Send<LauncherFileSelectRequestResponse>(parameter, r => {
                if(r.ResponseIsCancel) {
                    Logger.LogTrace("cancel");
                    return;
                }
                response(r);
            });
        }

        #endregion

        #region SingleModelViewModelBase
        #endregion

        #region ILauncherItemId

        public Guid LauncherItemId => Model.LauncherItemId;

        #endregion
    }
}
