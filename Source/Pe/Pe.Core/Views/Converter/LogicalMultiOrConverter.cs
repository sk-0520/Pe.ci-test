using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Core.Views.Converter
{
    public class LogicalMultiOrConverter: IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(values == null) {
                return false;
            }

            foreach(var b in values) {
                try {
                    if(CastUtility.GetCastWPFValue<bool>(b, false)) {
                        return true;
                    }
                }catch(Exception ex) {
                    Debug.WriteLine(ex);
                }
            }

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
