namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.View;

	public static class ViewUtility
	{
		public static LauncherToolbarWindow CreateToolbarWindow(ScreenModel screen, CommonData commonData)
		{
			var toolbar = new LauncherToolbarWindow();
			toolbar.SetCommonData(commonData, screen);
			
			commonData.AppSender.SendWindowAppend(toolbar);

			return toolbar;
		}
		public static NoteWindow CreateNoteWindow(NoteIndexItemModel noteItem, CommonData commonData)
		{
			var window = new NoteWindow();
			if(!noteItem.Visible) {
				commonData.Logger.Trace("hidden -> show", noteItem);
				noteItem.Visible = true;
			}
			window.SetCommonData(commonData, noteItem);

			commonData.AppSender.SendWindowAppend(window);

			return window;
		}
	}
}
