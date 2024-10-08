using System;
using System.ComponentModel;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Views
{
    public interface IExtendData: INotifyPropertyChanged, IDisposer
    { }

    public abstract class ViewExtendBase<TView, TExtendData>: DisposerBase
        where TView : UIElement
        where TExtendData : IExtendData
    {
        protected ViewExtendBase(TView view, TExtendData extendData, ILoggerFactory loggerFactory)
        {
            View = view;
            ExtendData = extendData;
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());

            ExtendData.Disposing += ExtendData_Disposing;
            ExtendData.PropertyChanged += ExtendData_PropertyChanged;

            PropertyChangedHooker = new PropertyChangedObserver(new DispatcherWrapper(View.Dispatcher), loggerFactory);
            PropertyChangedHooker.AddProperties<TExtendData>();
        }

        #region property

        protected TView View { get; private set; }
        protected TExtendData ExtendData { get; private set; }

        /// <inheritdoc cref="ILoggerFactory"/>
        protected ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        protected ILogger Logger { get; }

        /// <inheritdoc cref="PropertyChangedHooker"/>
        protected PropertyChangedObserver PropertyChangedHooker { get; }

        #endregion

        #region function

        protected abstract void ChangedExtendProperty(string propertyName);

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                View = null!;
                ExtendData.Disposing -= ExtendData_Disposing;
                ExtendData.PropertyChanged -= ExtendData_PropertyChanged;
            }

            base.Dispose(disposing);
        }

        #endregion

        private void ExtendData_Disposing(object? sender, EventArgs e)
        {
            ExtendData.Disposing -= ExtendData_Disposing;
            ExtendData.PropertyChanged -= ExtendData_PropertyChanged;
        }

        private void ExtendData_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            PropertyChangedHooker.Execute(e, ChangedExtendProperty);
        }
    }
}
