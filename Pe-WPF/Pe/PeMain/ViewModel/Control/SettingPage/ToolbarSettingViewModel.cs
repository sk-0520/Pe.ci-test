namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Input;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.Library.PeData.Setting;
	using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
	using ContentTypeTextNet.Pe.PeMain.Define;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public class ToolbarSettingViewModel: SettingPageLauncherIconCacheViewModelBase
	{
		#region variable

		LauncherItemsViewModel _launcherItems;
		CollectionModel<GroupRootViewModel> _groupTree;

		LauncherItemModel _selectedLauncherItem;

		#endregion

		public ToolbarSettingViewModel(ToolbarItemCollectionModel toolbarItems, LauncherGroupSettingModel groupSettingModel, LauncherItemSettingModel launcherItemSetting, IAppNonProcess appNonProcess, SettingNotifiyItem settingNotifiyItem)
			: base(appNonProcess, settingNotifiyItem)
		{
			ToolbarItems = toolbarItems;
			GroupSettingModel = groupSettingModel;
			LauncherItemSetting = launcherItemSetting;
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
						AppNonProcess
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
					var vm = new ToolbarViewModel(model, GroupSettingModel.Groups, AppNonProcess);
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

		public CollectionModel<GroupRootViewModel> GroupTree
		{
			get
			{
				if(this._groupTree == null) {
					var groupVm = GroupSettingModel.Groups
						.Select(g => new GroupRootViewModel(g, LauncherItemSetting.Items, AppNonProcess))
					;
					this._groupTree = new CollectionModel<GroupRootViewModel>(groupVm);
				}

				return this._groupTree;
			}
		}

		public LauncherItemModel SelectedLauncherItem
		{
			get { return this._selectedLauncherItem; }
			set
			{
				if (SetVariableValue(ref this._selectedLauncherItem, value)) {
					if (this._selectedLauncherItem != null) {
						var groupNode = this._groupTree
							.FirstOrDefault(g => g.Nodes.Any(n => n.IsSelected))
						;
						if (groupNode == null) {
							return;
						}
						var targetGroupSetting = GroupSettingModel.Groups[groupNode.Id];
						var itemViewModel = groupNode.Nodes.Single(n => n.IsSelected);
						var selectedIndex = groupNode.Nodes.IndexOf(itemViewModel);

						if(groupNode.Nodes[selectedIndex].Model != this._selectedLauncherItem) {
							targetGroupSetting.LauncherItems[selectedIndex] = this._selectedLauncherItem.Id;

							var insertViewModel = new GroupItemViewMode(this._selectedLauncherItem, AppNonProcess) {
								IsSelected = true,
							};
							foreach (var node in groupNode.Nodes) {
								node.IsSelected = false;
							}
							groupNode.Nodes[selectedIndex] = insertViewModel;
						}
					}
				}
			}
		}

		#endregion

		#region command

		public ICommand CreateGroupCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						var model = SettingUtility.CreateLauncherGroup(GroupSettingModel.Groups, AppNonProcess);
						GroupSettingModel.Groups.Add(model);
						var vm = new GroupRootViewModel(model, LauncherItemSetting.Items, AppNonProcess);
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
						if(nodeAndItem.LauncherItem == null || nodeAndItem.SelectedNode == null) {
							// 何をしろと。
							return;
						}
						if (nodeAndItem.SelectedNode.ToolbarNodeKind == ToolbarNodeKind.Group) {
							// グループに追加
							var groupViewModel = (GroupRootViewModel)nodeAndItem.SelectedNode;
							var groupModel = groupViewModel.Model;
							var target = this._groupTree.Single(g => g == groupViewModel);
							var appendViewModel = new GroupItemViewMode(nodeAndItem.LauncherItem, AppNonProcess);

							groupModel.LauncherItems.Add(nodeAndItem.LauncherItem.Id);
							target.Nodes.Add(appendViewModel);
						} else {
							// 選択ノードの下に追加
							Debug.Assert(nodeAndItem.SelectedNode.ToolbarNodeKind == ToolbarNodeKind.Item);
							var itemViewModel = (GroupItemViewMode)nodeAndItem.SelectedNode;
							var groupViewModel = this._groupTree.First(g => g.Nodes.Any(i => i == itemViewModel));
							var appendViewModel = new GroupItemViewMode(nodeAndItem.LauncherItem, AppNonProcess);
							var groupModel = groupViewModel.Model;
							
							var insertIndex = groupViewModel.Nodes.IndexOf(itemViewModel) + 1;

							groupModel.LauncherItems.Insert(insertIndex, nodeAndItem.LauncherItem.Id);
							groupViewModel.Nodes.Insert(insertIndex, appendViewModel);
						}
					}
				);

				return result;
			}
		}

		public ICommand NodeUpCommand
		{
			get
			{
				var result = CreateCommand(
					o => MoveNode(o, true)
				);

				return result;
			}
		}

		public ICommand NodeDownCommand
		{
			get
			{
				var result = CreateCommand(
					o => MoveNode(o, false)
				);

				return result;
			}
		}

		public ICommand NodeRemoveCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						if(o == null) {
							return;
						}

						var toolbarNode = (IToolbarNode)o;
						if(toolbarNode.ToolbarNodeKind == ToolbarNodeKind.Group) {
							var groupViewModel = (GroupRootViewModel)toolbarNode;
							var groupModel = groupViewModel.Model;

							GroupSettingModel.Groups.Remove(groupModel);
							this._groupTree.Remove(groupViewModel);
						} else {
							Debug.Assert(toolbarNode.ToolbarNodeKind == ToolbarNodeKind.Item);
							var itemViewModel = (GroupItemViewMode)toolbarNode;
							var groupViewModel = this._groupTree.First(g => g.Nodes.Any(i => i == itemViewModel));
							var groupModel = groupViewModel.Model;

							var removeIndex = groupViewModel.Nodes.IndexOf(itemViewModel);

							groupModel.LauncherItems.RemoveAt(removeIndex);
							groupViewModel.Nodes.RemoveAt(removeIndex);
						}
					});

				return result;
			}
		}

		public ICommand SelectedItemChangedCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						if(o != null) {
							var toolbarNode = (IToolbarNode)o;
							if (toolbarNode.ToolbarNodeKind == ToolbarNodeKind.Item) {
								var model = (GroupItemViewMode)toolbarNode;
								SelectedLauncherItem = model.Model;
							}
						}
					}
				);

				return result;
			}
		}

		#endregion

		#region function

		void MoveNode(object o, bool isUp)
		{
			if (o == null) {
				return;
			}
			var toolbarNode = (IToolbarNode)o;
			if(toolbarNode.ToolbarNodeKind == ToolbarNodeKind.Group) {
				var groupViewModel = (GroupRootViewModel)toolbarNode;
				var groupModel = groupViewModel.Model;
				var srcIndex = GroupSettingModel.Groups.IndexOf(groupModel);
				var nextIndex = srcIndex + (isUp ? -1 : +1);

				if (isUp && srcIndex == 0) {
					return;
				}
				if (!isUp && GroupSettingModel.Groups.Count == srcIndex + 1) {
					return;
				}
				var swapModel = nextIndex == 0
					? GroupSettingModel.Groups.First()
					: GroupSettingModel.Groups.Take(nextIndex).First()
				;
				GroupSettingModel.Groups.Remove(groupModel);
				GroupSettingModel.Groups.Insert(nextIndex, groupModel);
				this._groupTree.Remove(groupViewModel);
				this._groupTree.Insert(nextIndex, groupViewModel);
			} else {
				Debug.Assert(toolbarNode.ToolbarNodeKind == ToolbarNodeKind.Item);
				var itemViewModel = (GroupItemViewMode)toolbarNode;
				var groupViewModel = this._groupTree.First(g => g.Nodes.Any(i => i == itemViewModel));
				var targetIdList = GroupSettingModel.Groups[groupViewModel.Id].LauncherItems;
				var srcIndex = targetIdList.IndexOf(itemViewModel.Id);
				var nextIndex = srcIndex + (isUp ? -1 : +1);

				if (isUp && srcIndex == 0) {
					return;
				}
				if (!isUp && targetIdList.Count == srcIndex + 1) {
					return;
				}
				var swapModel = nextIndex == 0
					? targetIdList.First()
					: targetIdList.Take(nextIndex).First()
				;

				targetIdList.Remove(itemViewModel.Id);
				targetIdList.Insert(nextIndex, itemViewModel.Id);
				groupViewModel.Nodes.Remove(itemViewModel);
				groupViewModel.Nodes.Insert(nextIndex, itemViewModel);
			}
			toolbarNode.IsSelected = true;
		}

		#endregion
	}
}
