namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Control
{
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;
	using System.Windows.Documents;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using System.Windows.Navigation;
	using System.Windows.Shapes;

	/// <summary>
	/// FontSelectControl.xaml の相互作用ロジック
	/// </summary>
	public partial class FontSelectControl : CommonDataUserControl
	{
		public FontSelectControl()
		{
			InitializeComponent();
		}

		#region FamilyNameProperty

		public static readonly DependencyProperty FamilyNameProperty = DependencyProperty.Register(
			"FamilyName",
			typeof(FontFamily),
			typeof(FontSelectControl),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(OnFamilyNameChanged))
		);


		// 2. CLI用プロパティを提供するラッパー
		public FontFamily FamilyName
		{
			get { return (FontFamily)GetValue(FamilyNameProperty); }
			set { SetValue(FamilyNameProperty, value); }
		}

		static void OnFamilyNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var ctrl = d as FontSelectControl;
			if (ctrl != null) {
				ctrl.FamilyName = (FontFamily)e.NewValue;
			}
		}

		#endregion

		#region IsBoldProperty

		public static readonly DependencyProperty IsBoldProperty = DependencyProperty.Register(
			"IsBold",
			typeof(bool),
			typeof(FontSelectControl),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(OnIsBoldChanged))
		);


		// 2. CLI用プロパティを提供するラッパー
		public bool IsBold
		{
			get { return (bool)GetValue(IsBoldProperty); }
			set { SetValue(IsBoldProperty, value); }
		}

		static void OnIsBoldChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var ctrl = d as FontSelectControl;
			if (ctrl != null) {
				ctrl.IsBold = (bool)e.NewValue;
			}
		}

		#endregion

		#region IsItalicProperty

		public static readonly DependencyProperty IsItalicProperty = DependencyProperty.Register(
			"IsItalic",
			typeof(bool),
			typeof(FontSelectControl),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(OnIsItalicChanged))
		);


		// 2. CLI用プロパティを提供するラッパー
		public bool IsItalic
		{
			get { return (bool)GetValue(IsItalicProperty); }
			set { SetValue(IsItalicProperty, value); }
		}

		static void OnIsItalicChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var ctrl = d as FontSelectControl;
			if (ctrl != null) {
				ctrl.IsItalic = (bool)e.NewValue;
			}
		}

		#endregion

		#region SizeProperty

		public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
			"Size",
			typeof(double),
			typeof(FontSelectControl),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSizeChanged))
		);


		// 2. CLI用プロパティを提供するラッパー
		public double Size
		{
			get { return (double)GetValue(SizeProperty); }
			set { SetValue(SizeProperty, value); }
		}

		static void OnSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var ctrl = d as FontSelectControl;
			if (ctrl != null) {
				ctrl.Size = (double)e.NewValue;
			}
		}

		#endregion

		#region SizeMinimumProperty

		public static readonly DependencyProperty SizeMinimumProperty = DependencyProperty.Register(
			"SizeMinimum",
			typeof(double),
			typeof(FontSelectControl),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSizeMinimumChanged))
		);


		// 2. CLI用プロパティを提供するラッパー
		public double SizeMinimum
		{
			get { return (double)GetValue(SizeMinimumProperty); }
			set { SetValue(SizeMinimumProperty, value); }
		}

		static void OnSizeMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var ctrl = d as FontSelectControl;
			if (ctrl != null) {
				ctrl.SizeMinimum = (double)e.NewValue;
			}
		}

		#endregion

		#region SizeMaximumProperty

		public static readonly DependencyProperty SizeMaximumProperty = DependencyProperty.Register(
			"SizeMaximum",
			typeof(double),
			typeof(FontSelectControl),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSizeMaximumChanged))
		);


		// 2. CLI用プロパティを提供するラッパー
		public double SizeMaximum
		{
			get { return (double)GetValue(SizeMaximumProperty); }
			set { SetValue(SizeMaximumProperty, value); }
		}

		static void OnSizeMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var ctrl = d as FontSelectControl;
			if (ctrl != null) {
				ctrl.SizeMaximum = (double)e.NewValue;
			}
		}

		#endregion

	}
}
