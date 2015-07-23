namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.Define;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.View;

	public static class ViewUtility
	{
		public static LauncherToolbarWindow CreateToolbarWindow(ScreenModel screen, CommonData commonData)
		{
			var toolbar = new LauncherToolbarWindow();
			toolbar.SetCommonData(commonData, screen);
			
			commonData.AppSender.SendAppendWindow(toolbar);

			return toolbar;
		}
	}
}
