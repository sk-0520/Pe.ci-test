using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Logic;

namespace ContentTypeTextNet.Pe.Main.ViewModel.IconViewer
{
    public class IconViewerViewModel : SingleModelViewModelBase<IconImageLoaderBase>
    {
        public IconViewerViewModel(IconImageLoaderBase model, ILogger logger)
            : base(model, logger)
        { }

        public IconViewerViewModel(IconImageLoaderBase model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property

        ISet<string> PropertyNames { get; } = new HashSet<string>(new[] {
            nameof(IconImageLoadState),
        });

        public IconImageLoadState IconImageLoadState => Model.IconImageLoadState;


        #endregion

        #region command
        #endregion

        #region function
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
            if(PropertyNames.Contains(e.PropertyName)) {
                RaisePropertyChanged(e.PropertyName);
            }
        }

    }
}
