namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.View;
	using ContentTypeTextNet.Pe.PeMain.ViewModel.Control;

	/// <summary>
	/// <para>内部でモデルを元保持して最後に再設定する。</para>
	/// </summary>
	public class LauncherItemCustomizeViewModel: LauncherItemEditViewModel, IHavingView<LauncherItemCustomizeWindow>
	{
		#region variable

		LauncherItemModel _srcModel;

		#endregion

		public LauncherItemCustomizeViewModel(LauncherItemModel model, LauncherItemCustomizeWindow view, LauncherIconCaching launcherIconCaching, INonProcess nonPorocess)
			: base((LauncherItemModel)model.DeepClone(), launcherIconCaching, nonPorocess)
		{
			View = view;
			this._srcModel = model;
		}

		#region property
		#endregion

		#region command

		public ICommand SaveCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
					}
				);

				return result;
			}
		}

		public ICommand CancelCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						if(HasView) {
							View.Close();
						}
					}
				);

				return result;
			}
		}

		#endregion

		#region LauncherItemCustomizeWindow

		public LauncherItemCustomizeWindow View { get; private set; }

		public bool HasView { get { return View != null; } }

		#endregion
	}
}
