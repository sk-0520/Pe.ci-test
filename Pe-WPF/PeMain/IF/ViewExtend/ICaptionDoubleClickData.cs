namespace ContentTypeTextNet.Pe.PeMain.IF.ViewExtend
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF.Marker;

	public interface ICaptionDoubleClickData: IWindowsViewExtendRestrictionViewModelMarker
	{
		void OnCaptionDoubleClick(object sender, CancelEventArgs e);
	}
}
