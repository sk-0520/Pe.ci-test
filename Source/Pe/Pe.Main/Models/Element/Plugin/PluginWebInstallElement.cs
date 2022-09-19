using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Plugin
{
    public class PluginWebInstallElement: ElementBase, IViewShowStarter, IViewCloseReceiver
    {
        internal PluginWebInstallElement(PluginContainer pluginContainer, EnvironmentParameters environmentParameters, NewVersionChecker newVersionChecker, NewVersionDownloader newVersionDownloader, IHttpUserAgentFactory userAgentFactory, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            PluginContainer = pluginContainer;
            EnvironmentParameters = environmentParameters;
            NewVersionDownloader = newVersionDownloader;
            NewVersionChecker = newVersionChecker;
            UserAgentFactory = userAgentFactory;
        }

        #region property

        private PluginContainer PluginContainer { get; }
        private EnvironmentParameters EnvironmentParameters { get; }
        private NewVersionDownloader NewVersionDownloader { get; }
        private NewVersionChecker NewVersionChecker { get; }
        private IHttpUserAgentFactory UserAgentFactory { get; }

        internal string PluginIdOrInfoUrl { get; set; } = string.Empty;

        private bool ViewCreated { get; set; }

        public bool IsDownloaded { get; set; }

        internal FileInfo? PluginArchiveFile { get; private set; }

        #endregion

        #region function

        internal FileInfo GetArchiveFile()
        {
            if(PluginArchiveFile is null) {
                throw new InvalidOperationException();
            }

            return PluginArchiveFile;
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
            var notifyProgress = new NullNotifyProgress(LoggerFactory);
            var checkUri = await GetCheckUriAsync(pluginIdOrInfoUrl);
            var updateData = await NewVersionChecker.RequestUpdateDataAsync(checkUri);
            if(updateData is null) {
                throw new Exception(Properties.Resources.String_PluginWebInstall_NewVersionData_NotFound);
            }

            var versionConverter = new VersionConverter();
            var newVersionItem = NewVersionChecker.GetNewVersionItem(new Version(), updateData.Items);
            if(newVersionItem is null) {
                throw new Exception(Properties.Resources.String_PluginWebInstall_NewVersionData_NotFound);
            }

            var baseDirName = PathUtility.ToSafeNameDefault(pluginIdOrInfoUrl);
            var pluginDownloadDirectoryPath = Path.Join(EnvironmentParameters.MachinePluginInstallDirectory.FullName, baseDirName);
            var pluginDownloadFileName = PathUtility.AddExtension(versionConverter.ToFileString(newVersionItem.Version), NewVersionChecker.GetExtension(newVersionItem));
            var pluginArchivePath = Path.Join(pluginDownloadDirectoryPath, pluginDownloadFileName);
            var pluginArchiveFile = new FileInfo(pluginArchivePath);
            pluginArchiveFile.Refresh();

            IOUtility.MakeFileParentDirectory(pluginArchiveFile);
            await NewVersionDownloader.DownloadArchiveAsync(newVersionItem, pluginArchiveFile, notifyProgress);

            pluginArchiveFile.Refresh();

            var checksumOk = await NewVersionDownloader.ChecksumAsync(newVersionItem, pluginArchiveFile, notifyProgress);
            if(!checksumOk) {
                throw new Exception("チェックサム異常あり");
            }

            PluginArchiveFile = pluginArchiveFile;
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
