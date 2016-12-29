using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.IF.WindowsViewExtend;
using ContentTypeTextNet.Library.SharedLibrary.View.ViewExtend;

namespace ContentTypeTextNet.Pe.PeMain.View.Parts.ViewExtend
{
    public class ThemeStyle: VisualStyle
    {
        public ThemeStyle(System.Windows.Window view, IVisualStyleData restrictionViewModel, INonProcess nonProcess)
            : base(view, restrictionViewModel, nonProcess)
        {
            var version = Environment.OSVersion;
            IsWindows10 = 10 <= version.Version.Major;
        }

        #region property

        public bool IsWindows10 { get; }

        #endregion
    }
}
