using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Model.Element
{
    public interface IWindowShowStarter
    {
        #region property

        bool CanStartShowWindow { get; }

        #endregion

        #region function

        void StartShowWindow();

        #endregion
    }
}
