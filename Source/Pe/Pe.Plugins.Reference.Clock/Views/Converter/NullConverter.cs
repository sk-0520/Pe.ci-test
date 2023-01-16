using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace ContentTypeTextNet.Pe.Plugins.Reference.Clock.Views.Converter
{
    /// <summary>
    /// DataContext が null の際にバインドエラーがばんばん出るのを抑制するためだけの人。
    /// <para>知らんし・・・ System.Windows.Data Error: 5 : Value produced by BindingExpression is not valid for target property. null BindingExpression:Path=; DataItem=null; target element is 'RotateTransform' (HashCode=37304191); target property is 'Angle' (type 'Double')</para>
    /// </summary>
    [ValueConversion(typeof(double), typeof(double))]
    public class NullConverter: IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null) {
                return System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
