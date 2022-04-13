using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Plugin
{
    public class PluginWebInstallElement: ElementBase, IViewShowStarter, IViewCloseReceiver
    {
        public PluginWebInstallElement(NewVersionChecker newVersionChecker, IHttpUserAgentFactory userAgentFactory, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            NewVersionChecker = newVersionChecker;
            UserAgentFactory = userAgentFactory;
        }

        #region property

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

        public Task GetPluginAsync()
        {
            if(string.IsNullOrWhiteSpace(PluginIdOrInfoUrl)) {
                throw new Exception(Properties.Resources.String_PluginWebInstall_PluginIdOrInfoUrl_Empty);
            }

            return GetPluginAsync(PluginIdOrInfoUrl);
        }

        private Task GetPluginAsync(string pluginIdOrInfoUrl)
        {
            //Uri archiveUri;
            if(!Guid.TryParse(pluginIdOrInfoUrl, out var guid)) {
                NewVersionChecker.CheckPluginNewVersionAsync
            }
            
            throw new NotImplementedException();
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
