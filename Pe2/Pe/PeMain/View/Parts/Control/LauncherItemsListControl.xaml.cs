namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Control
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;
	using System.Windows.Documents;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using System.Windows.Navigation;
	using System.Windows.Shapes;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.ViewModel;

	/// <summary>
	/// LauncherItemsListControl.xaml の相互作用ロジック
	/// </summary>
	public partial class LauncherItemsListControl: CommonDataUserControl
	{
		public LauncherItemsListControl()
		{
			InitializeComponent();

			ListItems.SelectionChanged += ListItems_SelectionChanged;
		}

		public static readonly DependencyProperty SelectedLauncherItemProperty = DependencyProperty.Register(
			"SelectedLauncherItem",
			typeof(LauncherItemModel),
			typeof(LauncherItemsListControl),
			new FrameworkPropertyMetadata(null)
		);

		public LauncherItemModel SelectedLauncherItem
		{
			get { return GetValue(SelectedLauncherItemProperty) as LauncherItemModel; }
			set 
			{ 
				SetValue(SelectedLauncherItemProperty, value);
				SelectedLauncherViewModel = new LauncherViewModelBase(SelectedLauncherItem, CommonData.LauncherIcons, CommonData.NonProcess);
			}
		}

		public static readonly DependencyProperty SelectedLauncherViewModelProperty = DependencyProperty.Register(
			"SelectedLauncherViewModel",
			typeof(LauncherViewModelBase),
			typeof(LauncherItemsListControl),
			new FrameworkPropertyMetadata(null)
		);
		public LauncherViewModelBase SelectedLauncherViewModel
		{
			get { return GetValue(SelectedLauncherViewModelProperty) as LauncherViewModelBase; }
			set { SetValue(SelectedLauncherViewModelProperty, value); }
		}

		void ListItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SelectedLauncherItem = ListItems.SelectedItem as LauncherItemModel;
		}

	}
}
