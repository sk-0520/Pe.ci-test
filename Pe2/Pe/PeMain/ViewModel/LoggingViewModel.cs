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

		public Visibility Visible
		{
			get { return Model.Visible ? Visibility.Visible : Visibility.Hidden; }
			set 
			{
				var visible = value == Visibility.Visible;
				if (Model.Visible != visible) {
					Model.Visible = visible;
					OnPropertyChanged();
				}
			}
		}

		#endregion

		#region SingleModelWrapperViewModelBase
		#endregion

		#region ILogCollector

		public void AddLog(LogItemModel item)
		{
			View.Dispatcher.BeginInvoke(new Action(() => LogItems.Add(item)));
		}

		#endregion
	}
}
