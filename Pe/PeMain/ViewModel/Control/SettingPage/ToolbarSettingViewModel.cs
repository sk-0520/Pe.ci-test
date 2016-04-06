/*
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.Library.PeData.Setting;
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
using ContentTypeTextNet.Pe.PeMain.Define;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
using ContentTypeTextNet.Pe.PeMain.View;
using ContentTypeTextNet.Pe.PeMain.View.Parts.Control.SettingPage;

namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control.SettingPage
{
    public class ToolbarSettingViewModel: SettingPageLauncherIconCacheViewModelBase<ToolbarSettingControl>, IRefreshFromViewModel, IHasAppSender
    {
        #region define

        static readonly string DragNodeFormat = Constants.ApplicationName + "@@Node";

        #endregion

        #region variable

        LauncherListViewModel _launcherItems;
        //TODO: クソみたいな二重管理がつらい
        CollectionModel<GroupRootViewModel> _groupTree;

        LauncherItemModel _selectedLauncherItem;
        ToolbarViewModel _selectedToolbar;

        int _defaultGroupIndex;

        bool _isDragging = false;
        Point _dragStartPosition;

        #endregion

        public ToolbarSettingViewModel(ToolbarSettingModel toolbarSetting, LauncherGroupSettingModel groupSettingModel, LauncherItemSettingModel launcherItemSetting, ToolbarSettingControl view, IAppNonProcess appNonProcess, IAppSender appSender, SettingNotifyData settingNotifiyItem)
            : base(view, appNonProcess, settingNotifiyItem)
        {
            ToolbarSetting = toolbarSetting;
            GroupSettingModel = groupSettingModel;
            LauncherItemSetting = launcherItemSetting;
            AppSender = appSender;

            if(HasView) {
                View.treeGroup.PreviewMouseLeftButtonDown += treeGroup_PreviewMouseLeftButtonDown;
                View.treeGroup.MouseMove += treeGroup_MouseMove;
                View.DragOver += View_DragEnterAndOver;
                View.DragEnter += View_DragEnterAndOver;
                View.Drop += View_Drop;
            }
        }

        #region proerty

        IList<ScreenWindow> ScreenWindowList { get; set; }

        ToolbarSettingModel ToolbarSetting { get; set; }

        LauncherGroupSettingModel GroupSettingModel { get; set; }

        LauncherItemSettingModel LauncherItemSetting { get; set; }

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

        public IEnumerable<ToolbarViewModel> ToolbarItemList
        {
            get
            {
                foreach(var val in ToolbarSetting.Items.Select((m, i) => new { Model = m, Index = i })) {
                    var vm = new ToolbarViewModel(val.Model, GroupSettingModel.Groups, AppNonProcess);
                    if(val.Index == 0 && vm.DefaultGroupId != Guid.Empty) {
                        CallOnPropertyChangeDefaultGroupList(vm.DefaultGroupId);
                    }
                    yield return vm;
                }
            }
        }

        public ToolbarViewModel SelectedToolbar
        {
            get { return this._selectedToolbar; }
            set { SetVariableValue(ref this._selectedToolbar, value); }
        }

        public IEnumerable<LauncherGroupItemModel> DefaultGroupList
        {
            get
            {
                var defaultGroupItem = new LauncherGroupItemModel() {
                    Id = Guid.Empty,
                    Name = AppNonProcess.Language["group/default"],
                };
                SettingUtility.InitializeLauncherGroupItem(defaultGroupItem, null, AppNonProcess);
                yield return defaultGroupItem;

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
                if(SetVariableValue(ref this._selectedLauncherItem, value)) {
                    if(this._selectedLauncherItem != null) {
                        var groupNode = this._groupTree
                            .FirstOrDefault(g => g.Nodes.Any(n => n.IsSelected))
                        ;
                        if(groupNode == null) {
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
                            foreach(var node in groupNode.Nodes) {
                                node.IsSelected = false;
                            }
                            groupNode.Nodes[selectedIndex] = insertViewModel;
                        }
                    }
                }
            }
        }

        public int DefaultGroupIndex
        {
            get { return this._defaultGroupIndex; }
            set { SetVariableValue(ref this._defaultGroupIndex, value); }
        }

        public bool IsDragging
        {
            get { return this._isDragging; }
            set { SetVariableValue(ref this._isDragging, value); }
        }

        #region LauncherItemSetting

        public LauncherItemFileDropMode FileDropMode
        {
            get { return LauncherItemSetting.FileDropMode; }
            set { SetPropertyValue(LauncherItemSetting, value); }
        }

        #endregion

        #endregion

        #region command

        public ICommand CreateGroupCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var model = SettingUtility.CreateLauncherGroup(GroupSettingModel.Groups, AppNonProcess);
                        SettingUtility.UpdateUniqueGuid(model, GroupSettingModel.Groups);
                        GroupSettingModel.Groups.Add(model);
                        var vm = new GroupRootViewModel(model, LauncherItemSetting.Items, AppNonProcess);
                        this._groupTree.Add(vm);

                        //OnPropertyChanged("DefaultGroupList");
                        //SelectedToolbar.DefaultGroupId
                        //OnPropertyChangeDefaultGroupList(SelectedToolbar.DefaultGroupId)
                        CallOnPropertyChangeDefaultGroupList(SelectedToolbar.DefaultGroupId);
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
                        if(nodeAndItem.SelectedNode.ToolbarNodeKind == ToolbarNodeKind.Group) {
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

                            CallOnPropertyChangeDefaultGroupListFromRemove(groupViewModel.Id);

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
                            if(toolbarNode.ToolbarNodeKind == ToolbarNodeKind.Item) {
                                var model = (GroupItemViewMode)toolbarNode;
                                SelectedLauncherItem = model.Model;
                            }
                        }
                    }
                );

                return result;
            }
        }

        public ICommand AppendChildLauncherItemCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var viewModel = o as LauncherListItemViewModel;
                        if(viewModel != null) {
                            //AppNonProcess.Logger.Information(SelectedLauncherItem.ToString());
                            var groupViewModel = this._groupTree.FirstOrDefault(t => t.IsSelected);
                            if(groupViewModel != null) {
                                var target = this._groupTree.Single(g => g == groupViewModel);
                                var appendViewModel = new GroupItemViewMode(viewModel.Model, AppNonProcess);
                                groupViewModel.Model.LauncherItems.Add(viewModel.Model.Id);
                                target.Nodes.Add(appendViewModel);
                            }
                        }
                    }
                );

                return result;
            }
        }

        public ICommand ShowScreenCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        ScreenWindowList = Screen
                            .AllScreens
                            .Select(s => new ItemWithScreen<IModel>(default(IModel), s))
                            .Select(d => AppSender.SendCreateWindow(WindowKind.Screen, d, null))
                            .Cast<ScreenWindow>()
                            .ToList()
                        ;

                        foreach(var window in ScreenWindowList) {
                            window.MouseUp += CloseScreenWindow;
                            window.KeyUp += CloseScreenWindow;
                            window.Show();
                            window.UpdateLayout();
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
            if(o == null) {
                return;
            }
            var toolbarNode = (IToolbarNode)o;
            if(toolbarNode.ToolbarNodeKind == ToolbarNodeKind.Group) {
                var groupViewModel = (GroupRootViewModel)toolbarNode;
                var groupModel = groupViewModel.Model;
                var srcIndex = GroupSettingModel.Groups.IndexOf(groupModel);
                var nextIndex = srcIndex + (isUp ? -1 : +1);

                if(isUp && srcIndex == 0) {
                    return;
                }
                if(!isUp && GroupSettingModel.Groups.Count == srcIndex + 1) {
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
                CallOnPropertyChangeDefaultGroupList(SelectedToolbar.DefaultGroupId);
            } else {
                Debug.Assert(toolbarNode.ToolbarNodeKind == ToolbarNodeKind.Item);
                var itemViewModel = (GroupItemViewMode)toolbarNode;
                var groupViewModel = this._groupTree.First(g => g.Nodes.Any(i => i == itemViewModel));
                var targetIdList = GroupSettingModel.Groups[groupViewModel.Id].LauncherItems;
                var srcIndex = targetIdList.IndexOf(itemViewModel.Id);
                var nextIndex = srcIndex + (isUp ? -1 : +1);

                if(isUp && srcIndex == 0) {
                    return;
                }
                if(!isUp && targetIdList.Count == srcIndex + 1) {
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

        private void CallOnPropertyChangeDefaultGroupListFromRemove(Guid prevDefaultId)
        {
            OnPropertyChanged(nameof(DefaultGroupList));
            if(SelectedToolbar.DefaultGroupId == Guid.Empty) {
                // 値設定するとバインドが死ぬのでインデックス指定
                DefaultGroupIndex = 0;
                // BUGS: "DefaultGroupId" が何を指すのか分からん
                //OnPropertyChanged("DefaultGroupId");
            } else if(SelectedToolbar.DefaultGroupId == prevDefaultId) {
                SelectedToolbar.DefaultGroupId = Guid.Empty;
                //OnPropertyChanged("DefaultGroupId");
            }
            //OnPropertyChanged("DefaultGroupList");
        }

        void CallOnPropertyChangeDefaultGroupList(Guid groupId)
        {
            OnPropertyChanged(nameof(DefaultGroupList));
            DefaultGroupIndex = DefaultGroupList
                .Select((g, i) => new { Id = g.Id, Index = i })
                .FirstOrDefault(p => groupId == p.Id)
                .Index
            ;
        }

        void CloseScreenWindow(object sender, EventArgs e)
        {
            foreach(var window in ScreenWindowList) {
                window.Close();
            }
            ScreenWindowList = null;
        }

        /// <summary>
        /// 座標からデータを取得。
        /// </summary>
        /// <param name="treeView">指定ツリービュー。</param>
        /// <param name="position">ツリービューの原点を基点とした座標。</param>
        /// <returns></returns>
        IToolbarNode GetToolbarNode(TreeView treeView, Point position)
        {
            var node = treeView.InputHitTest(position);

            var hitTestResults = VisualTreeHelper.HitTest(treeView, position);
            IToolbarNode result = null;
            if(hitTestResults != null) {
                CastUtility.AsAction<FrameworkElement>(hitTestResults.VisualHit, element => {
                    result = element.DataContext as IToolbarNode;
                });
            }

            return result;
        }

        #endregion

        #region IHavingAppSender

        public IAppSender AppSender { get; private set; }

        #endregion

        #region IRefreshFromViewModel

        public void Refresh()
        {
            //foreach(var vm in LauncherItems.Items) {
            //	vm.Refresh();
            //}
            this._launcherItems = null;
            OnPropertyChanged(nameof(LauncherItems));

            foreach(var node in this._groupTree.SelectMany(t => t.Nodes)) {
                node.Refresh();
            }
        }

        #endregion

        /// <summary>
        /// <para>http://stackoverflow.com/questions/1026179/drag-drop-in-treeview</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void treeGroup_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //var treeView = (TreeView)sender;
            //this._dragStartPosition = e.GetPosition(treeView);
            //e.Handled = true;
            this._dragStartPosition = e.GetPosition(null);
        }

        void treeGroup_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton != MouseButtonState.Pressed) {
                return;
            }

            if(!IsDragging) {
                //IInputElement dropNode = View.treeGroup.InputHitTest(this._dragStartPosition);
                //Debug.WriteLine(dropNode);
                var isScrollDrag = GetToolbarNode(View.treeGroup, e.GetPosition(View.treeGroup)) == null;
                if(isScrollDrag) {
                    return;
                }

                var nowPosition = e.GetPosition(null);
                var size = new Size(10, 10);

                var isDragX = Math.Abs(nowPosition.X - this._dragStartPosition.X) > size.Width;
                var isDragY = Math.Abs(nowPosition.Y - this._dragStartPosition.Y) > size.Height;
                if(isDragX || isDragY) {
                    var treeView = (TreeView)sender;
                    treeView.AllowDrop = true;
                    CastUtility.AsAction<IToolbarNode>(treeView.SelectedItem, selectedNode => {
                        var item = new DataObject(DragNodeFormat, selectedNode);
                        IsDragging = true;
                        DragDrop.DoDragDrop(treeView, item, DragDropEffects.Move);
                        IsDragging = false;
                        treeView.AllowDrop = false;
                    });
                }
            }
        }

        void View_DragEnterAndOver(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DragNodeFormat)) {
                var srcNode = (IToolbarNode)e.Data.GetData(DragNodeFormat);
                CheckUtility.DebugEnforce(HasView);
                var dstNode = GetToolbarNode(View.treeGroup, e.GetPosition(View.treeGroup));
                if(dstNode != null && srcNode != dstNode) {
                    switch(srcNode.ToolbarNodeKind) {
                        case ToolbarNodeKind.Group:
                            if(dstNode.ToolbarNodeKind == ToolbarNodeKind.Group) {
                                e.Effects = DragDropEffects.Move;
                            } else {
                                e.Effects = DragDropEffects.None;
                            }
                            break;
                        case ToolbarNodeKind.Item:
                            if(dstNode.ToolbarNodeKind == ToolbarNodeKind.Group) {
                                var srcItemViewModel = (GroupItemViewMode)srcNode;
                                var srcGroupViewModel = this._groupTree.First(g => g.Nodes.Any(i => i == srcItemViewModel));
                                if(dstNode == srcGroupViewModel) {
                                    e.Effects = DragDropEffects.None;
                                    break;
                                }
                            }
                            e.Effects = DragDropEffects.Move;
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                } else {
                    e.Effects = DragDropEffects.None;
                }
            } else {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        void View_Drop(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DragNodeFormat)) {
                var srcNode = (IToolbarNode)e.Data.GetData(DragNodeFormat);
                CheckUtility.DebugEnforce(HasView);
                var dstNode = GetToolbarNode(View.treeGroup, e.GetPosition(View.treeGroup));
                if(dstNode != null && srcNode != dstNode) {
                    switch(srcNode.ToolbarNodeKind) {
                        case ToolbarNodeKind.Group:
                            // グループの移動
                            if(dstNode.ToolbarNodeKind == ToolbarNodeKind.Group) {
                                var srcGroupViewModel = (GroupRootViewModel)srcNode;
                                GroupSettingModel.Groups.Remove(srcGroupViewModel.Model);
                                this._groupTree.Remove(srcGroupViewModel);

                                var dstGroupViewModel = (GroupRootViewModel)dstNode;
                                var dstIndex = GroupSettingModel.Groups.IndexOf(dstGroupViewModel.Id);
                                if(dstIndex != -1) {
                                    GroupSettingModel.Groups.Insert(dstIndex, srcGroupViewModel.Model);
                                    this._groupTree.Insert(dstIndex, srcGroupViewModel);
                                    srcGroupViewModel.IsSelected = true;
                                }
                                CallOnPropertyChangeDefaultGroupList(SelectedToolbar.DefaultGroupId);
                            }
                            break;
                        case ToolbarNodeKind.Item:
                            // アイテムの移動
                            {
                                var srcItemViewModel = (GroupItemViewMode)srcNode;
                                var srcGroupViewModel = this._groupTree.First(g => g.Nodes.Any(i => i == srcItemViewModel));
                                srcGroupViewModel.Nodes.Remove(srcItemViewModel);
                                GroupSettingModel.Groups[srcGroupViewModel.Id].LauncherItems.Remove(srcItemViewModel.Id);

                                if(dstNode.ToolbarNodeKind == ToolbarNodeKind.Group) {
                                    // 親に追加
                                    var dstGroupViewModel = (GroupRootViewModel)dstNode;
                                    dstGroupViewModel.Nodes.Add(srcItemViewModel);
                                    GroupSettingModel.Groups[dstGroupViewModel.Id].LauncherItems.Add(srcItemViewModel.Id);
                                } else {
                                    // 指定アイテムに割り込む形で移動
                                    var dstItemViewModel = (GroupItemViewMode)dstNode;
                                    var dstGroupViewModel = this._groupTree.First(g => g.Nodes.Any(i => i == dstItemViewModel));
                                    var dstIndex = dstGroupViewModel.Nodes.IndexOf(dstItemViewModel);
                                    if(dstIndex != -1) {
                                        dstGroupViewModel.Nodes.Insert(dstIndex, srcItemViewModel);
                                        GroupSettingModel.Groups[dstGroupViewModel.Id].LauncherItems.Insert(dstIndex, srcItemViewModel.Id);
                                    }
                                }
                            }
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
        }

    }
}
