namespace ContentTypeTextNet.Pe.PeMain.Data.Temporary
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.Library.PeData.Setting;

	internal class LauncherGroupAndItems
	{
		public LauncherGroupAndItems()
		{
			LauncherGroupSetting = new LauncherGroupSettingModel();
			LauncherItemSetting = new LauncherItemSettingModel();
		}

		#region property

		public LauncherGroupSettingModel LauncherGroupSetting { get; private set; }
		public LauncherItemSettingModel LauncherItemSetting { get; private set; }
		public bool Find { get; set; }

		#endregion
	}
}
