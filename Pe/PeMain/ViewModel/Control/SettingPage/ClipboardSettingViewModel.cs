﻿namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage
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

		const string defineCaptureType = "CaptureType";
		const string defineLimitType = "LimitType";

		#endregion

		public ClipboardSettingViewModel(ClipboardSettingModel clipboard, ClipboardSettingControl view, IAppNonProcess appNonProcess, SettingNotifyData settingNotifiyItem)
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
			set { SetClipboardType(Clipboard, Clipboard.CaptureType, ClipboardType.Text, defineCaptureType); }
		}
		public bool CaptureTypeRtf
		{
			get { return Clipboard.CaptureType.HasFlag(ClipboardType.Rtf); }
			set { SetClipboardType(Clipboard, Clipboard.CaptureType, ClipboardType.Rtf, defineCaptureType); }
		}
		public bool CaptureTypeHtml
		{
			get { return Clipboard.CaptureType.HasFlag(ClipboardType.Html); }
			set { SetClipboardType(Clipboard, Clipboard.CaptureType, ClipboardType.Html, defineCaptureType); }
		}
		public bool CaptureTypeImage
		{
			get { return Clipboard.CaptureType.HasFlag(ClipboardType.Image); }
			set { SetClipboardType(Clipboard, Clipboard.CaptureType, ClipboardType.Image, defineCaptureType); }
		}
		public bool CaptureTypeFiles
		{
			get { return Clipboard.CaptureType.HasFlag(ClipboardType.Files); }
			set { SetClipboardType(Clipboard, Clipboard.CaptureType, ClipboardType.Files, defineCaptureType); }
		}

		#endregion

		#region LimitType

		public bool LimitTypeText
		{
			get { return Clipboard.LimitType.HasFlag(ClipboardType.Text); }
			set { SetClipboardType(Clipboard, Clipboard.LimitType, ClipboardType.Text, defineLimitType); }
		}
		public bool LimitTypeRtf
		{
			get { return Clipboard.LimitType.HasFlag(ClipboardType.Rtf); }
			set { SetClipboardType(Clipboard, Clipboard.LimitType, ClipboardType.Rtf, defineLimitType); }
		}
		public bool LimitTypeHtml
		{
			get { return Clipboard.LimitType.HasFlag(ClipboardType.Html); }
			set { SetClipboardType(Clipboard, Clipboard.LimitType, ClipboardType.Html, defineLimitType); }
		}
		public bool LimitTypeImage
		{
			get { return Clipboard.LimitType.HasFlag(ClipboardType.Image); }
			set { SetClipboardType(Clipboard, Clipboard.LimitType, ClipboardType.Image, defineLimitType); }
		}
		public bool LimitTypeFiles
		{
			get { return Clipboard.LimitType.HasFlag(ClipboardType.Files); }
			set { SetClipboardType(Clipboard, Clipboard.LimitType, ClipboardType.Files, defineLimitType); }
		}

		#endregion

		#region LimitSize

		public int LimitSizeText
		{
			get { return Clipboard.LimitSize.Text; }
			set { SetPropertyValue(Clipboard.LimitSize, value, "Text"); }
		}
		public int LimitSizeRtf
		{
			get { return Clipboard.LimitSize.Rtf; }
			set { SetPropertyValue(Clipboard.LimitSize, value, "Rtf"); }
		}
		public int LimitSizeHtml
		{
			get { return Clipboard.LimitSize.Html; }
			set { SetPropertyValue(Clipboard.LimitSize, value, "Html"); }
		}
		public int LimitSizeImageWidth
		{
			get { return Clipboard.LimitSize.ImageWidth; }
			set { SetPropertyValue(Clipboard.LimitSize, value, "ImageWidth"); }
		}
		public int LimitSizeImageHeight
		{
			get { return Clipboard.LimitSize.ImageHeight; }
			set { SetPropertyValue(Clipboard.LimitSize, value, "ImageHeight"); }
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
