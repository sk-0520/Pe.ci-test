namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Windows.Controls;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using System.Windows;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Logic.Property;
using ContentTypeTextNet.Library.SharedLibrary.Define;

	public abstract class LauncherToolbarViewModelBase<TView> : HavingViewSingleModelWrapperViewModelBase<LauncherToolbarItemModel, TView>, IHavingNonProcess, IHavingLauncherIconCaching
		where TView : UIElement
	{
		public LauncherToolbarViewModelBase(LauncherToolbarItemModel model, TView view, LauncherIconCaching launcherIconCaching, INonProcess nonProcess)
			: base(model, view)
		{
			LauncherIconCaching = launcherIconCaching;
			NonProcess = nonProcess;
		}

		#region IHavingLauncherIconCaching

		public LauncherIconCaching LauncherIconCaching { get; private set; }

		#endregion

		#region IHavingNonProcess

		public INonProcess NonProcess { get; private set; }

		#endregion

		#region ITopMost

		public bool TopMost
		{
			get { return TopMostProperty.GetTopMost(Model.Toolbar); }
			set { TopMostProperty.SetTopMost(Model.Toolbar, value, OnPropertyChanged); }
		}

		#endregion

		public virtual DockType DockType
		{
			get { return Model.Toolbar.DockType; }
			set { SetPropertyValue(Model.Toolbar, value); }
		}

		public virtual bool AutoHide
		{
			get { return Model.Toolbar.AutoHide; }
			set { SetPropertyValue(Model.Toolbar, value); }
		}

		/// <summary>
		/// 自動的に隠すまでの時間。
		/// </summary>
		public TimeSpan HideWaitTime
		{
			get { return Model.Toolbar.HideWaitTime; }
			set { SetPropertyValue(Model.Toolbar, value); }
		}

		/// <summary>
		/// 自動的に隠す際のアニメーション時間。
		/// </summary>
		public TimeSpan HideAnimateTime
		{
			get { return Model.Toolbar.HideAnimateTime; }
			set { SetPropertyValue(Model.Toolbar, value); }
		}

	}
}
