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
    public enum IconLoadState
    {
        None,
        Loading,
        Loaded,
        Error,
    }

    public abstract class IconImageLoaderBase : BindModelBase
    {
        #region variable

        IconLoadState _iconLoadState;

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

        protected IconScale IconScale { get; }

        public IconLoadState IconLoadState
        {
            get => this._iconLoadState;
            set => SetProperty(ref this._iconLoadState, value);
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
            IconLoadState = IconLoadState.Loading;
            try {
                var iconImage = await LoadImplAsync();
                IconLoadState = IconLoadState.Error;
                return iconImage;
            } catch(Exception ex) {
                Logger.Error(ex);
                IconLoadState = IconLoadState.Error;
                throw;
            }
        }

        #endregion
    }
}
