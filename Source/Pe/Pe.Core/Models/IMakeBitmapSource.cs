using System.Windows.Media.Imaging;

namespace ContentTypeTextNet.Pe.Core.Models
{
    /// <summary>
    /// 取り合えず画像が欲しい。
    /// </summary>
    /// <remarks>
    /// <para>いろんなフォーマットに手を付けられる場合にこれが実装される、はず。</para>
    /// </remarks>
    public interface IMakeBitmapSource
    {
        /// <summary>
        /// 画像の生成。
        /// </summary>
        /// <returns></returns>
        BitmapSource MakeBitmapSource();
    }
}
