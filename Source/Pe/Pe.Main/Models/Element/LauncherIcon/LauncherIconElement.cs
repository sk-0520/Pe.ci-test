using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.LauncherIcon
{
    public class LauncherIconElement : ElementBase, ILauncherItemId
    {
        public LauncherIconElement(Guid launcherItemId, IconImageLoaderBase iconImageLoaderPack, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            LauncherItemId = launcherItemId;
            IconImageLoaderPack = iconImageLoaderPack;
        }

        #region property

        public IconImageLoaderBase IconImageLoaderPack { get; }
        //IDatabaseStatementLoader StatementLoader { get; }

        //public IReadOnlyDictionary<IconScale, LauncherIconLoader> IconImageLoaders { get; }

        #endregion

        #region function

        public object GetIcon(IconScale iconScale)
        {
            //TODO: このメソッドに方向転換する!!
            return default!;
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        { }

        #endregion

        #region ILauncherItemId

        public Guid LauncherItemId { get; }

        #endregion

    }
}
