using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.ReleaseNote
{
    public class ReleaseNoteElement: ElementBase
    {
        public ReleaseNoteElement(NewVersionInfo newVersionInfo, IReadOnlyNewVersionItemData updateItem, bool isCheckOnly, IOrderManager orderManager, IUserAgentManager userAgentManager, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            NewVersionInfoImpl = newVersionInfo;
            NewVersionItem = updateItem;

            IsCheckOnly = isCheckOnly;

            OrderManager = orderManager;
            UserAgentManager = userAgentManager;
        }

        #region property
        private NewVersionInfo NewVersionInfoImpl { get; }
        public IReadOnlyNewVersionInfo NewVersionInfo => NewVersionInfoImpl;

        public IReadOnlyNewVersionItemData NewVersionItem { get; }
        public bool IsCheckOnly { get; private set; }
        private IOrderManager OrderManager { get; }
        private IUserAgentManager UserAgentManager { get; }

        #endregion

        #region function

        /// <summary>
        /// リリースノートを取得する。
        /// </summary>
        /// <remarks>ブラウザ機能でダウンロードしちゃう可能性があるので本体機能で落とす。</remarks>
        /// <returns></returns>
        public async Task<string> LoadReleaseNoteDocumentAsync()
        {
            using(var userAgent = UserAgentManager.CreateAppHttpUserAgent()) {
                return await userAgent.GetStringAsync(NewVersionItem.NoteUri);
            }
        }

        public void StartDownload()
        {
            OrderManager.StartUpdate(UpdateTarget.Application, UpdateProcess.Download);
            IsCheckOnly = false;
        }

        public void StartUpdate()
        {
            OrderManager.StartUpdate(UpdateTarget.Application, UpdateProcess.Update);
        }

        #endregion

        #region ElementBase

        protected override Task InitializeCoreAsync()
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}
