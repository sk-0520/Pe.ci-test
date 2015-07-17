namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Data.Temporary;

	public class ClipboardSettingViewModel : SettingPageViewModelBase
	{
		#region variable

		const string defineEnabled = "EnabledClipboardTypes";
		const string defineSave = "SaveClipboardTypes";

		#endregion

		public ClipboardSettingViewModel(ClipboardSettingModel clipboard, INonProcess nonProcess, VariableConstants variableConstants, SettingNotifiyItem settingNotifiyItem)
			: base(nonProcess, variableConstants, settingNotifiyItem)
		{
			Clipboard = clipboard;
		}

		#region property

		ClipboardSettingModel Clipboard { get; set; }

		public bool Enabled
		{
			get { return Clipboard.Enabled; }
			set { SetPropertyValue(Clipboard, value); }
		}

		public bool EnabledApplicationCopy
		{
			get { return Clipboard.EnabledApplicationCopy; }
			set { SetPropertyValue(Clipboard, value); }
		}

		public bool SaveClipboard
		{
			get { return Clipboard.SaveClipboard; }
			set { SetPropertyValue(Clipboard, value); }
		}

		public HotKeyModel ToggleHotKey
		{
			get { return Clipboard.ToggleHotKey; }
			set { SetPropertyValue(Clipboard, value); }
		}

		#region EnabledClipboardTypes

		public bool EnabledClipboardTypesText
		{
			get { return GetBoolean(Clipboard.EnabledClipboardTypes, ClipboardType.Text); }
			set { SetClipboardType(Clipboard, Clipboard.EnabledClipboardTypes, ClipboardType.Text, defineEnabled); }
		}
		public bool EnabledClipboardTypesRtf
		{
			get { return GetBoolean(Clipboard.EnabledClipboardTypes, ClipboardType.Rtf); }
			set { SetClipboardType(Clipboard, Clipboard.EnabledClipboardTypes, ClipboardType.Rtf, defineEnabled); }
		}
		public bool EnabledClipboardTypesHtml
		{
			get { return GetBoolean(Clipboard.EnabledClipboardTypes, ClipboardType.Html); }
			set { SetClipboardType(Clipboard, Clipboard.EnabledClipboardTypes, ClipboardType.Html, defineEnabled); }
		}
		public bool EnabledClipboardTypesImage
		{
			get { return GetBoolean(Clipboard.EnabledClipboardTypes, ClipboardType.Image); }
			set { SetClipboardType(Clipboard, Clipboard.EnabledClipboardTypes, ClipboardType.Image, defineEnabled); }
		}
		public bool EnabledClipboardTypesFile
		{
			get { return GetBoolean(Clipboard.EnabledClipboardTypes, ClipboardType.File); }
			set { SetClipboardType(Clipboard, Clipboard.EnabledClipboardTypes, ClipboardType.File, defineEnabled); }
		}

		#endregion

		public int SaveCount
		{
			get { return Clipboard.SaveCount; }
			set { SetPropertyValue(Clipboard, value); }
		}

		public double WaitTimeMs
		{
			get { return Clipboard.WaitTime.TotalMilliseconds; }
			set { SetPropertyValue(Clipboard, TimeSpan.FromMilliseconds(value), "WaitTime"); }
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

		public bool TopMost
		{
			get { return Clipboard.TopMost; }
			set { SetPropertyValue(Clipboard, value); }
		}

		public bool Visible
		{
			get { return Clipboard.Visible; }
			set { SetPropertyValue(Clipboard, value); }
		}

		#endregion

		#region function

		bool GetBoolean(ClipboardType clipboardType, ClipboardType value)
		{
			return clipboardType.HasFlag(value);
		}
		void SetClipboardType(object obj, ClipboardType nowValue, ClipboardType clipboardType, string memberName, [CallerMemberName]string propertyName = "")
		{
			SetPropertyValue(obj, nowValue ^ clipboardType, memberName, propertyName);
		}
		#endregion
	}
}
