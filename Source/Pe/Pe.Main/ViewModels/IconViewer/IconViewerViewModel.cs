using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.IconViewer
{
    public sealed class ImageViewModel: ViewModelBase
    {
        public ImageViewModel(ImageSource imageSource, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            ImageSource = imageSource;
        }

        #region property

        public ImageSource ImageSource { get; }

        #endregion
    }

    public sealed class IconObjectViewModel: ViewModelBase
    {
        public IconObjectViewModel(object icon, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Icon = icon;
        }

        #region property

        public object Icon { get; }

        #endregion
    }

    public class IconViewerViewModel: ViewModelBase
    {
        #region define

        private enum IconImageKind
        {
            IconImageLoader,
            LauncherItemExtension,
        }

        #endregion

        #region variable

        private ViewModelBase? _icon = null;
        private bool _useCache = false;

        #endregion

        public IconViewerViewModel(IconImageLoaderBase model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            IconKind = IconImageKind.IconImageLoader;

            IconImageLoader = model;
            DispatcherWrapper = dispatcherWrapper;
            RunningStatus = new RunningStatusViewModel(IconImageLoader.RunningStatus, LoggerFactory);

            IconImageLoader.PropertyChanged += Model_PropertyChanged;

            PropertyChangedHooker = new PropertyChangedHooker(DispatcherWrapper, LoggerFactory);
            PropertyChangedHooker.AddHook(nameof(RunningStatus), nameof(RunningStatus), nameof(ImageSource));
        }

        public IconViewerViewModel(LauncherItemId launcherItemId, ILauncherItemExtension launcherItemExtension, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            IconKind = IconImageKind.LauncherItemExtension;

            LauncherItemId = launcherItemId;
            LauncherItemExtension = launcherItemExtension;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        private IconImageKind IconKind { get; }
        private IDispatcherWrapper DispatcherWrapper { get; }

        #region IconImageLoader

        /// <summary>
        /// アイコン読み込み処理。
        /// </summary>
        /// <remarks>
        /// <para><see cref="IDisposable.Dispose"/>時にVM側で<see cref="IDisposable.Dispose"/>される。</para>
        /// </remarks>
        IconImageLoaderBase? IconImageLoader { get; }
        PropertyChangedHooker? PropertyChangedHooker { get; }
        public RunningStatusViewModel? RunningStatus { get; }

        #endregion

        #region LauncherItemExtension

        LauncherItemId LauncherItemId { get; }
        ILauncherItemExtension? LauncherItemExtension { get; }

        #endregion

        public ViewModelBase? Icon => this._icon;

        /// <summary>
        /// すでに読み込んだアイコンはキャッシュを使用する。
        /// </summary>
        public bool UseCache
        {
            get => this._useCache;
            set => SetProperty(ref this._useCache, value);
        }

        #endregion

        #region command
        #endregion

        #region function

        public async Task LoadAsync(IconScale iconScale, LauncherItemIconMode iconMode, CancellationToken cancellationToken)
        {
            switch(IconKind) {
                case IconImageKind.IconImageLoader:
                    Debug.Assert(IconImageLoader != null);

                    var image = await IconImageLoader.LoadAsync(UseCache, iconScale, cancellationToken);
                    if(image != null) {
                        this._icon = new ImageViewModel(image, LoggerFactory);
                        RaisePropertyChanged(nameof(Icon));
                    }
                    break;

                case IconImageKind.LauncherItemExtension:
                    Debug.Assert(LauncherItemExtension != null);

                    var icon = LauncherItemExtension.GetIcon(iconMode, iconScale);
                    if(icon != null) {
                        this._icon = new IconObjectViewModel(icon, LoggerFactory);
                        RaisePropertyChanged(nameof(Icon));
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion

        #region SingleModelViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                this._icon?.Dispose();
                this._icon = null;

                switch(IconKind) {
                    case IconImageKind.IconImageLoader:
                        Debug.Assert(IconImageLoader != null);
                        IconImageLoader.PropertyChanged -= Model_PropertyChanged;
                        IconImageLoader.Dispose();
                        break;

                    case IconImageKind.LauncherItemExtension:
                        Debug.Assert(LauncherItemExtension != null);
                        break;

                    default:
                        throw new NotImplementedException();
                }


                if(disposing) {
                    RunningStatus?.Dispose();
                    PropertyChangedHooker?.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        private void Model_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PropertyChangedHooker!.Execute(e, RaisePropertyChanged);
        }
    }
}
