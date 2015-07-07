namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;

	public class CommandSettingViewModel: SettingPageViewModelBase
	{
		public CommandSettingViewModel(CommandSettingModel command, INonProcess nonProcess)
			: base(nonProcess)
		{
			Command = command;
		}

		#region property

		CommandSettingModel Command { get; set; }

		public HotkeyModel ShowHotkey
		{
			get { return Command.ShowHotkey; }
			set { SetPropertyValue(Command, value); }
		}

		#endregion
	}
}
