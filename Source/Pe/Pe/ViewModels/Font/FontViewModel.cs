using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.Font;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Font
{
    public class FontViewModel : SingleModelViewModelBase<FontElement>
    {
        public FontViewModel(FontElement model, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            PropertyChangedHooker = new PropertyChangedHooker(dispatcherWapper, LoggerFactory);
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
                var fc = new FontConverter(LoggerFactory);
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
                return fc.MakeFontFamily(Model.FamilyName, SystemFonts.MessageFontFamily);
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
            }
            set
            {
                var fc = new FontConverter(LoggerFactory);
                Model.ChangeFamilyName(fc.GetOriginalFontFamilyName(value));
            }
        }

        public double FontSize => Model.Size;
        public FontStyle FontStyle
        {
            get
            {
                var fc = new FontConverter(LoggerFactory);
                return fc.ToStyle(Model.IsItalic);
            }
        }
        public FontWeight FontWeight
        {
            get
            {
                var fc = new FontConverter(LoggerFactory);
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
