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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Pe.Library.PeData.Item;

namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control
{
    public class EnvironmentVariablesEditViewModel: SingleModelWrapperViewModelBase<EnvironmentVariablesItemModel>, IHasNonProcess
    {
        public EnvironmentVariablesEditViewModel(EnvironmentVariablesItemModel model, INonProcess nonProcess)
            : base(model)
        {
            NonProcess = nonProcess;
        }

        #region property

        public bool Edit
        {
            get { return Model.Edit; }
            set { SetModelValue(value); }
        }

        public EnvironmentVariableUpdateItemCollectionModel Update
        {
            get { return Model.Update; }
        }

        public string Remove
        {
            get { return string.Join(Environment.NewLine, Model.Remove); }
            set
            {
                var removes = value.SplitLines();
                Model.Remove = new CollectionModel<string>(removes);
            }
        }

        #endregion

        #region IHasNonProcess

        public INonProcess NonProcess { get; private set; }

        #endregion
    }
}
