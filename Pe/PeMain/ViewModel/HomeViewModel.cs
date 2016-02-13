/**
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
namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
    using ContentTypeTextNet.Library.SharedLibrary.Model;
    using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
    using ContentTypeTextNet.Pe.Library.PeData.Item;
    using ContentTypeTextNet.Pe.Library.PeData.Setting;
    using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
    using ContentTypeTextNet.Pe.PeMain.IF;
    using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
    using ContentTypeTextNet.Pe.PeMain.View;
    using ContentTypeTextNet.Pe.PeMain.View.Parts;

    public class HomeViewModel: HasViewModelBase<HomeWindow>, IHasAppNonProcess
    {
        public HomeViewModel(HomeWindow view, LauncherGroupSettingModel launcherGroupSetting, LauncherItemSettingModel launcherItemSetting, IAppNonProcess appNonProcess)
            : base(view)
        {
            LauncherGroupSetting = launcherGroupSetting;
            LauncherItemSetting = launcherItemSetting;
            AppNonProcess = appNonProcess;

            LogList = new List<object>();
        }

        #region property

        LauncherGroupSettingModel LauncherGroupSetting { get; set; }
        LauncherItemSettingModel LauncherItemSetting { get; set; }

        public IList<object> LogList { get; private set; }

        public bool IsAppReload { get; private set; }

        #endregion

        #region command

        public ICommand CloseCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        if(HasView) {
                            View.Close();
                        }
                    }
                );

                return result;
            }
        }

        public ICommand ShowNotifyAreaCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        SystemExecuteUtility.OpenNotificationAreaHistory(AppNonProcess);
                    }
                );

                return result;
            }
        }

        public ICommand ResistStartupCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var startupPath = Environment.ExpandEnvironmentVariables(Constants.StartupShortcutPath);

                        var image = MessageBoxImage.Information;
                        string message;
                        if(!File.Exists(startupPath)) {
                            try {
                                AppUtility.MakeAppShortcut(startupPath);
                                message = AppNonProcess.Language["home/startup/regist/create"];
                            } catch(Exception ex) {
                                AppNonProcess.Logger.Error(ex);
                                message = ex.Message;
                                image = MessageBoxImage.Error;
                            }
                        } else {
                            message = AppNonProcess.Language["home/startup/regist/exists"];
                            AppNonProcess.Logger.Information(message, startupPath);
                        }
                        MessageBox.Show(message, AppNonProcess.Language["home/startup/regist/title"], MessageBoxButton.OK, image);
                    }
                );

                return result;
            }
        }

        public ICommand ResistItemsCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        if(LauncherItemSetting.Items.Any()) {
                            var dialogResult = MessageBox.Show(AppNonProcess.Language["home/auto-regist/merge/message"], AppNonProcess.Language["home/auto-regist/merge/title"], MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
                            if(dialogResult != MessageBoxResult.OK) {
                                return;
                            }
                        }
                        var createItems = CreateAutoLauncherItems();
                        if(createItems.Find) {
                            IsAppReload = true;
                            // 現行に組み合わせる
                            LauncherItemSetting.Items.AddRange(createItems.LauncherItemSetting.Items);
                            if(LauncherGroupSetting.Groups.Count == 1 && !LauncherGroupSetting.Groups.First().LauncherItems.Any()) {
                                // なんもなければ消しておく
                                LauncherGroupSetting.Groups.Clear();
                            }
                            LauncherGroupSetting.Groups.AddRange(createItems.LauncherGroupSetting.Groups);

                            MessageBox.Show(AppNonProcess.Language["home/auto-regist/registed/message"], AppNonProcess.Language["home/auto-regist/registed/title"], MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                );

                return result;
            }
        }

        #endregion

        #region function

        LauncherGroupAndItems CreateAutoLauncherItems()
        {
            var result = new LauncherGroupAndItems();

            // alluser/user programs
            var allUsersFiles =
                Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu))
                .Concat(Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms)))
            ;
            var nowUserFiles =
                Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu))
                .Concat(Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Programs)))
            ;
            var programMenuFiles = nowUserFiles
                .Concat(allUsersFiles)
                .Where(s => PathUtility.IsShortcut(s) || PathUtility.IsShortcut(s))
                .Distinct()
            ;
            var programItems = new List<LauncherItemModel>();
            foreach(var file in programMenuFiles) {
                result.Find = true;
                try {
                    var item = LauncherItemUtility.CreateFromFile(file, true, AppNonProcess);
                    programItems.Add(item);
                } catch(Exception ex) {
                    // #68 暫定回避
                    LogList.Add(ex);
                }
            }
            var programGroupItem = new LauncherGroupItemModel();
            SettingUtility.InitializeLauncherGroupItem(programGroupItem, null, AppNonProcess);
            programGroupItem.Name = AppNonProcess.Language["home/auto-regist/group/program"];
            foreach(var item in programItems) {
                programGroupItem.LauncherItems.Add(item.Id);
            }

            var groupItems = new[] {
                programGroupItem,
            };
            foreach(var group in groupItems) {
                result.LauncherGroupSetting.Groups.Add(group);
            }

            var launcherItems = programItems;
            foreach(var item in launcherItems) {
                result.LauncherItemSetting.Items.Add(item);
            }

            return result;
        }

        #endregion

        #region IHasAppNonProcess

        public IAppNonProcess AppNonProcess { get; private set; }

        #endregion
    }
}
