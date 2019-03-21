using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Logic;

namespace ContentTypeTextNet.Pe.Main.Model.Element.LauncherIcon
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
