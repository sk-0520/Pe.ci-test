﻿namespace ContentTypeTextNet.Pe.PeMain.View.Parts.WindowBase
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic;

	public class CommonDataWindow : Window, ICommonData
	{
		public CommonDataWindow()
		{
			//Startup = CreateStartupWindowStatus();
		}

		#region property

		//StartupWindowStatus Startup { get; set; }

		#endregion

		#region ICommonData

		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;

			ApplySetting();
			
			//Startup.UndoWindowState();
			//Startup = null;
		}

		public CommonData CommonData { get; private set; }

		#endregion

		#region function

		protected virtual StartupWindowStatus CreateStartupWindowStatus()
		{
			return new StartupWindowStatus(this);
		}

		protected virtual void ApplySetting()
		{
			Debug.Assert(CommonData != null);

			ApplyViewModel();
			Loaded += CommonDataWindow_Loaded;
		}

		protected virtual void ApplyLanguage()
		{
			Debug.Assert(CommonData != null);

			foreach(var element in UIUtility.FindVisualChildren<UIElement>(this)) {
				CommonData.Logger.Information(element.ToString());
			}
		}

		protected virtual void ApplyViewModel()
		{
			Debug.Assert(CommonData != null);
		}

		#endregion

		void CommonDataWindow_Loaded(object sender, RoutedEventArgs e)
		{
			ApplyLanguage();
			Loaded -= CommonDataWindow_Loaded;
		}

	}
}
