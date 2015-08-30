namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Media;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Property;

	public class TemplateSettingViewModel : SettingPageViewModelBase
	{
		public TemplateSettingViewModel(TemplateSettingModel template, IAppNonProcess appNonProcess, SettingNotifiyItem settingNotifiyItem)
			: base(appNonProcess, settingNotifiyItem)
		{
			Template = template;
		}

		#region property

		TemplateSettingModel Template { get; set; }

		public bool IsTopmost
		{
			get { return Template.IsTopmost; }
			set { SetPropertyValue(Template, value); }
		}

		public bool IsVisible
		{
			get { return Template.IsVisible; }
			set { SetPropertyValue(Template, value); }
		}

		public HotKeyModel ToggleHotKey
		{
			get { return Template.ToggleHotKey; }
			set { SetPropertyValue(Template, value); }
		}

		//public FontFamily FontFamily
		//{
		//	get { return FontUtility.MakeFontFamily(Template.Font.Family, SystemFonts.MessageFontFamily); }
		//	set
		//	{
		//		if(value != null) {
		//			var fontFamily = FontUtility.GetOriginalFontFamilyName(value);
		//			SetPropertyValue(Template.Font, fontFamily, "Family");
		//		}
		//	}
		//}

		//public bool FontBold
		//{
		//	get { return Template.Font.Bold; }
		//	set { SetPropertyValue(Template.Font, value, "Bold"); }
		//}

		//public bool FontItalic
		//{
		//	get { return Template.Font.Italic; }
		//	set { SetPropertyValue(Template.Font, value, "Italic"); }
		//}

		//public double FontSize
		//{
		//	get { return Template.Font.Size; }
		//	set { SetPropertyValue(Template.Font, value, "Size"); }
		//}

		#region font

		public FontFamily FontFamily
		{
			get { return FontModelProperty.GetFamilyDefault(Template.Font); }
			set { FontModelProperty.SetFamily(Template.Font, value, OnPropertyChanged); }
		}

		public bool FontBold
		{
			get { return FontModelProperty.GetBold(Template.Font); }
			set { FontModelProperty.SetBold(Template.Font, value, OnPropertyChanged); }
		}

		public bool FontItalic
		{
			get { return FontModelProperty.GetItalic(Template.Font); }
			set { FontModelProperty.SetItalic(Template.Font, value, OnPropertyChanged); }
		}

		public double FontSize
		{
			get { return FontModelProperty.GetSize(Template.Font); }
			set { FontModelProperty.SetSize(Template.Font, value, OnPropertyChanged); }
		}

		#endregion


		#endregion
	}
}
