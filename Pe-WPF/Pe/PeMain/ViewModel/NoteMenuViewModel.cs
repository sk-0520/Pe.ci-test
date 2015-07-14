namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using System.Windows.Media;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public class NoteMenuViewModel : SingleModelWrapperViewModelBase<NoteIndexItemModel>, INoteMenuItem, IHavingCommonData
	{
		public NoteMenuViewModel(NoteIndexItemModel model, CommonData commonData)
			: base(model)
		{
			CommonData = commonData;
		}

		#region property
		#endregion

		#region command



		#endregion

		#region INoteMenuItem

		public ImageSource MenuImage { get { return null; } }
		public override string DisplayText { get { return DisplayTextUtility.GetDisplayName(Model); } }

		public ICommand NoteMenuSelectedCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						CommonData.NonProcess.Logger.Information("menu");
					}
				);

				return result;
			}
		}

		#endregion

		#region IHavingCommonData

		public CommonData CommonData { get; private set; }

		#endregion
	}
}
