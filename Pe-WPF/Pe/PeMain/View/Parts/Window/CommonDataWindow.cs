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
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.View;
	using ContentTypeTextNet.Library.SharedLibrary.View.Window;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.IF;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Extension;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public abstract class CommonDataWindow: UserClosableWindowWindowBase, ICommonData
	{
		public CommonDataWindow()
			:base()
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

		#region UserClosableWindowWindowBase

		protected override void OnLoaded(object sender, RoutedEventArgs e)
		{
			base.OnLoaded(sender, e);

			ApplyLanguage();
			SetChildCommonData();
		}

		#endregion

		#region function

		protected virtual void ApplySetting()
		{
			Debug.Assert(CommonData != null);

			CreateViewModel();
			ApplyViewModel();
		}

		protected virtual void CreateViewModel()
		{ }

		protected virtual void ApplyLanguage()
		{
			Debug.Assert(CommonData != null);

			LanguageUtility.SetLanguage(this, CommonData.Language);
		}

		protected virtual void ApplyViewModel()
		{
			Debug.Assert(CommonData != null);
		}

		protected virtual void SetChildCommonData()
		{
			Debug.Assert(CommonData != null);

			foreach(var ui in UIUtility.FindLogicalChildren<UserControl>(this).OfType<ICommonData>()) {
				ui.SetCommonData(CommonData);
			}
		}


		#endregion

	}
}
