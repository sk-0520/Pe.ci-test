using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Plugin
{
    public class PluginWebInstallElement: ElementBase, IViewShowStarter, IViewCloseReceiver
    {
        internal PluginWebInstallElement(PluginContainer pluginContainer, NewVersionChecker newVersionChecker, IHttpUserAgentFactory userAgentFactory, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            PluginContainer = pluginContainer;
            NewVersionChecker = newVersionChecker;
            UserAgentFactory = userAgentFactory;
        }

        #region property

        private PluginContainer PluginContainer { get; }
        private NewVersionChecker NewVersionChecker { get; }
        private IHttpUserAgentFactory UserAgentFactory { get; }

        internal string PluginIdOrInfoUrl { get; set; } = string.Empty;

        private bool ViewCreated { get; set; }

        public bool IsDownloaded { get; set; }

        private FileInfo? ArchiveFile { get; set; }

        #endregion

        #region function

        internal FileInfo GetArchiveFile()
        {
            if(ArchiveFile is null) {
                throw new InvalidOperationException();
            }

            return ArchiveFile;
        }

        private async Task<Uri> GetCheckUriAsync(string pluginIdOrInfoUrl)
        {
            if(Guid.TryParse(pluginIdOrInfoUrl, out var guid)) {
                var info = await NewVersionChecker.GetPluginVersionInfoByApiAsync(new PluginId(guid));
                if(info is null) {
                    throw new Exception(Properties.Resources.String_PluginWebInstall_NotFoundByPluginId);
                }
                return new Uri(info.CheckUrl);
            }

            if(!Uri.TryCreate(pluginIdOrInfoUrl, UriKind.Absolute, out var uri)) {
                throw new Exception(Properties.Resources.String_PluginWebInstall_PluginIdOrInfoUrl_UrlParseError);
            }
            if(!(uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == Uri.UriSchemeHttp)) {
                throw new Exception(Properties.Resources.String_PluginWebInstall_PluginIdOrInfoUrl_ProtocolError);
            }

            return uri;
        }

        private async Task GetPluginAsync(string pluginIdOrInfoUrl)
        {
            var checkUri = await GetCheckUriAsync(pluginIdOrInfoUrl);
            var data = await NewVersionChecker.RequestUpdateDataAsync(checkUri);
            if(data is null) {
                throw new Exception(Properties.Resources.String_PluginWebInstall_NewVersionData_NotFound);
            }

            throw new NotImplementedException();
        }

        public Task GetPluginAsync()
        {
            if(string.IsNullOrWhiteSpace(PluginIdOrInfoUrl)) {
                throw new Exception(Properties.Resources.String_PluginWebInstall_PluginIdOrInfoUrl_Empty);
            }

            return GetPluginAsync(PluginIdOrInfoUrl);
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        { }

        #endregion


        #region IViewShowStarter

        public bool CanStartShowView
        {
            get
            {
                if(ViewCreated) {
                    return false;
                }

                return true;
            }
        }

        public void StartView()
        {
            ViewCreated = true;
        }

        #endregion

        #region IWindowCloseReceiver

        public bool ReceiveViewUserClosing()
        {
            return true;
        }
        public bool ReceiveViewClosing()
        {
            return true;
        }

        /// <inheritdoc cref="IViewCloseReceiver.ReceiveViewClosed(bool)"/>
        public void ReceiveViewClosed(bool isUserOperation)
        {
            ViewCreated = false;
        }

        #endregion
    }
}
