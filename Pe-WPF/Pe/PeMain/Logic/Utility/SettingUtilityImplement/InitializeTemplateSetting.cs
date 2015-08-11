namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityImplement
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;

	internal static class InitializeTemplateSetting
	{
		public static void Correction(TemplateSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			V_First(setting, previousVersion, nonProcess);
		}

		static void V_First(TemplateSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			if(previousVersion != null) {
				return;
			}

			setting.Font.Size = Constants.templateFontSize.median;
			setting.ItemsListWidth = Constants.templateItemsListWidth;
			setting.ReplaceListWidth = Constants.templateReplaceListWidth;
			setting.WindowWidth = Constants.templateDefaultWindowSize.Width;
			setting.WindowHeight = Constants.templateDefaultWindowSize.Height;
		}
	}
}
