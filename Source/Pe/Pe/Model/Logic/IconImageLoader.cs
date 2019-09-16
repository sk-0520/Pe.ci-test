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
using ContentTypeTextNet.Pe.Bridge.Model.Data;
using ContentTypeTextNet.Pe.Core.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Model.Logic
{
    public abstract class IconImageLoaderBase : BindModelBase
    {
        public IconImageLoaderBase(IconSize iconSIze, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            IconSize = iconSIze;
            DispatcherWapper = dispatcherWapper;
            RunningStatusImpl = new RunningStatus(LoggerFactory);
        }

        #region property

        public IconSize IconSize { get; }
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

                var iconLoader = new IconLoader(Logger);
                BitmapSource? iconImage = null;
                DispatcherWapper.Invoke(() => {
                    iconImage = iconLoader.Load(expandedPath, IconSize, iconData.Index);
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
                    FreezableUtility.SafeFreeze(iconImage);
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
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
            var map = iconImageLoaders.ToDictionary(i => (IconSize.Kind)i.IconSize.Width, i => i);
            Small = map[IconSize.Kind.Small];
            Normal = map[IconSize.Kind.Normal];
            Big = map[IconSize.Kind.Big];
            Large = map[IconSize.Kind.Large];
        }

        #region property

        public IconImageLoaderBase Small { get; }
        public IconImageLoaderBase Normal { get; }
        public IconImageLoaderBase Big { get; }
        public IconImageLoaderBase Large { get; }

        #endregion
    }
}
