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

    [Serializable, DataContract]
    public class FileWatchData: DataBase
    {
        #region property

        /// <summary>
        /// リンク対象ファイル名。
        /// </summary>
        [DataMember]
        public FileInfo? File { get; set; }

        /// <summary>
        /// ファイル変更から実際に読むまでの待機時間。
        /// </summary>
        [DataMember]
        public TimeSpan DelayTime { get; set; } = TimeSpan.FromMilliseconds(500);

        /// <summary>
        /// <see cref="System.IO.FileSystemWatcher.InternalBufferSize"/>。
        /// </summary>
        [DataMember]
        public int BufferSize { get; set; } = 8192;

        /// <summary>
        /// <see cref="System.IO.FileSystemWatcher"/> で取りこぼした際の更新時間。
        /// </summary>
        [DataMember]
        public TimeSpan RefreshTime { get; set; } = TimeSpan.FromMilliseconds(250);
        /// <summary>
        /// そもそも取りこぼしを考慮するか。
        /// <para>将来用。</para>
        /// </summary>
        [DataMember]
        public bool IsEnabledRefresh { get; set; } = true;

        #endregion
    }

    public class FileWatcher : DisposerBase
    {
        #region event

        public event EventHandler<FileChangedEventArgs>? NoteContentChanged;

        #endregion

        public FileWatcher(FileWatchData fileWatchData, ILoggerFactory loggerFactory)
        {
            if(fileWatchData == null) {
                throw new ArgumentNullException($"{nameof(fileWatchData)}");
            }
            if(fileWatchData.File == null) {
                throw new ArgumentNullException($"{nameof(fileWatchData)}.{nameof(fileWatchData.File)}");
            }

            WatchData = fileWatchData;
            Logger = loggerFactory.CreateLogger(GetType());

            DelayWatcher = new LazyAction(WatchData.File.Name, fileWatchData.DelayTime, loggerFactory);
        }

        #region property

        public FileWatchData WatchData { get; }
        protected ILogger Logger { get; }
        FileSystemWatcher? FileSystemWatcher { get; set; }

        LazyAction DelayWatcher { get; }
        #endregion

        #region function

        void OnNoteContentChanged(FileChangedEventArgs e)
        {
            NoteContentChanged?.Invoke(this, e);
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
            Debug.Assert(WatchData.File != null);

            if(FileSystemWatcher == null) {
                FileSystemWatcher = new FileSystemWatcher() {
                    Path = WatchData.File.DirectoryName,
                    Filter = WatchData.File.Name,
                    IncludeSubdirectories = false,
                    InternalBufferSize = WatchData.BufferSize,
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size
                };
                FileSystemWatcher.Changed += FileSystemWatcher_Changed;
            }

            FileSystemWatcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
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
            DelayWatcher.DelayAction(() => {
                var args = new FileChangedEventArgs(new FileInfo(e.FullPath), false);
                OnNoteContentChanged(args);
            });
        }


    }
}
