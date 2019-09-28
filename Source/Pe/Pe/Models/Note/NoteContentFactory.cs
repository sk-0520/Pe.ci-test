using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Note
{
    public class NoteContentFactory
    {
        #region property

        public Encoding Encoding { get; set; } = EncodingUtility.UTF8n;
        public string RichTextFormat { get; set; } = DataFormats.Rtf;

        #endregion

        #region function

        public string CreatePlain() => string.Empty;

        public string CreateRichText() => @"{\rtf1}";

        [Obsolete]
        public NoteLinkContentData CreateLink() => new NoteLinkContentData() {
            EncodingName = EncodingUtility.ToString(Encoding),
            FilePath = string.Empty,
            DelayTime = TimeSpan.FromSeconds(5),
            BufferSize = 1024 * 4,
            IsEnabledRefresh = false,
            RefreshTime = TimeSpan.FromMinutes(5),
        };

        #endregion
    }
}
