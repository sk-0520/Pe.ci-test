using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.IconViewer
{
    public class IconViewerViewModel : SingleModelViewModelBase<IconImageLoaderBase>
    {
        #region variable

        ImageSource? _imageSource = null;
        bool _useCache = false;

        #endregion

        public IconViewerViewModel(IconImageLoaderBase model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            RunningStatus = new RunningStatusViewModel(Model.RunningStatus, LoggerFactory);

            PropertyChangedHooker = new PropertyChangedHooker(dispatcherWrapper, LoggerFactory);
            PropertyChangedHooker.AddHook(nameof(RunningStatus), nameof(RunningStatus), nameof(ImageSource));
        }

        #region property

        PropertyChangedHooker PropertyChangedHooker { get; }

        public RunningStatusViewModel RunningStatus { get; }

        public ImageSource? ImageSource
        {
            get
            {
                if(this._imageSource != null) {
                    return this._imageSource;
                }

                return this._imageSource;
            }
        }

        public IconBox IconBox => Model.IconBox;

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

        public async Task LoadAsync(CancellationToken cancellationToken)
        {
            this._imageSource = await Model.LoadAsync(UseCache, cancellationToken);
            RaisePropertyChanged(nameof(ImageSource));
        }

        #endregion

        #region SingleModelViewModelBase

        protected override void AttachModelEventsImpl()
        {
            base.AttachModelEventsImpl();

            Model.PropertyChanged += Model_PropertyChanged;
        }

        protected override void DetachModelEventsImpl()
        {
            base.DetachModelEventsImpl();

            Model.PropertyChanged -= Model_PropertyChanged;
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                this._imageSource = null;
                if(disposing) {
                    RunningStatus.Dispose();
                    PropertyChangedHooker.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PropertyChangedHooker.Execute(e, RaisePropertyChanged);
        }

    }
}
