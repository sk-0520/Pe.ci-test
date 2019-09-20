using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Note
{
    public class NoteLinkSelectNotification: FileSaveDialogNotification
    {
        #region property

        public Encoding ResponseEncoding { get; set; }

        #endregion
    }
}
