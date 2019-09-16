using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity
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
