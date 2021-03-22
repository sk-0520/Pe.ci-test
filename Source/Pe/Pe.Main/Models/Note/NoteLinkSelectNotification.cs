using System.Text;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Main.Models.Note
{
    public class NoteLinkSelectNotification: FileSaveDialogRequestResponse
    {
        #region property

        public Encoding? ResponseEncoding { get; set; }

        #endregion
    }
}
