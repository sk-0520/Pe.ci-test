using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Embedded.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.View
{
    public interface IExtendData : INotifyPropertyChanged, IDisposer
    { }

    public abstract class ViewExtendBase<TView, TExtendData> : DisposerBase
        where TView : UIElement
        where TExtendData : IExtendData
    {
        public ViewExtendBase(TView view, TExtendData extendData, ILoggerFactory loggerFactory)
        {
            View = view;
            ExtendData = extendData;
            Logger = loggerFactory.CreateTartget(GetType());

            ExtendData.Disposing += ExtendData_Disposing;
            ExtendData.PropertyChanged += ExtendData_PropertyChanged;

            var extendDataType = typeof(TExtendData);
            var extendDataProps = extendDataType
                .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Select(p => p.Name)
            ;
            PropertyNames = new HashSet<string>(extendDataProps);
        }

        #region property

        protected TView View { get; private set; }
        protected TExtendData ExtendData { get; private set; }
        protected ILogger Logger { get; }

        protected ISet<string> PropertyNames { get; }

        #endregion

        #region function

        protected abstract void ChangedExtendProperty(string propertyName);

        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                View = null;
                ExtendData.Disposing -= ExtendData_Disposing;
                ExtendData.PropertyChanged -= ExtendData_PropertyChanged;
            }

            base.Dispose(disposing);
        }

        #endregion

        private void ExtendData_Disposing(object sender, EventArgs e)
        {
            ExtendData.Disposing -= ExtendData_Disposing;
            ExtendData.PropertyChanged -= ExtendData_PropertyChanged;
        }

        private void ExtendData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(PropertyNames.Contains(e.PropertyName)) {
                ChangedExtendProperty(e.PropertyName);
            }
        }
    }
}
