using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherGroup;

namespace ContentTypeTextNet.Pe.Main.Model.Manager
{
    public class OrderLauncherGroupElementParameter : OrderElementParameter
    {
        public OrderLauncherGroupElementParameter(Guid launcherGroupId)
            : base(ElementKind.LauncherGroup)
        {
            LauncherGroupId = launcherGroupId;
        }

        #region property

        public Guid LauncherGroupId { get; }

        #endregion
    }

    public class OrderLauncherToolbarElementParameter : OrderElementParameter
    {
        public OrderLauncherToolbarElementParameter(Screen screen, ObservableCollection<LauncherGroupElement> launcherGroups)
            : base(ElementKind.LauncherToolbar)
        {
            Screen = screen;
            LauncherGroups = launcherGroups;
        }

        #region property

        public Screen Screen { get; }
        public ObservableCollection<LauncherGroupElement> LauncherGroups { get; }

        #endregion
    }
}
