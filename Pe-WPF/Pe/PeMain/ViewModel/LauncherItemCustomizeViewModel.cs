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
	using ContentTypeTextNet.Pe.PeMain.Define;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
	using ContentTypeTextNet.Pe.PeMain.View;
	using ContentTypeTextNet.Pe.PeMain.ViewModel.Control;

	/// <summary>
	/// <para>内部でモデルを元保持して最後に再設定する。</para>
	/// </summary>
	public class LauncherItemCustomizeViewModel: LauncherItemEditViewModel, IHavingView<LauncherItemCustomizeWindow>, IHavingAppSender
	{
		#region variable

		LauncherItemModel _srcModel;

		#endregion

		public LauncherItemCustomizeViewModel(LauncherItemModel model, LauncherItemCustomizeWindow view, IAppNonProcess nonPorocess, IAppSender appSender)
			: base((LauncherItemModel)model.DeepClone(), nonPorocess, appSender)
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
						this._srcModel.Name = Model.Name;
						this._srcModel.LauncherKind = Model.LauncherKind;
						this._srcModel.Command = Model.Command;
						this._srcModel.Option = Model.Option;
						this._srcModel.WorkDirectoryPath = Model.WorkDirectoryPath;
						this._srcModel.Icon = Model.Icon;
						this._srcModel.History = Model.History;
						this._srcModel.Comment = Model.Comment;
						this._srcModel.Tag = Model.Tag;
						this._srcModel.StdStream = Model.StdStream;
						this._srcModel.Administrator = Model.Administrator;
						this._srcModel.EnvironmentVariables = Model.EnvironmentVariables;

						if(HasView) {
							View.Close();
						}
						SettingUtility.IncrementLauncherItem(Model, null, null, AppNonProcess);
						AppNonProcess.LauncherIconCaching.Remove(this._srcModel);
						AppSender.SendRefreshView(WindowKind.LauncherToolbar, null);
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
