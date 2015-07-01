namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	public class EnvironmentVariablesEditViewModel : SingleModelWrapperViewModelBase<EnvironmentVariablesItemModel>, IHavingNonProcess
	{
		public EnvironmentVariablesEditViewModel(EnvironmentVariablesItemModel model, INonProcess nonProcess)
			: base(model)
		{
			NonProcess = nonProcess;
		}

		#region property
		
		#region IHavingNonProcess

		public INonProcess NonProcess { get; private set; }

		#endregion

		public bool Edit
		{
			get { return Model.Edit; }
			set { SetModelValue(value); }
		}


		#endregion


	}
}
