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
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
    public class ClipboardFileItemViewModel: ViewModelBase
    {
        #region property

        public string Name { get; set; }
        public string Path { get; set; }
        public ImageSource Image
        {
            get
            {
                var iconPath = new IconPathModel() {
                    Path = this.Path,
                    Index = 0,
                };
                if(FileUtility.Exists(iconPath.Path)) {
                    return AppUtility.LoadIconDefault(iconPath, IconScale.Small);
                } else {
                    return AppResource.GetNotFoundIcon(IconScale.Small);
                }
            }
        }

        #endregion
    }
}
