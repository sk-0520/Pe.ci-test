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
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.Pe.PeMain.Data.Model
{
    public class HtmlViewerModel: ModelBase
    {
        public HtmlViewerModel()
        {
            WindowStyle = WindowStyle.SingleBorderWindow;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            WindowState = WindowState.Normal;
            WindowWidth = 640;
            WindowHeight = 480;

            ReplaceKeys = new Dictionary<string, string>();
        }

        #region property

        public string TitleLanguageKey { get; set; }

        public double WindowWidth { get; set; }
        public double WindowHeight { get; set; }
        public bool Topmost { get; set; }

        public WindowStyle WindowStyle { get; set; }
        public WindowStartupLocation WindowStartupLocation { get; set; }
        public WindowState WindowState { get; set; }

        public bool IgnoreScriptError { get; set; }
        public bool UsingSystemContextMenu { get; set; }

        public string HtmlSource { get; set; }

        public string CustomStylesheet { get; set; }
        public string CustomScript { get; set; }

        public IDictionary<string, string> ReplaceKeys { get; private set; }

        #endregion
    }
}
