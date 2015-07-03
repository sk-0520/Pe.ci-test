﻿namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Define;
	using ContentTypeTextNet.Pe.PeMain.IF;

	public class GroupItemViewMode : SingleModelWrapperViewModelBase<LauncherItemModel>, IHavingNonProcess, IHavingLauncherIconCaching, IToolbarNode
	{
		//#region variable

		//bool _isSelected;

		//#endregion

		public GroupItemViewMode(LauncherItemModel model, LauncherIconCaching launcherIconCaching, INonProcess nonProcess)
			: base(model)
		{
			LauncherIconCaching = launcherIconCaching;
			NonProcess = nonProcess;
		}

		#region property

		#region IToolbarNode

		public ToolbarNodeKind ToolbarNodeKind { get { return ToolbarNodeKind.Item; } }

		#endregion

		public ObservableCollection<GroupItemViewMode> Nodes
		{
			get { return null; }
		}

		//public bool IsSelected 
		//{
		//	get { return this._isSelected; }
		//	set { SetVariableValue(ref this._isSelected, value); }
		//}

		#region IHavingLauncherIconCaching

		public LauncherIconCaching LauncherIconCaching { get; private set; }

		#endregion

		#region IHavingNonProcess

		public INonProcess NonProcess { get; private set; }

		#endregion

		#endregion
	}
}
