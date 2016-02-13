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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.Library.PeData.IF;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Window
{
    public class ViewModelCommonDataWindow<TViewModel>: CommonDataWindow, IHasViewModel<TViewModel>
        where TViewModel : ViewModelBase
    {
        public ViewModelCommonDataWindow()
            : base()
        { }

        #region TViewModel

        public TViewModel ViewModel { get; protected set; }

        #endregion

        #region CommonDataWindow

        protected override void CreateViewModel()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
