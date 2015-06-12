namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Logic.Property;
	using ContentTypeTextNet.Pe.PeMain.View;

	public class LauncherToolbarViewModel : HavingViewSingleModelWrapperViewModelBase<LauncherToolbarItemModel, LauncherToolbarWindow>
	{
		public LauncherToolbarViewModel(LauncherToolbarItemModel model, LauncherToolbarWindow view)
			: base(model, view)
		{
			MessageString = "appbar";
		}

		#region property

		#region IVisible

		public Visibility Visibility
		{
			get { return VisibleVisibilityProperty.GetVisibility(Model.Toolbar); }
			set { VisibleVisibilityProperty.SetVisibility(Model.Toolbar, value, OnPropertyChanged); }
		}

		public bool Visible
		{
			get { return VisibleVisibilityProperty.GetVisible(Model.Toolbar); }
			set { VisibleVisibilityProperty.SetVisible(Model.Toolbar, value, OnPropertyChanged); }
		}

		#endregion

		#region Appbar

		public uint CallbackMessage { get; set; }
		public string MessageString { get; set; }

		/// <summary>
		/// 自動的に隠すか。
		/// </summary>
		public bool AutoHide { get; set; }
		/// <summary>
		/// 隠されているか。
		/// </summary>
		public bool IsHidden { get; set; }
		/// <summary>
		/// ドッキングしているか
		/// </summary>
		public bool IsDocking { get; set; }

		public DockType DockType 
		{
			get { return Model.Toolbar.DockType; }
			set { Model.Toolbar.DockType = value; }
		}

		public Size BarSize { 
			get; 
			set; 
		}

		public Size HiddenSize
		{
			get;
			set;
		}

		public TimeSpan HiddenAnimateTime
		{
			get { return Model.Toolbar.HiddenAnimateTime; }
			set { Model.Toolbar.HiddenAnimateTime = value; }
		}

		#endregion

		#endregion
	}
}
