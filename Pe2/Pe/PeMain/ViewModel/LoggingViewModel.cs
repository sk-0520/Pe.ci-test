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
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.IF;

	public class LoggingViewModel:SingleModelWrapperViewModelBase<LoggingItemModel>, ILogAppender
	{
		#region event

		public event EventHandler ShowFront = delegate { };

		#endregion

		public LoggingViewModel(LoggingItemModel model)
			:base(model)
		{
			LogItems = new ObservableCollection<LogItemModel>();
		}

		#region property

		public ObservableCollection<LogItemModel> LogItems { get; set; }

		#endregion

		#region SingleModelWrapperViewModelBase
		#endregion

		#region ILogCollector

		public void AddLog(LogItemModel item)
		{
			Application.Current.Dispatcher.Invoke(new Action(() => LogItems.Add(item)));
		}

		#endregion
	}
}
