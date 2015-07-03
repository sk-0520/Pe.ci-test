namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.Library.PeData.Setting;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public class ToolbarSettingViewModel : ViewModelBase, IHavingNonProcess, IHavingLauncherIconCaching
	{
		#region variable

		LauncherItemsViewModel _launcherItems;
		ObservableCollection<GroupViewModel> _groupTree;
		#endregion

		public ToolbarSettingViewModel(ToolbarItemCollectionModel toolbarItems, LauncherGroupSettingModel groupSettingModel, LauncherItemSettingModel launcherItemSetting, LauncherIconCaching launcherIconCaching, INonProcess nonProcess)
			: base()
		{
			ToolbarItems = toolbarItems;
			GroupSettingModel = groupSettingModel;
			LauncherItemSetting = launcherItemSetting;
			LauncherIconCaching = launcherIconCaching;
			NonProcess = nonProcess;
		}

		#region proerty

		ToolbarItemCollectionModel ToolbarItems { get; set; }

		LauncherGroupSettingModel GroupSettingModel { get; set; }

		LauncherItemSettingModel LauncherItemSetting { get; set; }

		public LauncherItemsViewModel LauncherItems
		{
			get
			{
				if (this._launcherItems == null) {
					this._launcherItems = new LauncherItemsViewModel(
						LauncherItemSetting.Items,
						LauncherIconCaching,
						NonProcess
					);
				}

				return this._launcherItems;
			}
		}

		public IEnumerable<ToolbarViewModel> ToolbarItemList
		{
			get
			{
				foreach (var model in ToolbarItems) {
					var vm = new ToolbarViewModel(model, GroupSettingModel.Groups, LauncherIconCaching, NonProcess);
					yield return vm;
				}
			}
		}

		public IEnumerable<LauncherGroupItemModel> DefaultGroupList
		{
			get
			{
				// TODO: なまえ
				yield return new LauncherGroupItemModel() { Name = "(default)" };

				foreach(var item in GroupSettingModel.Groups) {
					yield return item;
				}
			}
		}

		public ObservableCollection<GroupViewModel> GroupTree
		{
			get
			{
				if(this._groupTree == null) {
					var groupVm = GroupSettingModel.Groups
						.Select(g => new GroupViewModel(g, LauncherItemSetting.Items, LauncherIconCaching, NonProcess))
					;
					this._groupTree = new ObservableCollection<GroupViewModel>(groupVm);
				}

				return this._groupTree;
			}
		}

		#region IHavingLauncherIconCaching

		public LauncherIconCaching LauncherIconCaching { get; private set; }

		#endregion

		#region IHavingNonProcess

		public INonProcess NonProcess { get; private set; }

		#endregion

		#endregion

		#region

		public ICommand CreateGroupCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						var model = SettingUtility.CreateLauncherGroup(GroupSettingModel.Groups, NonProcess);
						GroupSettingModel.Groups.Add(model);
						var vm = new GroupViewModel(model, LauncherItemSetting.Items, LauncherIconCaching, NonProcess);
						this._groupTree.Add(vm);
					}
				);

				return result;
			}
		}

		public ICommand CreateLauncherItemCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						var nodeAndItem = (SelectedNodeAndLauncherItem)o;
						//new GroupItemViewMode(model, LauncherIconCaching, NonProcess);
						NonProcess.Logger.Information(nodeAndItem.ToString());
					}
				);

				return result;
			}
		}

		#endregion
	}
}
