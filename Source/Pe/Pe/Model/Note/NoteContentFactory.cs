using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Main.Model.Data;

namespace ContentTypeTextNet.Pe.Main.Model.Note
{
    public class NoteContentFactory
    {
        #region property

        public Encoding Encoding { get; set; } = Encoding.UTF8;
        public string RichTextFormat { get; set; } = DataFormats.Rtf;

        #endregion

        #region function

        public string CreatePlain() => string.Empty;

        public string CreateRichText() => @"{\rtf1}";

        public NoteLinkContentData CreateLink() => new NoteLinkContentData() {
            EncodingName = Encoding.WebName,
            FilePath = string.Empty,
            RefreshTime = TimeSpan.FromSeconds(1),
        };

        #endregion
    }
}
