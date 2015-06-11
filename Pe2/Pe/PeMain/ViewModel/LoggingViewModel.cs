namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Threading;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
	using ContentTypeTextNet.Pe.PeMain.View;

	public class LoggingViewModel : HavingViewSingleModelWrapperViewModelBase<LoggingWindow, LoggingItemModel>, ILogAppender
	{
		#region event

		public event EventHandler ShowFront = delegate { };

		#endregion

		public LoggingViewModel(LoggingWindow view, LoggingItemModel model)
			: base(view, model)
		{
			LogItems = new ObservableCollection<LogItemModel>();
		}

		#region property

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

		public double Left
		{
			get { return Model.Left; }
			set 
			{
				if (Model.Left != value) {
					Model.Left = value;
					OnPropertyChanged();
				}
			}
		}
		public double Top
		{
			get { return Model.Top; }
			set
			{
				if (Model.Top != value) {
					Model.Top = value;
					OnPropertyChanged();
				}
			}
		}
		public double Width
		{
			get { return Model.Width; }
			set
			{
				if (Model.Width != value) {
					Model.Width = value;
					OnPropertyChanged();
				}
			}
		}
		public double Height
		{
			get { return Model.Height; }
			set
			{
				if (Model.Height != value) {
					Model.Height = value;
					OnPropertyChanged();
				}
			}
		}

		#endregion

		#region HavingViewSingleModelWrapperViewModelBase

		protected override void InitializeView()
		{
			base.InitializeView();
		}

		protected override void UninitializeView()
		{
			base.UninitializeView();
		}

		#endregion

		#region ILogCollector

		public void AddLog(LogItemModel item)
		{
			View.Dispatcher.BeginInvoke(new Action(() => LogItems.Add(item)));
		}

		#endregion
	}
}
