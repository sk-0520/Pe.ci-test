using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Models
{
    public class FileChangedEventArgs : EventArgs
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
        /// <see cref="System.IO.FileSystemWatcher.InternalBufferSize"/>。
        /// </summary>
        public int BufferSize { get; set; } = 8192;

        /// <summary>
        /// <see cref="System.IO.FileSystemWatcher"/> で取りこぼした際の更新時間。
        /// </summary>
        public TimeSpan RefreshTime { get; set; } = TimeSpan.FromMilliseconds(250);
        /// <summary>
        /// そもそも取りこぼしを考慮するか。
        /// <para>将来用。</para>
        /// </summary>
        public bool IsEnabledRefresh { get; set; } = true;

        #endregion
    }

    public class FileWatcher : DisposerBase
    {
        #region event

        public event EventHandler<FileChangedEventArgs>? FileContentChanged;

        #endregion

        public FileWatcher(FileWatchParameter watchParameter, ILoggerFactory loggerFactory)
        {
            if(watchParameter == null) {
                throw new ArgumentNullException($"{nameof(watchParameter)}");
            }
            if(watchParameter.File == null) {
                throw new ArgumentNullException($"{nameof(watchParameter)}.{nameof(watchParameter.File)}");
            }

            WatchParameter = watchParameter;
            Logger = loggerFactory.CreateLogger(GetType());

            DelayWatcher = new LazyAction(WatchParameter.File.Name, watchParameter.DelayTime, loggerFactory);
        }

        #region property

        public FileWatchParameter WatchParameter { get; }
        protected ILogger Logger { get; }
        FileSystemWatcher? FileSystemWatcher { get; set; }

        LazyAction DelayWatcher { get; }
        #endregion

        #region function

        void OnFileContentChanged(FileChangedEventArgs e)
        {
            ThrowIfDisposed();

            FileContentChanged?.Invoke(this, e);
        }

        void DisposeFileSystemWatcher()
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

            DelayWatcher.DelayAction(() => {
                var args = new FileChangedEventArgs(new FileInfo(e.FullPath), false);
                OnFileContentChanged(args);
            });
        }


    }
}
