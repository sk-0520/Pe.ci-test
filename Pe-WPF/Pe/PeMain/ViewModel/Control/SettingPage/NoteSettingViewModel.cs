namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
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

		#endregion
	}
}
