using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Model.Element
{
    public interface IViewShowStarter
    {
        #region property

        bool CanStartShowView { get; }

        #endregion

        #region function

        void StartView();

        #endregion
    }
}
