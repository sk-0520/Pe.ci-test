namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
	using ContentTypeTextNet.Pe.PeMain.View;

	public class UpdateConfirmViewModel: HavingViewModelBase<UpdateConfirmWindow>
	{
		public UpdateConfirmViewModel(UpdateConfirmWindow view, UpdateData updateDate)
			:base(view)
		{
			UpdateData = updateDate;
		}

		#region property

		UpdateData UpdateData { get; set; }

		public string NewVersion { get { return UpdateData.Info.Version; } }
		public bool IsRcVersion { get { return UpdateData.Info.IsRcVersion; } }

		#endregion

		#region HavingViewModelBase

		#endregion
	}
}
