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
    using System.Windows.Input;
    using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
    using ContentTypeTextNet.Pe.Library.PeData.Define;
    using ContentTypeTextNet.Pe.Library.PeData.Item;
    using ContentTypeTextNet.Pe.PeMain.IF;
    using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

    public class ComponentItemViewModel: SingleModelWrapperViewModelBase<ComponentItemModel>, IHasAppNonProcess
    {
        public ComponentItemViewModel(ComponentItemModel model, IAppNonProcess appNonProcess)
            : base(model)
        {
            AppNonProcess = appNonProcess;
        }

        #region proeprty

        public string License { get { return Model.License; } }
        public ComponentKind ComponentKind { get { return Model.Kind; } }

        #endregion

        #region command

        public ICommand OpenComponentCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        try {
                            ExecuteUtility.ExecuteCommand(Model.Uri, AppNonProcess);
                        } catch(Exception ex) {
                            AppNonProcess.Logger.Error(ex);
                        }
                    }
                );

                return result;
            }
        }

        #endregion

        #region SingleModelWrapperViewModelBase

        public override string DisplayText
        {
            get
            {
                return Model.Name;
            }
        }

        #endregion

        #region IHasAppNonProcess

        public IAppNonProcess AppNonProcess { get; private set; }

        #endregion
    }
}
