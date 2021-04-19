using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Main.Models.Note
{
    public class NoteLinkChangeRequestParameter: FileDialogRequestParameter
    {
        #region property

        public bool IsOpen { get; set; }
        public Encoding? Encoding { get; set; }
        public IList<Encoding> Encodings { get; set; } = new List<Encoding>();

        #endregion
    }
    public class NoteLinkChangeRequestResponse: FileDialogRequestResponse
    {
        #region property

        public Encoding? Encoding { get; set; }

        #endregion
    }
}
