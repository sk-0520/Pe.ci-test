using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ContentTypeTextNet.Pe.Bridge.Models.Data
{
    public interface IScreen
    {
        #region property
        /// <summary>
        /// 1 ピクセルのデータに関連付けられているメモリのビット数を取得します。
        /// </summary>
        int BitsPerPixel { get;  }
        /// <summary>
        /// ディスプレイの範囲を取得します。
        /// </summary>
        [PixelKind(Px.Device)]
        Rect DeviceBounds { get; }
        /// <summary>
        /// ディスプレイに関連付けられているデバイス名を取得します。
        /// </summary>
        string DeviceName { get; }
        /// <summary>
        /// 特定のディスプレイがプライマリ デバイスかどうかを示す値を取得します。
        /// </summary>
        bool Primary { get; }
        /// <summary>
        /// ディスプレイの作業領域を取得します。 作業領域とは、ディスプレイのデスクトップ領域からタスクバー、ドッキングされたウィンドウ、およびドッキングされたツール バーを除いた部分です。
        /// </summary>
        [PixelKind(Px.Device)]
        Rect DeviceWorkingArea { get;  }

        #endregion
    }
}
