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
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using ContentTypeTextNet.Library.SharedLibrary.IF;
    using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
    using ContentTypeTextNet.Pe.Library.PeData.Item;
    using ContentTypeTextNet.Pe.PeMain.Data;
    using ContentTypeTextNet.Pe.PeMain.Define;
    using ContentTypeTextNet.Pe.PeMain.IF;
    using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
    using ContentTypeTextNet.Pe.PeMain.View;

    public class NoteMenuViewModel: SingleModelWrapperViewModelBase<NoteIndexItemModel>, INoteMenuItem, IHavingAppSender, IHasNonProcess
    {
        public NoteMenuViewModel(NoteIndexItemModel model, INonProcess nonProcess, IAppSender appSender)
            : base(model)
        {
            NonProcess = nonProcess;
            AppSender = appSender;
        }

        #region property
        #endregion

        #region command



        #endregion

        #region INoteMenuItem

        public string MenuText { get { return NoteUtility.MakeMenuText(Model); } }

        public FrameworkElement MenuIcon { get { return NoteUtility.MakeMenuIcon(Model); } }

        public ICommand NoteMenuSelectedCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        NonProcess.Logger.Information("menu");
                        var window = (NoteWindow)AppSender.SendCreateWindow(WindowKind.Note, Model, null);
                        window.Activate();
                    }
                );

                return result;
            }
        }

        #endregion

        #region IHasNonProcess

        public INonProcess NonProcess { get; private set; }

        #endregion

        #region IHavingAppSender

        public IAppSender AppSender { get; private set; }

        #endregion

        #region SingleModelWrapperViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                NonProcess = null;
                AppSender = null;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
