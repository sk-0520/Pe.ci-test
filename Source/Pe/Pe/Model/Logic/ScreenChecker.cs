using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Logic
{
    public interface IScreenData
    {
        #region property

        string ScreenName { get; }
        [PixelKind(Px.Device)]
        long ScreenX { get; }
        [PixelKind(Px.Device)]
        long ScreenY { get; }
        [PixelKind(Px.Device)]
        long ScreenWidth { get; }
        [PixelKind(Px.Device)]
        long ScreenHeight { get; }

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
            if(data.ScreenX == deviceBounds.X && data.ScreenY == deviceBounds.Y && data.ScreenWidth == deviceBounds.Width && data.ScreenHeight == deviceBounds.Height) {
                return true;
            }

            return false;
        }

        #endregion
    }
}
