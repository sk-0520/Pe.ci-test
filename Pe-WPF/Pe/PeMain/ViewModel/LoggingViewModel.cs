namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Threading;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Property;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
	using ContentTypeTextNet.Pe.PeMain.View;

	public class LoggingViewModel : HavingViewSingleModelWrapperViewModelBase<LoggingSettingModel, LoggingWindow>, ILogAppender, IWindowStatus
	{
		public LoggingViewModel(LoggingSettingModel model, LoggingWindow view, CollectionModel<LogItemModel> logItems)
			: base(model, view)
		{
			if(logItems != null) {
				LogItems = logItems;
			} else {
				LogItems = new CollectionModel<LogItemModel>();
			}
		}

		#region property

		public CollectionModel<LogItemModel> LogItems { get; set; }

		#region IWindowStatus

		public double WindowLeft
		{
			get { return WindowStatusProperty.GetWindowLeft(Model); }
			set { WindowStatusProperty.SetWindowLeft(Model, value, OnPropertyChanged); }
		}

		public double WindowTop
		{
			get { return WindowStatusProperty.GetWindowTop(Model); }
			set { WindowStatusProperty.SetWindowTop(Model, value, OnPropertyChanged); }
		}

		public double WindowWidth
		{
			get { return WindowStatusProperty.GetWindowWidth(Model); }
			set { WindowStatusProperty.SetWindowWidth(Model, value, OnPropertyChanged); }
		}

		public double WindowHeight
		{
			get { return WindowStatusProperty.GetWindowHeight(Model); }
			set { WindowStatusProperty.SetWindowHeight(Model, value, OnPropertyChanged); }
		}

		public WindowState WindowState
		{
			get { return WindowStatusProperty.GetWindowState(Model); }
			set { WindowStatusProperty.SetWindowState(Model, value, OnPropertyChanged); }
		}

		#region IVisible

		public Visibility Visibility
		{
			get { return VisibleVisibilityProperty.GetVisibility(Model); }
			set { VisibleVisibilityProperty.SetVisibility(Model, value, OnPropertyChanged); }
		}

		public bool Visible
		{
			get { return VisibleVisibilityProperty.GetVisible(Model); }
			set { VisibleVisibilityProperty.SetVisible(Model, value, OnPropertyChanged); }
		}

		#endregion

		#region ITopMost

		public bool TopMost
		{
			get { return TopMostProperty.GetTopMost(Model); }
			set { TopMostProperty.SetTopMost(Model, value, OnPropertyChanged); }
		}

		#endregion

		#endregion

		#endregion

		#region HavingViewSingleModelWrapperViewModelBase

		protected override void InitializeView()
		{
			Debug.Assert(HasView);

			View.UserClosing += View_UserClosing;

			base.InitializeView();
		}

		protected override void UninitializeView()
		{
			Debug.Assert(HasView);

			View.UserClosing -= View_UserClosing;

			base.UninitializeView();
		}

		#endregion

		#region ILogCollector

		public void AddLog(LogItemModel item)
		{
			if(HasView) {
				View.Dispatcher.BeginInvoke(new Action(() => LogItems.Add(item)));
			}
		}

		#endregion

		void View_UserClosing(object sender, CancelEventArgs e)
		{
			Debug.Assert(HasView);

			e.Cancel = true;
			Visible = false;
		}
	}
}
