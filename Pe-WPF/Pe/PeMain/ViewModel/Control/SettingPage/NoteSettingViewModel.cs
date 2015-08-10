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

	public class NoteSettingViewModel : SettingPageViewModelBase
	{
		public NoteSettingViewModel(NoteSettingModel note, IAppNonProcess appNonProcess, SettingNotifiyItem settingNotifiyItem)
			: base(appNonProcess, settingNotifiyItem)
		{
			Note = note;
		}

		#region property

		NoteSettingModel Note { get; set; }

		public HotKeyModel CreateHotKey
		{
			get { return Note.CreateHotKey; }
			set { SetPropertyValue(Note, value); }
		}

		public HotKeyModel HideHotKey
		{
			get { return Note.HideHotKey; }
			set { SetPropertyValue(Note, value); }
		}

		public HotKeyModel CompactHotKey
		{
			get { return Note.CompactHotKey; }
			set { SetPropertyValue(Note, value); }
		}

		public HotKeyModel ShowFrontHotKey
		{
			get { return Note.ShowFrontHotKey; }
			set { SetPropertyValue(Note, value); }
		}

		public FontFamily FontFamily
		{
			get { return FontUtility.MakeFontFamily(Note.Font.Family, SystemFonts.MessageFontFamily); }
			set
			{
				if(value != null) {
					var fontFamily = FontUtility.GetOriginalFontFamilyName(value);
					SetPropertyValue(Note.Font, fontFamily, "Family");
				}
			}
		}

		public bool FontBold
		{
			get { return Note.Font.Bold; }
			set { SetPropertyValue(Note.Font, value, "Bold"); }
		}

		public bool FontItalic
		{
			get { return Note.Font.Italic; }
			set { SetPropertyValue(Note.Font, value, "Italic"); }
		}

		public double FontSize
		{
			get { return Note.Font.Size; }
			set { SetPropertyValue(Note.Font, value, "Size"); }
		}
		#endregion
	}
}
