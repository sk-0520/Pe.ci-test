namespace ContentTypeTextNet.Library.SharedLibrary.View.Converter
{
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
