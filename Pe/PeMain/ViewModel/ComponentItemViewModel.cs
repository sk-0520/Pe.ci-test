namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public class ComponentItemViewModel: SingleModelWrapperViewModelBase<ComponentItemModel>, IHavingAppNonProcess
	{
		public ComponentItemViewModel(ComponentItemModel model, IAppNonProcess appNonProcess)
			: base(model)
		{
			AppNonProcess = appNonProcess;
		}

		#region proeprty

		public string License { get { return Model.License; } }
		public ComponentKind ComponentKind { get { return Model.Kind; } }

		#endregion

		#region command

		public ICommand OpenComponentCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						try {
							ExecuteUtility.ExecuteCommand(Model.Uri, AppNonProcess);
						} catch(Exception ex) {
							AppNonProcess.Logger.Error(ex);
						}
					}
				);

				return result;
			}
		}

		#endregion

		#region SingleModelWrapperViewModelBase

		public override string DisplayText
		{
			get
			{
				return Model.Name;
			}
		}

		#endregion

		#region IHavingAppNonProcess

		public IAppNonProcess AppNonProcess { get; private set; }

		#endregion
	}
}
