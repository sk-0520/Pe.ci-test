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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.Define;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
using ContentTypeTextNet.Pe.PeMain.View;
using ContentTypeTextNet.Pe.PeMain.ViewModel.Control;

namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
    /// <summary>
    /// <para>内部でモデルを元保持して最後に再設定する。</para>
    /// </summary>
    public class LauncherItemCustomizeViewModel: LauncherItemEditViewModel, IHasView<LauncherItemCustomizeWindow>, IHasAppSender
    {
        public LauncherItemCustomizeViewModel(LauncherItemModel model, LauncherItemCustomizeWindow view, ScreenModel screen, IAppNonProcess nonPorocess, IAppSender appSender)
            : base((LauncherItemModel)model.DeepClone(), null, null, nonPorocess, appSender)
        {
            View = view;
            Screen = screen;
            SourceModel = model;

            if(HasView) {
                ScreenUtility.AttachmentStartupMoveScreenCenter(view, Screen);
                //View.SourceInitialized += View_SourceInitialized;
            }
        }

        #region proeprty

        LauncherItemModel SourceModel { get; set; }
        ScreenModel Screen { get; set; }

        #endregion

        #region command

        public ICommand SaveCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        SourceModel.Name = Model.Name;
                        SourceModel.LauncherKind = Model.LauncherKind;
                        SourceModel.Command = Model.Command;
                        SourceModel.Option = Model.Option;
                        SourceModel.WorkDirectoryPath = Model.WorkDirectoryPath;
                        SourceModel.Icon = Model.Icon;
                        SourceModel.History = Model.History;
                        SourceModel.Comment = Model.Comment;
                        SourceModel.Tag = Model.Tag;
                        SourceModel.StdStream = Model.StdStream;
                        SourceModel.Administrator = Model.Administrator;
                        SourceModel.EnvironmentVariables = Model.EnvironmentVariables;

                        if(HasView) {
                            View.Close();
                        }
                        SettingUtility.IncrementLauncherItem(Model, null, null, AppNonProcess);
                        AppNonProcess.LauncherIconCaching.Remove(this.SourceModel);
                        AppSender.SendRefreshView(WindowKind.LauncherToolbar, null);
                    }
                );

                return result;
            }
        }

        public ICommand CancelCommand
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

        #endregion

        #region LauncherItemCustomizeWindow

        public LauncherItemCustomizeWindow View { get; private set; }

        public bool HasView { get { return HasViewUtility.GetHasView(this); } }

        #endregion
    }
}
