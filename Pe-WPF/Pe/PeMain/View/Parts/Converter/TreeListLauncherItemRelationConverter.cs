namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Converter
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Data;
	using ContentTypeTextNet.Pe.PeMain.Define;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.ViewModel;

	public class TreeListLauncherItemRelationConverter: IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null) {
				return null;
			}
			var toolbarNode = (IToolbarNode)value;
			if (toolbarNode.ToolbarNodeKind == ToolbarNodeKind.Group) {
				return null;
			}
			var itemNode = (GroupItemViewMode)toolbarNode;
			return itemNode.GetModel();
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
