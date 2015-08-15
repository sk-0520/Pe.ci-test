namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage
{
	using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.Library.PeData.Setting;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
using ContentTypeTextNet.Pe.PeMain.IF;

	public class LauncherItemSettingViewModel: SettingPageLauncherIconCacheViewModelBase, IHavingAppSender
	{
		#region variable

		LauncherListItemsViewModel _launcherItems;
		bool _isItemEdited;

		#endregion

		public LauncherItemSettingViewModel(LauncherItemSettingModel launcherItemSetting, IAppNonProcess appNonProcess, IAppSender appSender, SettingNotifiyItem settingNotifiyItem)
			: base(appNonProcess, settingNotifiyItem)
		{
			LauncherItemSetting = launcherItemSetting;
			AppSender = appSender;
		}

		#region proerty

		LauncherItemSettingModel LauncherItemSetting { get; set; }

		public bool IsItemEdited 
		{
			get { return this._isItemEdited; }
			set { 
				SetVariableValue(ref this._isItemEdited, value);
			}
		}

		public LauncherListItemsViewModel LauncherItems
		{
			get
			{
				if(this._launcherItems == null) {
					this._launcherItems = new LauncherListItemsViewModel(
						LauncherItemSetting.Items,
						AppNonProcess,
						AppSender
					);
				}

				return this._launcherItems;
			}
		}

		#endregion

		#region command

		public ICommand AppendItemCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						AppNonProcess.Logger.Information("AppendItemCommand");
					}
				);

				return result;
			}
		}

		public ICommand RemoveItemCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						AppNonProcess.Logger.Information("RemoveItemCommand");
						var viewModel = o as LauncherListItemViewModel;
						LauncherItems.LauncherItemPairList.Remove(viewModel.Model);
						//LauncherItems.Items.Remove(o);
					}
				);

				return result;
			}
		}

		#endregion

		#region IHavingAppSender

		public IAppSender AppSender { get; private set; }

		#endregion
	}
}
