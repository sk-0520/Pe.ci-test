namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Media;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
//	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Property;
	using ContentTypeTextNet.Pe.PeMain.View.Parts.Control.SettingPage;

	public class ClipboardSettingViewModel : SettingPageViewModelBase<ClipboardSettingControl>
	{
		#region variable

		const string defineEnabled = "CaptureType";
		//const string defineSave = "SaveClipboardTypes";

		#endregion

		public ClipboardSettingViewModel(ClipboardSettingModel clipboard, ClipboardSettingControl view, IAppNonProcess appNonProcess, SettingNotifiyItem settingNotifiyItem)
			: base(view, appNonProcess, settingNotifiyItem)
		{
			Clipboard = clipboard;
		}

		#region property

		ClipboardSettingModel Clipboard { get; set; }

		public bool IsEnabled
		{
			get { return Clipboard.IsEnabled; }
			set { SetPropertyValue(Clipboard, value); }
		}

		public bool EnabledApplicationCopy
		{
			get { return Clipboard.IsEnabledApplicationCopy; }
			set { SetPropertyValue(Clipboard, value); }
		}

		public HotKeyModel ToggleHotKey
		{
			get { return Clipboard.ToggleHotKey; }
			set { SetPropertyValue(Clipboard, value); }
		}

		#region CaptureType

		public bool CaptureTypeText
		{
			get { return Clipboard.CaptureType.HasFlag(ClipboardType.Text); }
			set { SetClipboardType(Clipboard, Clipboard.CaptureType, ClipboardType.Text, defineEnabled); }
		}
		public bool CaptureTypeRtf
		{
			get { return Clipboard.CaptureType.HasFlag(ClipboardType.Rtf); }
			set { SetClipboardType(Clipboard, Clipboard.CaptureType, ClipboardType.Rtf, defineEnabled); }
		}
		public bool CaptureTypeHtml
		{
			get { return Clipboard.CaptureType.HasFlag(ClipboardType.Html); }
			set { SetClipboardType(Clipboard, Clipboard.CaptureType, ClipboardType.Html, defineEnabled); }
		}
		public bool CaptureTypeImage
		{
			get { return Clipboard.CaptureType.HasFlag(ClipboardType.Image); }
			set { SetClipboardType(Clipboard, Clipboard.CaptureType, ClipboardType.Image, defineEnabled); }
		}
		public bool CaptureTypeFiles
		{
			get { return Clipboard.CaptureType.HasFlag(ClipboardType.Files); }
			set { SetClipboardType(Clipboard, Clipboard.CaptureType, ClipboardType.Files, defineEnabled); }
		}

		#endregion

		public int SaveCount
		{
			get { return Clipboard.SaveCount; }
			set { SetPropertyValue(Clipboard, value); }
		}

		//public double WaitTimeMs
		//{
		//	get { return Clipboard.WaitTime.TotalMilliseconds; }
		//	set { SetPropertyValue(Clipboard, TimeSpan.FromMilliseconds(value), "WaitTime"); }
		//}

		public TimeSpan WaitTime
		{
			get { return Clipboard.WaitTime; }
			set { SetPropertyValue(Clipboard, value); }
		}

		public int DuplicationCount
		{
			get { return Clipboard.DuplicationCount; }
			set { SetPropertyValue(Clipboard, value); }
		}

		public bool UsingClipboard
		{
			get { return Clipboard.UsingClipboard; }
			set { SetPropertyValue(Clipboard, value); }
		}

		public bool IsTopmost
		{
			get { return Clipboard.IsTopmost; }
			set { SetPropertyValue(Clipboard, value); }
		}

		public bool IsVisible
		{
			get { return Clipboard.IsVisible; }
			set { SetPropertyValue(Clipboard, value); }
		}

		//public FontFamily FontFamily
		//{
		//	get { return FontUtility.MakeFontFamily(Clipboard.Font.Family, SystemFonts.MessageFontFamily); }
		//	set
		//	{
		//		if(value != null) {
		//			var fontFamily = FontUtility.GetOriginalFontFamilyName(value);
		//			SetPropertyValue(Clipboard.Font, fontFamily, "Family");
		//		}
		//	}
		//}

		//public bool FontBold
		//{
		//	get { return Clipboard.Font.Bold; }
		//	set { SetPropertyValue(Clipboard.Font, value, "Bold"); }
		//}

		//public bool FontItalic
		//{
		//	get { return Clipboard.Font.Italic; }
		//	set { SetPropertyValue(Clipboard.Font, value, "Italic"); }
		//}

		//public double FontSize
		//{
		//	get { return Clipboard.Font.Size; }
		//	set { SetPropertyValue(Clipboard.Font, value, "Size"); }
		//}

		#region font

		public FontFamily FontFamily
		{
			get { return FontModelProperty.GetFamilyDefault(Clipboard.Font); }
			set { FontModelProperty.SetFamily(Clipboard.Font, value, OnPropertyChanged); }
		}

		public bool FontBold
		{
			get { return FontModelProperty.GetBold(Clipboard.Font); }
			set { FontModelProperty.SetBold(Clipboard.Font, value, OnPropertyChanged); }
		}

		public bool FontItalic
		{
			get { return FontModelProperty.GetItalic(Clipboard.Font); }
			set { FontModelProperty.SetItalic(Clipboard.Font, value, OnPropertyChanged); }
		}

		public double FontSize
		{
			get { return FontModelProperty.GetSize(Clipboard.Font); }
			set { FontModelProperty.SetSize(Clipboard.Font, value, OnPropertyChanged); }
		}

		#endregion


		#endregion

		#region function

		void SetClipboardType(object obj, ClipboardType nowValue, ClipboardType clipboardType, string memberName, [CallerMemberName]string propertyName = "")
		{
			SetPropertyValue(obj, nowValue ^ clipboardType, memberName, propertyName);
		}

		#endregion
	}
}
