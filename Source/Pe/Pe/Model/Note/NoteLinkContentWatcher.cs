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
        #region property

        bool IsRefresh { get; }
        public FileInfo File { get; }
        public Encoding Encoding { get; }

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
        }

        #region property

        NoteLinkContentData LinkData { get; }
        ILogger Logger { get; }

        public FileInfo File
        {
            get
            {
                if(this._file == null) {
                    var filePath = Environment.ExpandEnvironmentVariables(LinkData.FilePath?.Trim() ?? string.Empty);
                    this._file = new FileInfo(filePath);
                }

                return this._file;
            }
        }

        public Encoding Encoding
        {
            get
            {
                if(this._encoding == null) {
                    this._encoding = EncodingUtility.Parse(LinkData.EncodingName);
                }

                return this._encoding;
            }
        }

        #endregion

        #region function

        void OnNoteContentChanged(NoteContentChangedEventArgs e)
        {
            NoteContentChanged?.Invoke(this, e);
        }

        public void Start()
        {

        }

        public void Stop()
        {

        }

        #endregion
    }
}
