using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
