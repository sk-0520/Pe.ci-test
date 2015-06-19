namespace ContentTypeTextNet.Library.SharedLibrary.CompatibleForms
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Forms = System.Windows.Forms;
	using System.Threading.Tasks;

	public static class SystemInformation
	{
		public static TimeSpan MouseHoverTime { get { return TimeSpan.FromMilliseconds(Forms.SystemInformation.MouseHoverTime); } }
	}
}
