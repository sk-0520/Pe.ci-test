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
using ContentTypeTextNet.Pe.Main.Model.Logic;

namespace ContentTypeTextNet.Pe.Main.ViewModel.Font
{
    public class FontViewModel : SingleModelViewModelBase<FontElement>
    {
        public FontViewModel(FontElement model, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            PropertyChangedHooker = new PropertyChangedHooker(dispatcherWapper, Logger.Factory);
            PropertyChangedHooker.AddHook(nameof(Model.FamilyName), nameof(FontFamily));
            PropertyChangedHooker.AddHook(nameof(Model.Size), new[] { nameof(Size), nameof(FontSize) });
            PropertyChangedHooker.AddHook(nameof(Model.IsItalic), new[] { nameof(IsItalic), nameof(FontStyle) });
            PropertyChangedHooker.AddHook(nameof(Model.IsBold), new[] { nameof(IsBold), nameof(FontWeight) });
        }

        #region property


        PropertyChangedHooker PropertyChangedHooker { get; }

        public FontFamily FontFamily
        {
            get
            {
                var fc = new FontConverter(Logger.Factory);
                return fc.MakeFontFamily(Model.FamilyName, SystemFonts.MessageFontFamily);
            }
            set
            {
                var fc = new FontConverter(Logger.Factory);
                Model.ChangeFamilyName(fc.GetOriginalFontFamilyName(value));
            }
        }

        public double FontSize => Model.Size;
        public FontStyle FontStyle
        {
            get
            {
                var fc = new FontConverter(Logger.Factory);
                return fc.ToStyle(Model.IsItalic);
            }
        }
        public FontWeight FontWeight
        {
            get
            {
                var fc = new FontConverter(Logger.Factory);
                return fc.ToWeight(Model.IsBold);
            }
        }

        public bool IsBold
        {
            get => Model.IsBold;
            set => Model.ChangeBold(value);
        }
        public bool IsItalic
        {
            get => Model.IsItalic;
            set => Model.ChangeItalic(value);
        }

        public double Size
        {
            get => Model.Size;
            set => Model.ChangeSize(value);
        }

        public virtual double MinimumSize => 6;
        public virtual double MaximumSize => 72;
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
