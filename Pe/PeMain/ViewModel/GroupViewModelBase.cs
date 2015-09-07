namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.Define;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public abstract class GroupViewModelBase<TModel>: SingleModelWrapperViewModelBase<TModel>, IHavingAppNonProcess, IToolbarNode, IRefreshFromViewModel
		where TModel: IModel, ITId<Guid>, IName
	{
		#region variable

		bool _isSelected;

		#endregion

		public GroupViewModelBase(TModel model, IAppNonProcess appNonProcess)
			:base(model)
		{
			AppNonProcess = appNonProcess;
		}

		#region property

		public Guid Id { get { return Model.Id; } }

		#endregion

		#region IToolbarNode

		public abstract ToolbarNodeKind ToolbarNodeKind { get; }
		public bool IsExpanded { get { return true; } }
		public bool IsSelected {
			get { return this._isSelected; }
			set { SetVariableValue(ref this._isSelected, value); }
		}
		public abstract string Name { get; set; }
		public abstract bool CanEdit { get; }
		public virtual BitmapSource Image { get { return null; } }

		#endregion

		#region IHavingAppNonProcess

		public IAppNonProcess AppNonProcess { get; private set; }

		#endregion

		#region SingleModelWrapperViewModelBase

		public override string DisplayText { get { return DisplayTextUtility.GetDisplayName(Model); } }

		#endregion

		#region IRefreshFromViewModel

		public void Refresh()
		{
			CallOnPropertyChangeDisplayItem();
		}

		#endregion
	}
}
