using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Logic;

namespace ContentTypeTextNet.Pe.Main.Model.Applications
{
    public class ApplicationIconImageLoader : IconImageLoaderBase
    {
        public ApplicationIconImageLoader(IconScale iconScale, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(iconScale, dispatcherWapper, loggerFactory)
        { }

        protected override Task<BitmapSource> LoadImplAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public interface IApplicationIconImageLoaders
    {
        #region property

        IDictionary<IconScale, IconImageLoaderBase> IconImageLoaders { get; }

        #endregion
    }

    public class ApplicationIconImageLoaders : IApplicationIconImageLoaders
    {
        public ApplicationIconImageLoaders(IDispatcherWapper dispatcherWapper, ILogger logger)
        {
            Logger = logger;

            IconImageLoaders = EnumUtility.GetMembers<IconScale>()
                .Select(i => new ApplicationIconImageLoader(i, dispatcherWapper, Logger.Factory))
                .ToDictionary(k => k.IconScale, v => (IconImageLoaderBase)v)
            ;
        }

        #region property

        ILogger Logger { get; }

        #endregion

        #region property

        public IDictionary<IconScale, IconImageLoaderBase> IconImageLoaders { get; }

        #endregion
    }
}
