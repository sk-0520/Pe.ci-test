namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Window
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Extension;

	public abstract class CommonDataWindow : Window, ICommonData
	{
		public CommonDataWindow()
		{ }

		#region property

		//StartupWindowStatus Startup { get; set; }

		#endregion

		#region ICommonData

		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;

			ApplySetting();
		}

		public CommonData CommonData { get; private set; }

		#endregion

		#region function

		protected virtual void ApplySetting()
		{
			Debug.Assert(CommonData != null);

			ApplyViewModel();
			Loaded += CommonDataWindow_Loaded;
		}

		protected virtual void ApplyLanguage()
		{
			Debug.Assert(CommonData != null);

			var map = new Dictionary<Type, Action<UIElement>>() {
				{ typeof(Button), ui => ((Button)ui).SetUI(CommonData.Language) }
			};

			this.SetUI(CommonData.Language);

			foreach(var element in UIUtility.FindVisualChildren<UIElement>(this)) {
				var type = element.GetType();

				Action<UIElement> action;
				if(map.TryGetValue(type, out action)) {
					action(element);
				}
			}
		}

		protected virtual void ApplyViewModel()
		{
			Debug.Assert(CommonData != null);
		}

		#endregion

		void CommonDataWindow_Loaded(object sender, RoutedEventArgs e)
		{
			Loaded -= CommonDataWindow_Loaded;
			ApplyLanguage();
		}

	}
}
