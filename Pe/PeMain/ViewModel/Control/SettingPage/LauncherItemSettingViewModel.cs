namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Input;
	using ContentTypeTextNet.Library.SharedLibrary.Data;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.Library.PeData.Setting;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
	using ContentTypeTextNet.Pe.PeMain.View.Parts.Control.SettingPage;

	public class LauncherItemSettingViewModel: SettingPageLauncherIconCacheViewModelBase<LauncherItemSettingControl>, IHavingAppSender
	{
		#region variable

		LauncherListViewModel _launcherItems;
		bool _isItemEdited;

		#endregion

		public LauncherItemSettingViewModel(LauncherItemSettingModel launcherItemSetting, LauncherItemSettingControl view, IAppNonProcess appNonProcess, IAppSender appSender, SettingNotifyData settingNotifiyItem)
			: base(view, appNonProcess, settingNotifiyItem)
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

		public LauncherListViewModel LauncherItems
		{
			get
			{
				if(this._launcherItems == null) {
					this._launcherItems = new LauncherListViewModel(
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
						var model = SettingUtility.CreateLauncherItem(LauncherItemSetting.Items, AppNonProcess);
						LauncherItems.LauncherItemPairList.Add(model, null);
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
						var viewModel = o as LauncherListItemViewModel;
						LauncherItems.LauncherItemPairList.Remove(viewModel.Model);
						//LauncherItems.Items.Remove(o);
					}
				);

				return result;
			}
		}

		public ICommand DragOverCommand
		{
			get
			{
				var reslut = CreateCommand(
					o => {
						var eventData = (EventData<DragEventArgs>)o;

						eventData.EventArgs.Effects = DragDropEffects.None;
						
						if(eventData.EventArgs.Data.GetDataPresent(DataFormats.FileDrop)) {
							var filePathList = eventData.EventArgs.Data.GetData(DataFormats.FileDrop) as string[];
							if(filePathList != null && filePathList.Count() == 1) {
								eventData.EventArgs.Effects = DragDropEffects.Copy;
							}
						}

						eventData.EventArgs.Handled = true;
					}
				);

				return reslut;
			}
		}

		public ICommand DragDropCommand
		{
			get
			{
				var reslut = CreateCommand(
					o => {
						var eventData = (EventData<DragEventArgs>)o;
						if(eventData.EventArgs.Data.GetDataPresent(DataFormats.FileDrop)) {
							var filePathList = eventData.EventArgs.Data.GetData(DataFormats.FileDrop) as string[];
							if(filePathList != null && filePathList.Count() == 1) {
								// TODO: LauncherToolbarViewModel.DragDropCommandと重複。いつか見た光景。
								var filePath = filePathList.First();
								var loadShorcut = true;
								if(PathUtility.IsShortcut(filePath)) {
									var dialogResult = MessageBox.Show(
										AppNonProcess.Language["confirm/shortcut/message"],
										AppNonProcess.Language["confirm/shortcut/caption"],
										MessageBoxButton.YesNoCancel,
										MessageBoxImage.Question
									);
									if(dialogResult == MessageBoxResult.Cancel) {
										// やめる
										return;
									}
									loadShorcut = dialogResult == MessageBoxResult.Yes;
								}
								var item = LauncherItemUtility.CreateFromFile(filePath, loadShorcut, AppNonProcess);
								LauncherItems.LauncherItemPairList.Add(item, null);
							}
						}
					}
				);

				return reslut;
			}
		}

		#endregion

		#region IHavingAppSender

		public IAppSender AppSender { get; private set; }

		#endregion
	}
}
