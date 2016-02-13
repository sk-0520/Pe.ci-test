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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
using ContentTypeTextNet.Pe.PeMain.Logic;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
using ContentTypeTextNet.Pe.PeMain.View.Parts.Window;
using ContentTypeTextNet.Pe.PeMain.ViewModel;

namespace ContentTypeTextNet.Pe.PeMain.View
{
    /// <summary>
    /// UpdateWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class UpdateConfirmWindow: ViewModelCommonDataWindow<UpdateConfirmViewModel>
    {
        public UpdateConfirmWindow()
        {
            InitializeComponent();
        }

        #region ViewModelCommonDataWindow

        protected override void CreateViewModel()
        {
            ViewModel = new UpdateConfirmViewModel(this, (Updater)ExtensionData);
        }

        protected override void ApplyViewModel()
        {
            base.ApplyViewModel();

            DataContext = ViewModel;

            ViewModel.SetUpdateDocument(this.webUpdate);
        }

        public override void ApplyLanguage(Dictionary<string, string> map)
        {
            map[LanguageKey.updateNowVersion] = Constants.ApplicationVersion;
            map[LanguageKey.updateNewVersion] = ViewModel.NewVersion;
            map[LanguageKey.updateNewType] = ViewModel.IsRcVersion ? "${version-rc}" : "${version-release}";

            base.ApplyLanguage(map);
        }

        #endregion
    }
}
