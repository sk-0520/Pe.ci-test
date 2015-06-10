namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.IF;

	public class LoggingViewModel:SingleModelWrapperViewModelBase<LoggingItemModel>, ILogCollector
	{
		public LoggingViewModel(LoggingItemModel model)
			:base(model)
		{ }

		#region property
		#endregion

		#region SingleModelWrapperViewModelBase
		#endregion

		#region ILogCollector

		public void Puts(LogItemModel item)
		{ }

		#endregion
	}
}
