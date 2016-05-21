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
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using System.Security.Cryptography;
using System.Runtime.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Item;

namespace ContentTypeTextNet.Pe.PeMain
{
    partial class App
    {
#if DEBUG
        /// <summary>
        /// デバッグ時にちょっとしたコード検証
        /// </summary>
        void DebugProcess()
        {
            debugIcon();
            debugFont();
            debugBox();
            debugBrowser();
            debugToolbar();
            debugJson();
        }

        void debugIcon()
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

        void debugFont()
        {
            var names = new[] { null, "", "Arial", "ＭＳ ゴシック" };
            foreach(var name in names) {
                var ff = FontUtility.MakeFontFamily(name, SystemFonts.MessageFontFamily);
                Debug.WriteLine(ff);
            }
        }

        void debugBox()
        {
            var canvas = ImageUtility.CreateBox(Colors.Red, Colors.Yellow, new Size(16, 16));
            var box16 = ImageUtility.MakeBitmapSourceDefualtDpi(canvas);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(box16));

            using(var stm = File.Create(@"Z:\image.png")) {
                encoder.Save(stm);
            }
        }

        void debugBrowser()
        {
            SystemEnvironmentUtility.SetUsingBrowserVersionForExecutingAssembly(8000);
        }

        void debugToolbar()
        {
            Size imageSize = new Size(IconScale.Small.ToWidth() * 2, IconScale.Small.ToHeight());
            var dts = new[] {
                DockType.None,
                DockType.Top,
                DockType.Bottom,
                DockType.Left,
                DockType.Right,
            };
            foreach(var dt in dts) {
                var icon = LauncherToolbarUtility.MakeDockIcon(dt, imageSize);
                var image = ImageUtility.MakeBitmapSourceDefualtDpi(icon);

                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                using(var s = File.OpenWrite($@"Z:\{dt}.png")) {
                    encoder.Save(s);
                }
            }
        }

        void debugJson()
        {
            var src = new HashItemModel() {
                Code = new byte[1024],
                Type = Library.PeData.Define.HashType.SHA1,
            };
            var rnd = new Random();
            foreach(var i in Enumerable.Range(0, src.Code.Length)) {
                src.Code[i] = (byte)rnd.Next(byte.MaxValue);
            }

            var stream = new MemoryStream();
            SerializeUtility.SaveJsonDataToStream(stream, src);
            var saved = Encoding.UTF8.GetString(stream.GetBuffer());
            stream.Seek(0, SeekOrigin.Begin);
            var dst = SerializeUtility.LoadJsonDataFromStream<HashItemModel>(stream);
        }

#endif
    }
}
