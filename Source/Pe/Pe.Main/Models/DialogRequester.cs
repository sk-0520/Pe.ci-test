using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Element.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using Microsoft.Extensions.Logging;
using ContentTypeTextNet.Pe.Library.Base.Linq;

namespace ContentTypeTextNet.Pe.Main.Models
{
    public class FileSystemSelectDialogRequestParameter: FileDialogRequestParameter
    {
        #region property

        /// <summary>
        /// ファイルを選択するか。
        /// </summary>
        public FileSystemDialogMode FileSystemDialogMode { get; set; }

        #endregion
    }

    /// <summary>
    /// ファイルダイアログモード。
    /// </summary>
    public enum FileSystemDialogMode
    {
        /// <summary>
        /// 開く。
        /// </summary>
        FileOpen,
        /// <summary>
        /// 保存。
        /// </summary>
        FileSave,
        /// <summary>
        /// ディレクトリ。
        /// </summary>
        Directory,
    }

    public class FileSystemSelectDialogRequestResponse: FileDialogRequestResponse
    {
        #region property
        #endregion
    }

    public class IconSelectDialogRequestParameter: RequestParameter
    {
        #region property

        public string FileName { get; set; } = string.Empty;
        public int IconIndex { get; set; }

        #endregion
    }

    public class IconSelectDialogRequestResponse: CancelResponse
    {
        #region property

        public string FileName { get; set; } = string.Empty;
        public int IconIndex { get; set; }

        #endregion
    }

    public class DialogRequester
    {
        public DialogRequester(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        private ILogger Logger { get; }

        #endregion

        #region function

        public string ExpandPath(string? path) => Environment.ExpandEnvironmentVariables(path ?? string.Empty);

        public DialogFilterItem CreateAllFilter() => new DialogFilterItem(Properties.Resources.String_FileDialog_Filter_Common_All, string.Empty, "*");

        private void SelectFileSystem(IRequestSender requestSender, string path, FileSystemDialogMode fileSystemDialogMode, IEnumerable<DialogFilterItem> filters, Action<FileSystemSelectDialogRequestResponse> response)
        {
            var parameter = new FileSystemSelectDialogRequestParameter() {
                FilePath = path,
                FileSystemDialogMode = fileSystemDialogMode,
            };

            if(filters.Any()) {
                parameter.Filter.SetRange(filters);
            }

            requestSender.Send<FileSystemSelectDialogRequestResponse>(parameter, r => {
                if(r.ResponseIsCancel) {
                    Logger.LogTrace("cancel");
                    return;
                }
                response(r);
            });
        }

        /// <summary>
        /// ファイル選択。
        /// </summary>
        /// <param name="requestSender"></param>
        /// <param name="path"></param>
        /// <param name="filters"></param>
        /// <param name="response"><see cref="CancelResponse.ResponseIsCancel"/>は真。</param>
        public void SelectFile(IRequestSender requestSender, string path, bool isOpen, IEnumerable<DialogFilterItem> filters, Action<FileSystemSelectDialogRequestResponse> response)
        {
            var mode = isOpen ? FileSystemDialogMode.FileOpen : FileSystemDialogMode.FileSave;
            SelectFileSystem(requestSender, path, mode, filters, response);
        }
        public void SelectDirectory(IRequestSender requestSender, string path, Action<FileSystemSelectDialogRequestResponse> response)
        {
            SelectFileSystem(requestSender, path, FileSystemDialogMode.Directory, Enumerable.Empty<DialogFilterItem>(), response);
        }

        public void SelectIcon(IRequestSender requestSender, string path, int index, Action<IconSelectDialogRequestResponse> response)
        {
            var parameter = new IconSelectDialogRequestParameter() {
                FileName = path,
                IconIndex = index,
            };
            requestSender.Send<IconSelectDialogRequestResponse>(parameter, r => {
                if(r.ResponseIsCancel) {
                    Logger.LogTrace("cancel");
                    return;
                }
                response(r);
            });
        }

        #endregion
    }

    public class PluginWebInstallRequestParameter: RequestParameter, IDisposable
    {
        #region property

        public PluginWebInstallElement Element { get; init; } = default!;
        public IWindowManager WindowManager { get; init; } = default!;
        public IUserTracker UserTracker { get; init; } = default!;
        public IDispatcherWrapper DispatcherWrapper { get; init; } = default!;
        public ILoggerFactory LoggerFactory { get; init; } = default!;

        #endregion

        #region IDisposable

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if(!this._disposedValue) {
                if(disposing) {
                    Element.Dispose();
                }

                this._disposedValue = true;
            }
        }

        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }

    public class PluginWebInstallRequestResponse: CancelResponse
    {
        #region variable

        private FileInfo? _archiveFile;

        #endregion

        #region property

        public FileInfo ArchiveFile
        {
            get
            {
                if(ResponseIsCancel) {
                    throw new InvalidOperationException();
                }
                Debug.Assert(this._archiveFile is not null);

                return this._archiveFile;
            }
            set
            {
                ArgumentNullException.ThrowIfNull(value);

                this._archiveFile = value;
            }
        }

        #endregion
    }

}
