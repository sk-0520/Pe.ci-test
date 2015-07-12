namespace ContentTypeTextNet.Library.SharedLibrary.IF.WindowsViewExtend
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media;
	using ContentTypeTextNet.Library.SharedLibrary.IF.Marker;

	public interface IVisualStyleData : IWindowsViewExtendRestrictionViewModelMarker
	{
		bool UsingVisualStyle { get; }
		bool EnabledVisualStyle { get; set; }
		Color VisualPlainColor { get; set; }
		Color VisualAlphaColor { get; set; }
	}
}
