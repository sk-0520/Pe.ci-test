using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Model;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Logic
{
    public interface IScreenData
    {
        #region property

        string? ScreenName { get; }
        [PixelKind(Px.Device)]
        long X { get; }
        [PixelKind(Px.Device)]
        long Y { get; }
        [PixelKind(Px.Device)]
        long Width { get; }
        [PixelKind(Px.Device)]
        long Height { get; }

        #endregion
    }

    public class ScreenChecker
    {
        #region function

        public bool FindMaybe(Screen screen, IScreenData data)
        {
            if(data.ScreenName == screen.DeviceName) {
                return true;
            }

            var deviceBounds = screen.DeviceBounds;
            // 完全一致パターン: ドライバ更新でも大抵は大丈夫だと思う
            if(data.X == deviceBounds.X && data.Y == deviceBounds.Y && data.Width == deviceBounds.Width && data.Height == deviceBounds.Height) {
                return true;
            }

            return false;
        }

        #endregion
    }
}
