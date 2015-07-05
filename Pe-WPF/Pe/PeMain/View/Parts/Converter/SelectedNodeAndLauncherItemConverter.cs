namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Converter
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Controls;
	using System.Windows.Data;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.ViewModel;

	public class SelectedNodeAndLauncherItemConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var result = new SelectedNodeAndLauncherItem();
			result.SelectedNode = values[0] as IToolbarNode;
			result.LauncherItem = values[1] == null
				? default(LauncherItemModel)
				: ((LauncherViewModelBase)values[1]).GetModel()
			;

			return result;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
