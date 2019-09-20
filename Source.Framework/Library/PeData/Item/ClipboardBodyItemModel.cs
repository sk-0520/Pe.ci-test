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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Define;

namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
    /// <summary>
    /// クリップデータのボディ部。
    /// </summary>
    [DataContract, Serializable]
    public class ClipboardBodyItemModel: IndexBodyItemModelBase
    {
        public ClipboardBodyItemModel()
            : base()
        { }

        #region property

        /// <summary>
        /// テキストデータ。
        /// </summary>
        [DataMember, IsDeepClone]
        public string Text { get; set; }
        /// <summary>
        /// RTFデータ。
        /// </summary>
        [DataMember, IsDeepClone]
        public string Rtf { get; set; }
        /// <summary>
        /// HTMLデータ。
        /// </summary>
        [DataMember, IsDeepClone]
        public string Html { get; set; }
        /// <summary>
        /// 画像データ。
        /// <para>コード上ではこちらを使用する。</para>
        /// </summary>
        [IgnoreDataMember, XmlIgnore]
        public BitmapSource Image { get; set; }
        /// <summary>
        /// 画像データの内部実装。
        /// </summary>
        [DataMember(Name = "Image")]
        public byte[] ImageCore
        {
            get
            {
                if(Image != null) {
                    var encoder = new PngBitmapEncoder();
                    //var encoder = new BmpBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create((BitmapSource)Image.Clone()));
                    using(var stream = new MemoryStream()) {
                        encoder.Save(stream);
                        return stream.ToArray();
                    }
                } else {
                    return null;
                }
            }
            set
            {
                if(value == null) {
                    Image = null;
                } else {
                    if(Image != null) {
                        return;
                    }

                    using(var stream = new MemoryStream(value)) {
                        var bitmapImage = new BitmapImage();
                        try {
                            using(Initializer.BeginInitialize(bitmapImage)) {
                                //bitmapImage.BeginInit();
                                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                                bitmapImage.CreateOptions = BitmapCreateOptions.None;
                                //bitmapImage.StreamSource = new MemoryStream(value);
                                bitmapImage.StreamSource = stream;
                            }
                        } finally {
                            //bitmapImage.EndInit();
                            //bitmapImage.Freeze();
                            FreezableUtility.SafeFreeze(bitmapImage);
                        }
                        Image = bitmapImage;
                    }
                }
            }
        }
        /// <summary>
        /// ファイルデータ。
        /// </summary>
        [DataMember, IsDeepClone]
        public CollectionModel<string> Files { get; set; } = new CollectionModel<string>();

        #endregion

        #region IndexBodyItemModelBase

        /// <summary>
        /// インデックス種別。
        /// </summary>
        public override IndexKind IndexKind { get { return IndexKind.Clipboard; } }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                //var bitmapImage = Image as BitmapImage;
                //if(bitmapImage != null && Application.Current != null) {
                //	Application.Current.Dispatcher.Invoke(new Action(() => {
                //		if(bitmapImage.StreamSource != null) {
                //			bitmapImage.StreamSource.Dispose();
                //			bitmapImage.StreamSource = null;
                //		}
                //	}));
                //}
                Image = null;
                ImageCore = null;
            }

            base.Dispose(disposing);
        }

        //public override void DeepCloneTo(IDeepClone target)
        //{
        //    base.DeepCloneTo(target);

        //    var obj = (ClipboardBodyItemModel)target;

        //    obj.Text = Text;
        //    obj.Rtf = Rtf;
        //    obj.Html = Html;
        //    if(ImageCore != null) {
        //        obj.ImageCore = new byte[ImageCore.Length];
        //        ImageCore.CopyTo(obj.ImageCore, 0);
        //    }
        //    obj.Files.InitializeRange(Files);
        //}

        public override IDeepClone DeepClone()
        {
            //var result = new ClipboardBodyItemModel();

            //DeepCloneTo(result);

            //return result;

            var result = (ClipboardBodyItemModel)DeepCloneUtility.Copy(this);
            if(ImageCore != null) {
                result.ImageCore = new byte[ImageCore.Length];
                ImageCore.CopyTo(result.ImageCore, 0);
            }
            return result;
        }

        #endregion
    }
}
