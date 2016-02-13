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
namespace ContentTypeTextNet.Pe.PeMain.View
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
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
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
    using ContentTypeTextNet.Pe.Library.PeData.Item;
    using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
    using ContentTypeTextNet.Pe.PeMain.Define;
    using ContentTypeTextNet.Pe.PeMain.IF;
    using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
    using ContentTypeTextNet.Pe.PeMain.View.Parts.Window;
    using ContentTypeTextNet.Pe.PeMain.ViewModel;

    /// <summary>
    /// LauncherExecuteWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class LauncherItemExecuteWindow: ViewModelCommonDataWindow<LauncherItemExecuteViewModel>, IHasWindowKind
    {
        public LauncherItemExecuteWindow()
        {
            InitializeComponent();
        }

        #region ViewModelCommonDataWindow

        protected override void CreateViewModel()
        {
            var data = (LauncherItemWithScreen)ExtensionData;

            ViewModel = new LauncherItemExecuteViewModel(data.Model, this, data.Screen, CommonData.NonProcess, CommonData.AppSender);
            if(data.Options != null && data.Options.Any()) {
                ViewModel.Option = string.Join(" ", data.Options.WhitespaceToQuotation());
            }
        }

        protected override void ApplyViewModel()
        {
            DataContext = ViewModel;


            base.ApplyViewModel();
        }

        public override void ApplyLanguage(Dictionary<string, string> map)
        {
            CheckUtility.DebugEnforceNotNull(ViewModel);
            CheckUtility.DebugEnforceNotNull(map);

            map[LanguageKey.executeItem] = ViewModel.DisplayText;

            base.ApplyLanguage(map);
        }

        #endregion

        #region IHasWindowKind

        public WindowKind WindowKind { get { return WindowKind.LauncherExecute; } }

        #endregion
    }
}
