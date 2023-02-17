using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;

namespace ContentTypeTextNet.Pe.Main.Models
{
    internal class ViewManager: IViewManager
    {
        #region IViewManager

        public ViewKind GetViewKind(nint hWnd)
        {
            throw new NotImplementedException();
        }

        public bool IsApplicationWindow(ViewKind kind)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
