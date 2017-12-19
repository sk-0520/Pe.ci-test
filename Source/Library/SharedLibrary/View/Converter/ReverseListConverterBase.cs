/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.Library.SharedLibrary.View.Converter
{
    /// <summary>
    /// <para>http://stackoverflow.com/questions/7405473/reversed-listbox-without-sorting?answertab=votes#tab-top</para>
    /// </summary>
    public abstract class ReverseListConverterBase<T>: MarkupExtension, IValueConverter
    {
        private CollectionModel<T> _reversedList;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            _reversedList = new CollectionModel<T>();

            var data = (CollectionModel<T>)value;

            for(var i = data.Count - 1; i >= 0; i--)
                _reversedList.Add(data[i]);

            data.CollectionChanged += DataCollectionChanged;

            return _reversedList;
        }

        void DataCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var data = (ObservableCollection<T>)sender;

            _reversedList.Clear();
            for(var i = data.Count - 1; i >= 0; i--)
                _reversedList.Add(data[i]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
