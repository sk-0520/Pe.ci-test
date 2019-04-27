using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Library.ViewModel;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Element.Font;

namespace ContentTypeTextNet.Pe.Main.ViewModel.Font
{
    public class FontViewModel : SingleModelViewModelBase<FontElement>
    {
        public FontViewModel(FontElement model, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            PropertyChangedHooker = new PropertyChangedHooker(dispatcherWapper, Logger.Factory);
            PropertyChangedHooker.AddHook(nameof(Model.FontFamily), nameof(FontFamily));
            PropertyChangedHooker.AddHook(nameof(Model.FontSize), nameof(FontSize));
            PropertyChangedHooker.AddHook(nameof(Model.FontStyle), nameof(FontStyle));
            PropertyChangedHooker.AddHook(nameof(Model.FontWeight), nameof(FontWeight));
        }

        #region property


        PropertyChangedHooker PropertyChangedHooker { get; }

        public FontFamily FontFamily
        {
            get => Model.FontFamily;
            set => Model.ChangeFontFamily(value);
        }

        public double FontSize => Model.FontSize;
        public FontStyle FontStyle => Model.FontStyle;
        public FontWeight FontWeight => Model.FontWeight;


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
            PropertyChangedHooker.Execute(e, RaisePropertyChanged);
        }
    }
}
