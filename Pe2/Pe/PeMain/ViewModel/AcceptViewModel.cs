namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Input;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	public class AcceptViewModel: SingleModelWrapperViewModelBase<RunningInformationItemModel>
	{
		public AcceptViewModel(RunningInformationItemModel model)
			: base(model)
		{ }

		#region property

		public bool CheckUpdateRelease
		{
			get { return Model.CheckUpdateRelease; }
			set { Model.CheckUpdateRelease = value; }
		}

		public bool CheckUpdateRC
		{
			get { return Model.CheckUpdateRC; }
			set { Model.CheckUpdateRC = value; }
		}

		public bool Accept
		{
			get { return Model.Accept; }
			set { Model.Accept = value; }
		}

		#endregion

		#region command


		public ICommand OkCommand
		{
			get
			{
				return new DelegateCommand(o => OnDailogCommand(o, true));
			}
		}

		public ICommand NgCommand
		{
			get
			{
				return new DelegateCommand(o => OnDailogCommand(o, false));
			}
		}

		#endregion

		#region function

		void OnDailogCommand(object sender, bool result)
		{
			Model.Accept = result;

			var window = sender as Window;
			if (window != null) {
				if (result) {
					window.DialogResult = true;
				} else {
					window.DialogResult = false;
				}
			}
		}


		#endregion
	}
}
