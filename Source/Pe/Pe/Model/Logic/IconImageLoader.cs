using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Bridge.Model;
using ContentTypeTextNet.Pe.Core.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Model.Logic
{
    public abstract class IconImageLoaderBase : BindModelBase
    {
        public IconImageLoaderBase(IconScale iconScale, IDispatcherWapper dispatcherWapper, ILogger logger)
            : base(logger)
        {
            IconScale = iconScale;
            DispatcherWapper = dispatcherWapper;
            RunningStatusImpl = new RunningStatus(Lf.Create());
        }

        public IconImageLoaderBase(IconScale iconScale, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            IconScale = iconScale;
            DispatcherWapper = dispatcherWapper;
            RunningStatusImpl = new RunningStatus(Lf.Create());
        }

        #region property

        public IconScale IconScale { get; }
        protected IDispatcherWapper DispatcherWapper { get; }

        RunningStatus RunningStatusImpl { get; }
        public IRunningStatus RunningStatus => RunningStatusImpl;

        #endregion

        #region function

        protected Task<BitmapSource?> GetIconImageAsync(IconData iconData, CancellationToken cancellationToken)
        {
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
            var path = TextUtility.SafeTrim(iconData.Path);
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
            var expandedPath = Environment.ExpandEnvironmentVariables(path);

            return Task.Run(() => {
                if(!FileUtility.Exists(expandedPath)) {
                    return null;
                }

                var iconLoader = new IconLoader(Lf.Create());
                BitmapSource? iconImage = null;
                DispatcherWapper.Invoke(() => {
                    iconImage = iconLoader.Load(expandedPath, IconScale, iconData.Index);
                    FreezableUtility.SafeFreeze(iconImage);
                });
                return iconImage;
            });
        }

        protected abstract Task<BitmapSource> LoadImplAsync(CancellationToken cancellationToken);

        public async Task<BitmapSource> LoadAsync(CancellationToken cancellationToken)
        {
            RunningStatusImpl.State = RunningState.Running;
            try {
                var iconImage = await LoadImplAsync(cancellationToken);
                RunningStatusImpl.State = RunningState.End;
                return iconImage;
            } catch(OperationCanceledException ex) {
                Logger.LogWarning(ex, ex.Message);
                RunningStatusImpl.State = RunningState.Cancel;
                throw;
            } catch(Exception ex) {
                Logger.LogError(ex, ex.Message);
                RunningStatusImpl.State = RunningState.Error;
                throw;
            }
        }

        #endregion
    }

    public class IconImageLoaderPack: IIconPack<IconImageLoaderBase>
    {
        public IconImageLoaderPack(IEnumerable<IconImageLoaderBase> iconImageLoaders)
        {
            var map = iconImageLoaders.ToDictionary(i => i.IconScale, i => i);
            Small = map[IconScale.Small];
            Normal = map[IconScale.Normal];
            Big = map[IconScale.Big];
            Large = map[IconScale.Large];
        }

        #region property

        public IconImageLoaderBase Small { get; }
        public IconImageLoaderBase Normal { get; }
        public IconImageLoaderBase Big { get; }
        public IconImageLoaderBase Large { get; }

        #endregion
    }
}
