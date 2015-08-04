namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
	using ContentTypeTextNet.Pe.Library.PeData.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
	using ContentTypeTextNet.Pe.PeMain.Logic.Property;
	using ContentTypeTextNet.Pe.PeMain.View;
	using System.Diagnostics;

	public class CommandViewModel : HavingViewSingleModelWrapperViewModelBase<CommandSettingModel, CommandWindow>
	{
		#region variable
		
		double _windowLeft, _windowTop;
		Visibility _visibility = Visibility.Hidden;
		
		#endregion

		public CommandViewModel(CommandSettingModel model, CommandWindow view)
			: base(model, view)
		{ }

		#region property

		public double WindowLeft
		{
			get { return this._windowLeft; }
			set { SetVariableValue(ref this._windowLeft, value); }
		}

		public double WindowTop
		{
			get { return this._windowTop; }
			set { SetVariableValue(ref this._windowTop, value); }
		}

		public double WindowWidth
		{
			get { return Model.WindowWidth; }
			set { SetModelValue(value); }
		}

		public Visibility Visibility
		{
			get { return this._visibility; }
			set { SetVariableValue(ref this._visibility, value); }
		}

		public double IconWidth { get { return Model.IconScale.ToWidth(); } }
		public double IconHeight { get { return Model.IconScale.ToHeight(); } }

		#endregion

		#region HavingViewSingleModelWrapperIndexViewModelBase

		protected override void InitializeView()
		{
			Debug.Assert(HasView);

			View.UserClosing += View_UserClosing;

			base.InitializeView();
		}

		protected override void UninitializeView()
		{
			Debug.Assert(HasView);

			View.UserClosing -= View_UserClosing;

			base.UninitializeView();
		}

		#endregion

		private void View_UserClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = true;
			Visibility = Visibility.Hidden;
		}

	}
}
