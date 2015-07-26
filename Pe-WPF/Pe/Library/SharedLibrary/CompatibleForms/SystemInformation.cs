namespace ContentTypeTextNet.Library.SharedLibrary.CompatibleForms
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Forms = System.Windows.Forms;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.Attribute;
	using ContentTypeTextNet.Library.SharedLibrary.Define;

	public static class SystemInformation
	{
		public static TimeSpan MouseHoverTime { get { return TimeSpan.FromMilliseconds(Forms.SystemInformation.MouseHoverTime); } }
		[PixelKind(Px.Device)]
		public static Size BorderSize { get { return DrawingUtility.Convert(Forms.SystemInformation.BorderSize); } }
	}
}
