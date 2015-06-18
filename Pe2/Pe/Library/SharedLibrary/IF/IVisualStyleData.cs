namespace ContentTypeTextNet.Library.SharedLibrary.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF.Marker;

	public interface IVisualStyleData : IWindowsViewExtendRestrictionViewModelMarker
	{
		bool EnabledVisualStyle { get; set; }
	}
}
