namespace ContentTypeTextNet.Pe.PeMain.ViewModel.Control
{
	using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.PeMain.Data;

	public class LauncherItemEditViewModel: LauncherSimpleViewModel
	{
		public LauncherItemEditViewModel(LauncherItemModel model, LauncherIconCaching launcherIconCaching, INonProcess nonPorocess)
			: base(model, launcherIconCaching, nonPorocess)
		{ }

		#region property

		public string Name
		{
			get { return Model.Name; }
			set { SetModelValue(value); }
		}

		public override LauncherKind LauncherKind
		{
			get { return base.LauncherKind; }
			set { SetModelValue(value); }
		}

		public string IconPath
		{
			get { return Model.Icon.DisplayText; }
		}

		public EnvironmentVariablesEditViewModel EnvironmentVariables
		{
			get { return new EnvironmentVariablesEditViewModel(Model.EnvironmentVariables, NonProcess); }
		}

		#endregion
	}
}
