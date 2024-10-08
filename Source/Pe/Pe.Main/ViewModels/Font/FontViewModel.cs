using System;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.Font;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Font
{
    public class FontViewModel: SingleModelViewModelBase<FontElement>, IFlushable
    {
        public FontViewModel(FontElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            if(!Model.IsInitialized) {
                throw new ArgumentException(nameof(Model) + "." + nameof(Model.InitializeAsync), nameof(model));
            }

            PropertyChangedObserver = new PropertyChangedObserver(dispatcherWrapper, LoggerFactory);
            PropertyChangedObserver.AddObserver(nameof(Model.FamilyName), nameof(FontFamily));
            PropertyChangedObserver.AddObserver(nameof(Model.Size), new[] { nameof(Size), nameof(FontSize) });
            PropertyChangedObserver.AddObserver(nameof(Model.IsItalic), new[] { nameof(IsItalic), nameof(FontStyle) });
            PropertyChangedObserver.AddObserver(nameof(Model.IsBold), new[] { nameof(IsBold), nameof(FontWeight) });
        }

        #region property

        private PropertyChangedObserver PropertyChangedObserver { get; }

        public FontFamily FontFamily
        {
            get
            {
                var fc = new FontConverter(LoggerFactory);
                return fc.MakeFontFamily(Model.FamilyName, SystemFonts.MessageFontFamily);
            }
            set
            {
                var fc = new FontConverter(LoggerFactory);
                Model.FamilyName = fc.GetOriginalFontFamilyName(value);
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
            set => Model.IsBold = value;
        }
        public bool IsItalic
        {
            get => Model.IsItalic;
            set => Model.IsItalic = value;
        }

        public double Size
        {
            get => Model.Size;
            set => Model.Size = value;
        }

        public virtual double MinimumSize => 6;
        public virtual double MaximumSize => 72;

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

        #region IFlushable

        public void Flush()
        {
            Model.SafeFlush();
        }

        #endregion

        private void Model_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PropertyChangedObserver.Execute(e, RaisePropertyChanged);
        }
    }
}
