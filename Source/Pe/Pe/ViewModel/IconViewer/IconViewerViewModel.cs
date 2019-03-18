using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Logic;

namespace ContentTypeTextNet.Pe.Main.ViewModel.IconViewer
{
    public class IconViewerViewModel : SingleModelViewModelBase<IconImageLoaderBase>
    {
        #region variable

        ImageSource _imageSource = null;

        #endregion

        public IconViewerViewModel(IconImageLoaderBase model, ILogger logger)
            : base(model, logger)
        {
            RunningStatus = new RunningStatusViewModel(Model.RunningStatus, Logger.Factory);
        }

        public IconViewerViewModel(IconImageLoaderBase model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            RunningStatus = new RunningStatusViewModel(Model.RunningStatus, Logger.Factory);
        }

        #region property

        IReadOnlyDictionary<string, IReadOnlyList<string>> PropertyNames { get; } = new Dictionary<string, IReadOnlyList<string>>() {
            [nameof(RunningStatus)] = new[] { nameof(RunningStatus), nameof(ImageSource) },
        };

        public RunningStatusViewModel RunningStatus { get; }

        public ImageSource ImageSource
        {
            get
            {
                if(this._imageSource != null) {
                    return this._imageSource;
                }

                return this._imageSource;
            }
        }

        public IconScale IconScale => Model.IconScale;

        #endregion

        #region command
        #endregion

        #region function

        public async Task LoadAsync(CancellationToken cancellationToken)
        {
            this._imageSource = await Model.LoadAsync(cancellationToken);
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

        #endregion

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(PropertyNames.TryGetValue(e.PropertyName, out var propertyNames)) {
                foreach(var propertyName in propertyNames) {
                    RaisePropertyChanged(propertyName);
                }
            }
        }

    }
}
