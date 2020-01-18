using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Note
{
    public class NoteLinkChangeRequestParameter : FileDialogRequestParameter
    {
        #region peoperty

        public bool IsOpen { get; set; }
        public Encoding? Encoding { get; set; }
        public IList<Encoding> Encodings { get; set; } = new List<Encoding>();

        #endregion
    }
    public class NoteLinkChangeRequestResponse : FileDialogRequestResponse
    {
        #region peoperty

        public Encoding? Encoding { get; set; }

        #endregion
    }
}
