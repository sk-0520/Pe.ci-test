namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Property;
	using ContentTypeTextNet.Pe.PeMain.View;

	public class NoteViewModel: HavingViewSingleModelWrapperViewModelBase<NoteItemModel, NoteWindow>, IHavingNonProcess, IHavingClipboardWatcher
	{
		public NoteViewModel(NoteItemModel model, NoteWindow view, INonProcess nonProcess, IClipboardWatcher clipboardWatcher)
			: base(model, view)
		{
			NonProcess = nonProcess;
			ClipboardWatcher = clipboardWatcher;
		}

		#region property
		#region ITopMost

		public bool TopMost
		{
			get { return TopMostProperty.GetTopMost(Model); }
			set { TopMostProperty.SetTopMost(Model, value, OnPropertyChanged); }
		}

		#endregion

		#region IVisible

		public Visibility Visibility
		{
			get { return VisibleVisibilityProperty.GetVisibility(Model); }
			set { VisibleVisibilityProperty.SetVisibility(Model, value, OnPropertyChanged); }
		}

		public bool Visible
		{
			get { return VisibleVisibilityProperty.GetVisible(Model); }
			set { VisibleVisibilityProperty.SetVisible(Model, value, OnPropertyChanged); }
		}

		#endregion

		#region window

		public double WindowLeft
		{
			get { return WindowAreaProperty.GetWindowLeft(Model); }
			set { WindowAreaProperty.SetWindowLeft(Model, value, OnPropertyChanged);}
		}

		public double WindowTop
		{
			get { return WindowAreaProperty.GetWindowTop(Model); }
			set { WindowAreaProperty.SetWindowTop(Model, value, OnPropertyChanged); 
			}
		}
		public double WindowWidth
		{
			get { return WindowAreaProperty.GetWindowWidth(Model); }
			set { WindowAreaProperty.SetWindowWidth(Model, value, OnPropertyChanged); }
		}
		public double WindowHeight
		{
			get { return WindowAreaProperty.GetWindowHeight(Model); }
			set { WindowAreaProperty.SetWindowHeight(Model, value, OnPropertyChanged); }
		}

		#endregion

		#endregion

		#region IHavingNonProcess

		public INonProcess NonProcess { get; private set; }

		#endregion

		#region IHavingClipboardWatcher

		public IClipboardWatcher ClipboardWatcher { get; private set; }

		#endregion

	}
}
