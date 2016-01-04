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
    using Logic;
    using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
    using System.Security.Cryptography;
    using System.Runtime.Serialization;
    using ContentTypeTextNet.Library.SharedLibrary.Model;
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
            //browser();
            //toolbar();
            //json();
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

        void browser()
        {
            SystemEnvironmentUtility.SetUsingBrowserVersionForExecutingAssembly(8000);
        }

        void toolbar()
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
                var image = ImageUtility.MakeBitmapBitmapSourceDefualtDpi(icon);

                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                using(var s = File.OpenWrite(@"Z:\" + dt.ToString() + ".png")) {
                    encoder.Save(s);
                }
            }
        }

        [DataContract]
        public class json_debug: ModelBase
        {
            [DataMember]
            public DateTime DateTime1 { get; set; }
            [DataMember]
            public byte[] ByteArray { get; set; }
            [DataMember]
            public string Text { get; set; }
            [DataMember]
            public DateTime DateTime2 { get; set; }
        }

        void json()
        {
            var hash = new SHA1CryptoServiceProvider();
            var jd = new json_debug() {
                DateTime1 = DateTime.MinValue,
                DateTime2 = DateTime.MaxValue,
                Text = "JSON TEST",
                ByteArray = hash.ComputeHash(new byte[] { 0, 1, 2, 3, 4 }),
            };
            var stream = new MemoryStream();
            SerializeUtility.SaveJsonDataToStream(stream, jd);
            var saved = Encoding.UTF8.GetString(stream.GetBuffer());
            stream.Seek(0, SeekOrigin.Begin);
            var jd2 = SerializeUtility.LoadJsonDataFromStream<json_debug>(stream);
        }

#endif
    }
}
