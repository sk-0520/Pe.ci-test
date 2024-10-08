using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Library.Base
{
    /// <summary>
    /// ファイル監視検知イベント。
    /// </summary>
    public class FileChangedEventArgs: EventArgs
    {
        public FileChangedEventArgs(FileInfo file, bool isRefresh)
        {
            File = file;
            IsRefresh = isRefresh;
        }

        #region property

        public FileInfo File { get; }
        public bool IsRefresh { get; }

        #endregion
    }

    public class FileWatchParameter
    {
        #region property

        /// <summary>
        /// リンク対象ファイル名。
        /// </summary>
        public FileInfo? File { get; set; }

        /// <summary>
        /// ファイル変更から実際に読むまでの待機時間。
        /// </summary>
        public TimeSpan DelayTime { get; set; } = TimeSpan.FromMilliseconds(500);

        /// <summary>
        /// <see cref="FileSystemWatcher.InternalBufferSize"/>。
        /// </summary>
        public int BufferSize { get; set; } = 8192;

        /// <summary>
        /// <see cref="FileSystemWatcher"/> で取りこぼした際の更新時間。
        /// </summary>
        public TimeSpan RefreshTime { get; set; } = TimeSpan.FromMilliseconds(250);
        /// <summary>
        /// そもそも取りこぼしを考慮するか。
        /// </summary>
        /// <remarks>
        /// <para>将来用。</para>
        /// </remarks>
        public bool IsEnabledRefresh { get; set; } = true;

        #endregion
    }

    /// <summary>
    /// ファイル監視処理。
    /// </summary>
    public class FileWatcher: DisposerBase
    {
        #region event

        public event EventHandler<FileChangedEventArgs>? FileContentChanged;

        #endregion

        public FileWatcher(FileWatchParameter watchParameter, ILoggerFactory loggerFactory)
        {
            if(watchParameter == null) {
                throw new ArgumentNullException(nameof(watchParameter));
            }
            if(watchParameter.File == null) {
                throw new ArgumentNullException($"{nameof(watchParameter)}.{nameof(watchParameter.File)}");
            }

            WatchParameter = watchParameter;
            Logger = loggerFactory.CreateLogger(GetType());

            DelayWatcher = new DelayAction(WatchParameter.File.Name, watchParameter.DelayTime, loggerFactory);
        }

        #region property

        public FileWatchParameter WatchParameter { get; }
        protected ILogger Logger { get; }
        private FileSystemWatcher? FileSystemWatcher { get; set; }

        private DelayAction DelayWatcher { get; }

        #endregion

        #region function

        private void OnFileContentChanged(FileChangedEventArgs e)
        {
            ThrowIfDisposed();

            FileContentChanged?.Invoke(this, e);
        }

        private void DisposeFileSystemWatcher()
        {
            if(FileSystemWatcher != null) {
                FileSystemWatcher.Changed -= FileSystemWatcher_Changed;
                FileSystemWatcher.Dispose();
            }
        }

        public void Start()
        {
            Debug.Assert(WatchParameter.File != null);
            ThrowIfDisposed();

            if(FileSystemWatcher == null) {
                Debug.Assert(WatchParameter.File.DirectoryName != null);

                FileSystemWatcher = new FileSystemWatcher() {
                    Path = WatchParameter.File.DirectoryName,
                    Filter = WatchParameter.File.Name,
                    IncludeSubdirectories = false,
                    InternalBufferSize = WatchParameter.BufferSize,
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size
                };
                FileSystemWatcher.Changed += FileSystemWatcher_Changed;
            }

            FileSystemWatcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            ThrowIfDisposed();

            if(FileSystemWatcher != null) {
                FileSystemWatcher.EnableRaisingEvents = false;
            }
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                DisposeFileSystemWatcher();
                FileSystemWatcher = null;
                DelayWatcher.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            if(DelayWatcher.IsDisposed) {
                return;
            }

            DelayWatcher.Callback(() => {
                var args = new FileChangedEventArgs(new FileInfo(e.FullPath), false);
                OnFileContentChanged(args);
            });
        }
    }
}
