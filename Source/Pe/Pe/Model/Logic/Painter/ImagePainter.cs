using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Logic.Theme
{
    public enum TargetIcon
    {
        LauncherMain,
    }

    public interface IIconImagePainter
    {
        #region function

        IconImageLoaderBase GetIconImageLoader(TargetIcon targetIcon, IconScale iconScale);
        IconImageLoaderPack GetIconImageLoaderPack(TargetIcon targetIcon);

        #endregion
    }

    public abstract class ImagePainterBase : PainterBase, IIconImagePainter
    {
        public ImagePainterBase(IDispatcherWapper dispatcherWapper, ILogger logger)
            : base(dispatcherWapper, logger)
        { }
        public ImagePainterBase(IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(dispatcherWapper, loggerFactory)
        { }

        #region IIconImagePainter

        public abstract IconImageLoaderBase GetIconImageLoader(TargetIcon targetIcon, IconScale iconScale);
        public IconImageLoaderPack GetIconImageLoaderPack(TargetIcon targetIcon)
        {
            var loaders = EnumUtility.GetMembers<IconScale>()
                .Select(i => GetIconImageLoader(targetIcon, i))
            ;
            return new IconImageLoaderPack(loaders);
        }

        #endregion
    }

    public class ImagePainter : ImagePainterBase
    {
        public ImagePainter(IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(dispatcherWapper, loggerFactory)
        { }

        #region ImagePainterBase

        public override IconImageLoaderBase GetIconImageLoader(TargetIcon targetIcon, IconScale iconScale)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
