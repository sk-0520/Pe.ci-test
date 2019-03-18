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
using ContentTypeTextNet.Pe.Library.Shared.Embedded.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Launcher;

namespace ContentTypeTextNet.Pe.Main.Model.Logic
{
    public abstract class IconImageLoaderBase : BindModelBase
    {
        public IconImageLoaderBase(IconScale iconScale, ILogger logger)
            : base(logger)
        {
            IconScale = iconScale;
            RunningStatusImpl = new RunningStatus(Logger.Factory);
        }

        public IconImageLoaderBase(IconScale iconScale, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            IconScale = iconScale;
            RunningStatusImpl = new RunningStatus(Logger.Factory);
        }

        #region property

        public IconScale IconScale { get; }

        RunningStatus RunningStatusImpl { get; }
        public IRunningStatus RunningStatus => RunningStatusImpl;

        #endregion

        #region function

        protected Task<BitmapSource> GetIconImageAsync(IconData iconData, CancellationToken cancellationToken)
        {
            var path = TextUtility.SafeTrim(iconData.Path);
            var expandedPath = Environment.ExpandEnvironmentVariables(path);

            return Task.Run(() => {
                if(!File.Exists(expandedPath)) {
                    return null;
                }

                var iconLoader = new IconLoader(Logger.Factory);
                BitmapSource iconImage = null;
                Application.Current.Dispatcher.Invoke(() => {
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
                Logger.Warning(ex);
                RunningStatusImpl.State = RunningState.Cancel;
                throw;
            } catch(Exception ex) {
                Logger.Error(ex);
                RunningStatusImpl.State = RunningState.Error;
                throw;
            }
        }

        #endregion
    }
}
