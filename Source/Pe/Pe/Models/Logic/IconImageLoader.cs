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
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public abstract class IconImageLoaderBase : BindModelBase
    {
        public IconImageLoaderBase(IconBox iconBox, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            IconBox = iconBox;
            DispatcherWapper = dispatcherWapper;
            RunningStatusImpl = new RunningStatus(LoggerFactory);
        }

        #region property

        public IconBox IconBox { get; }
        protected IDispatcherWapper DispatcherWapper { get; }

        RunningStatus RunningStatusImpl { get; }
        public IRunningStatus RunningStatus => RunningStatusImpl;

        public static IReadOnlyCollection<string> ImageFilePath { get; } = new[] { "png", "bmp", "jpeg", "jpg" };

        #endregion

        #region function

        protected Task<BitmapSource?> GetIconImageAsync(IconData iconData, CancellationToken cancellationToken)
        {
            var path = TextUtility.SafeTrim(iconData.Path);
            var expandedPath = Environment.ExpandEnvironmentVariables(path);

            return Task.Run(() => {
                var isFile = File.Exists(expandedPath);
                var isDir = !isFile && Directory.Exists(expandedPath);
                if(!isFile && !isDir) {
                    return null;
                }

                BitmapSource? iconImage = null;

                if(isFile && PathUtility.HasExtensions(expandedPath, ImageFilePath)) {
                    Logger.LogDebug("画像ファイルとして読み込み {0}", expandedPath);
                    using(var stream = new FileStream(expandedPath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                        DispatcherWapper.Invoke(() => {
                            var bitmapImage = new BitmapImage();
                            using(Initializer.BeginInitialize(bitmapImage)) {
                                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                                bitmapImage.CreateOptions = BitmapCreateOptions.None;
                                bitmapImage.StreamSource = stream;
                            }
                            iconImage = FreezableUtility.GetSafeFreeze(bitmapImage);
                        });
                    }
                } else {
                    Logger.LogDebug("アイコンファイルとして読み込み {0}", expandedPath);
                    var iconLoader = new IconLoader(Logger);
                    DispatcherWapper.Invoke(() => {
                        var image = iconLoader.Load(expandedPath, new IconSize(IconBox), iconData.Index);
                        iconImage = FreezableUtility.GetSafeFreeze(image!);
                    });
                }

                return iconImage;
            });
        }

        protected abstract Task<BitmapSource?> LoadImplAsync(CancellationToken cancellationToken);

        public async Task<BitmapSource?> LoadAsync(CancellationToken cancellationToken)
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

    public class IconImageLoaderPack : IIconPack<IconImageLoaderBase>
    {
        public IconImageLoaderPack(IEnumerable<IconImageLoaderBase> iconImageLoaders)
        {
            var map = iconImageLoaders.ToDictionary(i => i.IconBox, i => i);
            Small = map[IconBox.Small];
            Normal = map[IconBox.Normal];
            Big = map[IconBox.Big];
            Large = map[IconBox.Large];
        }

        #region property

        public IconImageLoaderBase Small { get; }
        public IconImageLoaderBase Normal { get; }
        public IconImageLoaderBase Big { get; }
        public IconImageLoaderBase Large { get; }

        #endregion
    }
}
