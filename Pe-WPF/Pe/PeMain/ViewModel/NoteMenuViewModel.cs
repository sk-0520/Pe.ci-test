namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Input;
	using System.Windows.Media;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Define;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
	using ContentTypeTextNet.Pe.PeMain.View;

	public class NoteMenuViewModel: SingleModelWrapperViewModelBase<NoteIndexItemModel>, INoteMenuItem, IHavingAppSender, IHavingNonProcess
	{
		public NoteMenuViewModel(NoteIndexItemModel model, INonProcess nonProcess,  IAppSender appSender)
			: base(model)
		{
			NonProcess = nonProcess;
			AppSender = appSender;
		}

		#region property
		#endregion

		#region command



		#endregion

		#region INoteMenuItem

		public FrameworkElement MenuImage { get { return NoteUtility.CreateMenuBox(Model); } }
		public override string DisplayText { get { return DisplayTextUtility.GetDisplayName(Model); } }

		public ICommand NoteMenuSelectedCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						NonProcess.Logger.Information("menu");
						var window = (NoteWindow)AppSender.SendCreateWindow(WindowKind.Note, Model, null);
						window.Activate();
					}
				);

				return result;
			}
		}

		#endregion

		#region IHavingNonProcess

		public INonProcess NonProcess { get; private set; }

		#endregion

		#region IHavingAppSender

		public IAppSender AppSender { get; private set; }

		#endregion
	}
}
