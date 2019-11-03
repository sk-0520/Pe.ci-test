using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;

namespace ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity
{
    public class ScreensRowDto : CommonDtoBase
    {
        #region property

        public string? ScreenName { get; set; }
        [PixelKind(Px.Device)]
        public long ScreenX { get; set; }
        [PixelKind(Px.Device)]
        public long ScreenY { get; set; }
        [PixelKind(Px.Device)]
        public long ScreenWidth { get; set; }
        [PixelKind(Px.Device)]
        public long ScreenHeight { get; set; }

        #endregion
    }
}
