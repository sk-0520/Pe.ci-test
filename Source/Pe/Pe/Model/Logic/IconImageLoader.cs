using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Pe.Library.Shared.Embedded.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Launcher;

namespace ContentTypeTextNet.Pe.Main.Model.Logic
{
    public enum IconImageLoadState
    {
        None,
        Loading,
        Loaded,
        Error,
    }

    public abstract class IconImageLoaderBase : BindModelBase
    {
        #region variable

        IconImageLoadState _iconImageLoadState;

        #endregion

        public IconImageLoaderBase(IconScale iconScale, ILogger logger) : base(logger)
        {
            IconScale = iconScale;
        }

        public IconImageLoaderBase(IconScale iconScale, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            IconScale = iconScale;
        }

        #region property

        public IconScale IconScale { get; }

        public IconImageLoadState IconImageLoadState
        {
            get => this._iconImageLoadState;
            set => SetProperty(ref this._iconImageLoadState, value);
        }

        #endregion

        #region function

        protected BitmapSource GetIconImage(IconData iconData)
        {
            var path = TextUtility.SafeTrim(iconData.Path);
            var expandedPath = Environment.ExpandEnvironmentVariables(path);
            if(!File.Exists(expandedPath)) {
                return null;
            }

            var iconLoader = new IconLoader(Logger.Factory);
            var iconImage = iconLoader.Load(expandedPath, IconScale, iconData.Index);

            return iconImage;
        }

        protected abstract Task<BitmapSource> LoadImplAsync();

        public async Task<BitmapSource> LoadAsync()
        {
            IconImageLoadState = IconImageLoadState.Loading;
            try {
                var iconImage = await LoadImplAsync();
                IconImageLoadState = IconImageLoadState.Error;
                return iconImage;
            } catch(Exception ex) {
                Logger.Error(ex);
                IconImageLoadState = IconImageLoadState.Error;
                throw;
            }
        }

        #endregion
    }
}
