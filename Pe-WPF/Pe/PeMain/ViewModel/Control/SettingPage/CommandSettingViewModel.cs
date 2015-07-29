namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
	using ContentTypeTextNet.Pe.PeMain.IF;

	public class CommandSettingViewModel: SettingPageViewModelBase
	{
		public CommandSettingViewModel(CommandSettingModel command, IAppNonProcess nonProcess, SettingNotifiyItem settingNotifiyItem)
			: base(nonProcess, settingNotifiyItem)
		{
			Command = command;
		}

		#region property

		CommandSettingModel Command { get; set; }

		public IconScale IconScale 
		{
			get { return Command.IconScale; }
			set { SetPropertyValue(Command, value); }
		}

		public double HideTimeMs
		{
			get { return Command.HideTime.TotalMilliseconds; }
			set { SetPropertyValue(Command, TimeSpan.FromMilliseconds(value), "HideTime"); }
		}

		public HotKeyModel ShowHotkey
		{
			get { return Command.ShowHotkey; }
			set { SetPropertyValue(Command, value); }
		}

		public bool FindId
		{
			get { return Command.FindId; }
			set { SetPropertyValue(Command, value); }
		}

		public bool FindTag
		{
			get { return Command.FindTag; }
			set { SetPropertyValue(Command, value); }
		}

		public bool FindFile
		{
			get { return Command.FindFile; }
			set { SetPropertyValue(Command, value); }
		}


		#endregion
	}
}
