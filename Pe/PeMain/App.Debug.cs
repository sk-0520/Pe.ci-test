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
namespace ContentTypeTextNet.Pe.PeMain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using ContentTypeTextNet.Library.SharedLibrary.Define;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
    using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

    partial class App
    {
#if DEBUG
        /// <summary>
        /// デバッグ時にちょっとしたコード検証
        /// </summary>
        void DebugProcess()
        {
            //icon();
            //font();
            //box();
        }

        void icon()
        {
            //var path = Environment.ExpandEnvironmentVariables(@"%PROGRAMFILES%\Waterfox\waterfox.exe");
            var pathList = new[] {
                @"C:\Program Files\Waterfox\waterfox.exe",
                @"C:\Windows",
                @"C:\",
                @"C:\Program Files (x86)\IrfanView\i_view32.exe"
            };
            var saveDir = @"Z:\";

            foreach(var path in pathList.Select(Environment.ExpandEnvironmentVariables)) {
                foreach(var scale in new[] { IconScale.Small, IconScale.Normal, IconScale.Big, IconScale.Large }) {
                    var image = IconUtility.Load(path, scale, 0);
                    var name = string.Format("{0}-{1}.png", Path.GetFileNameWithoutExtension(path), scale);
                    var savePath = Path.Combine(saveDir, name);
                    using(var fileStream = new FileStream(savePath, FileMode.Create)) {
                        var encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create((BitmapSource)image));
                        encoder.Save(fileStream);
                    }
                }
            }
        }

        void font()
        {
            var names = new[] { null, "", "Arial", "ＭＳ ゴシック" };
            foreach(var name in names) {
                var ff = FontUtility.MakeFontFamily(name, SystemFonts.MessageFontFamily);
                Debug.WriteLine(ff);
            }
        }

        void box()
        {
            var canvas = ImageUtility.CreateBox(Colors.Red, Colors.Yellow, new Size(16, 16));
            var box16 = ImageUtility.MakeBitmapBitmapSourceDefualtDpi(canvas);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(box16));

            using(var stm = File.Create(@"Z:\image.png")) {
                encoder.Save(stm);
            }
        }


#endif
    }
}
