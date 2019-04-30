using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Embedded.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;

namespace ContentTypeTextNet.Pe.Main.Model.Note
{
    public class NoteContentChangedEventArgs : EventArgs
    {
        public NoteContentChangedEventArgs(FileInfo file, Encoding encoding, bool isRefresh)
        {
            File = file;
            Encoding = encoding;
            IsRefresh = isRefresh;
        }

        #region property

        public FileInfo File { get; }
        public Encoding Encoding { get; }
        public bool IsRefresh { get; }

        #endregion
    }

    public class NoteLinkContentWatcher : DisposerBase
    {
        #region event

        public event EventHandler<NoteContentChangedEventArgs> NoteContentChanged;

        #endregion

        #region variable

        FileInfo _file;
        Encoding _encoding;

        #endregion

        public NoteLinkContentWatcher(NoteLinkContentData linkData, ILoggerFactory loggerFactory)
        {
            LinkData = linkData;
            Logger = loggerFactory.CreateTartget(GetType());

            DelayWatcher = new LazyAction(File.Name, linkData.DelayTime, Logger.Factory);
        }

        #region property

        NoteLinkContentData LinkData { get; }
        ILogger Logger { get; }

        public FileInfo File
        {
            get
            {
                if(this._file == null) {
                    this._file = LinkData.ToFileInfo();
                }

                return this._file;
            }
        }

        public Encoding Encoding
        {
            get
            {
                if(this._encoding == null) {
                    this._encoding = LinkData.ToEncoding();
                }

                return this._encoding;
            }
        }

        FileSystemWatcher FileSystemWatcher { get; set; }

        LazyAction DelayWatcher { get; }

        #endregion

        #region function

        void OnNoteContentChanged(NoteContentChangedEventArgs e)
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
            if(FileSystemWatcher == null) {
                FileSystemWatcher = new FileSystemWatcher() {
                    Path = File.DirectoryName,
                    Filter = File.Name,
                    IncludeSubdirectories = false,
                    InternalBufferSize = LinkData.BufferSize,
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
                var args = new NoteContentChangedEventArgs(File, Encoding, false);
                OnNoteContentChanged(args);
            });
        }

    }
}
