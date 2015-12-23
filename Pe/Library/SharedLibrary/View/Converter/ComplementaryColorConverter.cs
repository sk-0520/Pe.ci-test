namespace ContentTypeTextNet.Library.SharedLibrary.View.Converter
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Data;
    using System.Windows.Media;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

    public class ComplementaryColorConverter: IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color =  CastUtility.GetCastWPFValue(value, Colors.IndianRed);
            return MediaUtility.GetComplementaryColor(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
