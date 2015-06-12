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
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
	using ContentTypeTextNet.Pe.PeMain.View;

	public class LoggingViewModel : HavingViewSingleModelWrapperViewModelBase<LoggingWindow, LoggingItemModel>, ILogAppender, IWindowStatus
	{
		public LoggingViewModel(LoggingItemModel model, LoggingWindow view)
			: base(model, view)
		{
			LogItems = new ObservableCollection<LogItemModel>();
		}

		#region property

		#region IWindowStatus

		public double WindowLeft
		{
			get { return Model.WindowLeft; }
			set
			{
				if (Model.WindowLeft != value && Model.WindowState == WindowState.Normal) {
					Model.WindowLeft = value;
					OnPropertyChanged();
				}
			}
		}

		public double WindowTop
		{
			get { return Model.WindowTop; }
			set
			{
				if (Model.WindowTop != value && Model.WindowState == WindowState.Normal) {
					Model.WindowTop = value;
					OnPropertyChanged();
				}
			}
		}

		public double WindowWidth
		{
			get { return Model.WindowWidth; }
			set
			{
				if (Model.WindowWidth != value && Model.WindowState == WindowState.Normal) {
					Model.WindowWidth = value;
					OnPropertyChanged();
				}
			}
		}

		public double WindowHeight
		{
			get { return Model.WindowHeight; }
			set
			{
				if (Model.WindowHeight != value && Model.WindowState == WindowState.Normal) {
					Model.WindowHeight = value;
					OnPropertyChanged();
				}
			}
		}

		public WindowState WindowState
		{
			get { return Model.WindowState; }
			set
			{
				if (Model.WindowState != value) {
					Model.WindowState = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion


		public ObservableCollection<LogItemModel> LogItems { get; set; }

		public Visibility Visibility
		{
			get { return ConvertUtility.To(Visible); }
			set { Visible = ConvertUtility.To(value); }
		}

		public bool Visible 
		{
			get { return Model.Visible; }
			set
			{
				if (Model.Visible != value) {
					Model.Visible = value;
					OnPropertyChanged();
					OnPropertyChanged("Visibility");
				}
			}
		}

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
		}
	}
}
