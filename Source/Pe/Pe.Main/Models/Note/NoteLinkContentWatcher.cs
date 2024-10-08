using System;
using System.IO;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Note
{
    public class NoteContentChangedEventArgs: EventArgs
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

    public class NoteLinkWatchParameter: FileWatchParameter
    {
        #region property

        /// <summary>
        /// 作っといてなんやけど、いるかこれ？
        /// </summary>
        public Encoding? Encoding { get; set; }

        #endregion
    }

    public class NoteLinkWatcher: FileWatcher
    {
        public NoteLinkWatcher(NoteLinkWatchParameter noteLinkWatchParameter, ILoggerFactory loggerFactory)
            : base(noteLinkWatchParameter, loggerFactory)
        { }
    }
}
