using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
{
    /// <summary>
    /// 取り合えず画像が欲しい。
    /// <para>いろんなフォーマットに手を付けられる場合にこれが実装される、はず。</para>
    /// </summary>
    public interface IMakeBitmapSource
    {
        /// <summary>
        /// 画像の生成。
        /// </summary>
        /// <returns></returns>
        BitmapSource MakeBitmapSource();
    }
}
