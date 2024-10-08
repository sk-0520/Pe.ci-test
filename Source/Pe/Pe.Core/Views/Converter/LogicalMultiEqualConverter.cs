using System;
using System.Windows.Data;
using ContentTypeTextNet.Pe.Library.Base.Linq;

namespace ContentTypeTextNet.Pe.Core.Views.Converter
{
    /// <summary>
    /// 指定された値が全て同じか。
    /// </summary>
    public class LogicalMultiEqualConverter: IMultiValueConverter
    {
        #region IValueConverter

        public virtual object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return values.AllEquals();
        }

        public virtual object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
