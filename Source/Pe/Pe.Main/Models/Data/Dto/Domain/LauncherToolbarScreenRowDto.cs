using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;

namespace ContentTypeTextNet.Pe.Main.Models.Data.Dto.Domain
{
    public class LauncherToolbarScreenRowDto : CommonDtoBase
    {
        #region property

        public Guid LauncherToolbarId { get; set; }

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
