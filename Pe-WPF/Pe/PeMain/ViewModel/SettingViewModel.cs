namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.View;
	using ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage;

	public class SettingViewModel: ViewModelBase, IHavingCommonData, IHavingView<SettingWindow>
	{
		#region variable

		MainSettingViewModel _mainSetting;
		LauncherItemSettingViewModel _launcherItemSetting;
		ToolbarSettingViewModel _toolbarSetting;
		CommandSettingViewModel _commandSetting;

		#endregion

		public SettingViewModel(CommonData commonData, SettingWindow view)
		{
			CommonData = commonData;
			View = view;
		}

		#region property

		public MainSettingViewModel MainSetting
		{
			get
			{
				if(this._mainSetting == null) {
					this._mainSetting = new MainSettingViewModel(
						CommonData.MainSetting.RunningInformation,
						CommonData.MainSetting.Language,
						CommonData.MainSetting.Logging,
						CommonData.NonProcess
					);
				}

				return this._mainSetting;
			}
		}

		public LauncherItemSettingViewModel LauncherItemSetting
		{
			get
			{
				if(this._launcherItemSetting == null) {
					this._launcherItemSetting = new LauncherItemSettingViewModel(
						CommonData.LauncherItemSetting,
						CommonData.LauncherIconCaching,
						CommonData.NonProcess
					);
				}

				return this._launcherItemSetting;
			}
		}

		public ToolbarSettingViewModel ToolbarSetting
		{
			get
			{
				if (this._toolbarSetting == null) {
					this._toolbarSetting = new ToolbarSettingViewModel(
						CommonData.MainSetting.Toolbar,
						CommonData.LauncherGroupSetting,
						CommonData.LauncherItemSetting,
						CommonData.LauncherIconCaching,
						CommonData.NonProcess
					);
				}

				return this._toolbarSetting;
			}
		}

		public CommandSettingViewModel CommandSetting
		{
			get
			{
				if(this._commandSetting == null) {
					this._commandSetting = new CommandSettingViewModel(
						CommonData.NonProcess
					);
				}

				return this._commandSetting;
			}
		}

		#region IHavingCommonData

		public CommonData CommonData { get; private set; }

		#endregion

		#region IHavingView

		public SettingWindow View { get; private set; }
		public bool HasView { get { return HavingViewUtility.GetHasView(this); } }

		#endregion

		#endregion

		#region command

		public ICommand CancelCommand
		{
			get
			{
				var reslut = CreateCommand(
					o => {
						View.Close();
					}
				);

				return reslut;
			}
		}

		#endregion
	}
}
