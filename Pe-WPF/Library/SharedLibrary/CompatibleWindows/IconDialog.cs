namespace ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.PInvoke.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using Microsoft.Win32;

	public class IconDialog: CommonDialog
	{
		public IconDialog()
		{
			Reset();
		}

		#region property

		public IconPathModel Icon { get; private set; }

		#endregion

		#region CommonDialog

		public override void Reset()
		{
			Icon = new IconPathModel();
		}

		protected override bool RunDialog(IntPtr hwndOwner)
		{
			var iconIndex = Icon.Index;
			var sb = new StringBuilder(Icon.Path, (int)MAX.MAX_PATH);
			var result = NativeMethods.SHChangeIconDialog(hwndOwner, sb, sb.Capacity, ref iconIndex);
			if(result) {
				Icon.Index = iconIndex;
				Icon.Path = sb.ToString();
			}

			return result;
		}

		#endregion
	}
}
