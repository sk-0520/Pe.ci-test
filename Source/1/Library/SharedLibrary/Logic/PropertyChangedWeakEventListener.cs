using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
    public class PropertyChangedWeakEventListener: WeakEventListener<PropertyChangedEventManager, PropertyChangedEventArgs>
    {
        public PropertyChangedWeakEventListener(EventHandler<PropertyChangedEventArgs> handler) 
            : base(handler)
        { }

        #region function

        public void Add(INotifyPropertyChanged target, string propertyName)
        {
            PropertyChangedEventManager.AddListener(target, this, propertyName);
        }
        public void Add(INotifyPropertyChanged target)
        {
            Add(target, string.Empty);
        }

        public void Remove(INotifyPropertyChanged target, string propertyName)
        {
            PropertyChangedEventManager.RemoveListener(target, this, propertyName);
        }

        public void Remove(INotifyPropertyChanged target)
        {
            Remove(target, string.Empty);
        }

        #endregion
    }
}
