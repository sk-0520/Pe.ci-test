namespace ContentTypeTextNet.Pe.PeMain.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.Library.PeData.Define;

	public interface IApplicationDesktopToolbar
	{
		void Docking(DockType dockType);
	}
}
