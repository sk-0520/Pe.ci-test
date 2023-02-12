using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace ContentTypeTextNet.Pe.Core.Views.Converter
{
    public class IsEmptyCollectionConverter: IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value is ICollectionView view) {
                return view.IsEmpty;
            }

            var collection = value as IEnumerable<object>;
            if(collection != null) {
                return !collection.Any();
            }

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
