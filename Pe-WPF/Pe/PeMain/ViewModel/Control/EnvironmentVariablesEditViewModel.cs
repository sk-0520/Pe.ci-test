namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	public class EnvironmentVariablesEditViewModel : SingleModelWrapperViewModelBase<EnvironmentVariablesItemModel>, IHavingNonProcess
	{
		public EnvironmentVariablesEditViewModel(EnvironmentVariablesItemModel model, INonProcess appNonProcess)
			: base(model)
		{
			NonProcess = appNonProcess;
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

		public EnvironmentVariableUpdateItemCollectionModel Update
		{
			get { return Model.Update; }
		}

		public string Remove 
		{
			get { return string.Join(Environment.NewLine, Model.Remove); }
			set
			{
				var removes = value.SplitLines();
				Model.Remove = new CollectionModel<string>(removes);
			}
		}



		#endregion


	}
}
