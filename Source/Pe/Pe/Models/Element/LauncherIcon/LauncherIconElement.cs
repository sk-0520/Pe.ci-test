using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.LauncherIcon
{
    public class LauncherIconElement : ElementBase
    {
        public LauncherIconElement(Guid launcherItemId, IconImageLoaderPack iconImageLoaderPack, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            LauncherItemId = launcherItemId;
            IconImageLoaderPack = iconImageLoaderPack;
        }

        #region property

        Guid LauncherItemId { get; }
        public IconImageLoaderPack IconImageLoaderPack { get; }
        //IDatabaseStatementLoader StatementLoader { get; }

        //public IReadOnlyDictionary<IconScale, LauncherIconLoader> IconImageLoaders { get; }

        #endregion

        #region function
        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
        }

        #endregion
    }
}
