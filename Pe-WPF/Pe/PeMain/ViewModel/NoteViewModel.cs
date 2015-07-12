namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.IF.WindowsViewExtend;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.Library.PeData.IF;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.IF.ViewExtend;
using ContentTypeTextNet.Pe.PeMain.Logic.Property;
using ContentTypeTextNet.Pe.PeMain.View;

	public class NoteViewModel: HavingViewSingleModelWrapperViewModelBase<NoteIndexItemModel, NoteWindow>, IHavingNonProcess, IHavingClipboardWatcher, IWindowHitTestData, IWindowAreaCorrectionData, ICaptionDoubleClickData, IHavingAppSender
	{
		#region variable

		IIndexBody _indexBody;

		#endregion

		public NoteViewModel(NoteIndexItemModel model, NoteWindow view, INonProcess nonProcess, IClipboardWatcher clipboardWatcher, IAppSender appSender)
			: base(model, view)
		{
			NonProcess = nonProcess;
			ClipboardWatcher = clipboardWatcher;
			AppSender = appSender;
		}

		#region property

		NoteBodyItemModel IndexBody { get { return this._indexBody as NoteBodyItemModel; } }

		public bool IsTemporary { get; set; }
		public bool IsRemove { get; set; }

		public Brush BorderBrush
		{
			get
			{
				return new SolidColorBrush(Colors.Red);
			}
		}

		public double TitleHeight { get { return 20; } }

		public Visibility CaptionButtonVisibility
		{
			get
			{
				if(IsLocked) {
					return Visibility.Collapsed;
				} else {
					return Visibility.Visible;
				}
			}
		}

		public bool IsLocked
		{
			get { return Model.IsLocked; }
			set { SetModelValue(value); }
		}

		public string Body
		{
			get 
			{
				if(IndexBody == null) {
					this._indexBody = AppSender.SendGetIndexBody(Library.PeData.Define.IndexKind.Note, Model.Id);
					if(this._indexBody == null) {
						this._indexBody = new NoteBodyItemModel();
					}
				}
				var indexBody = IndexBody;
				return indexBody.Text ?? string.Empty;
			}
			set
			{
				if(IndexBody == null) {
					this._indexBody = new NoteBodyItemModel();
				}
				var indexBody = IndexBody;
				SetPropertyValue(indexBody, value, "Text");
			}
		}

		#endregion

		#region HavingViewSingleModelWrapperViewModelBase

		protected override void InitializeView()
		{
			View.UserClosing += View_UserClosing;

			base.InitializeView();
		}

		protected override void UninitializeView()
		{
			View.UserClosing -= View_UserClosing;

			base.UninitializeView();
		}

		#endregion

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
			set { WindowAreaProperty.SetWindowLeft(Model, value, OnPropertyChanged); }
		}

		public double WindowTop
		{
			get { return WindowAreaProperty.GetWindowTop(Model); }
			set
			{
				WindowAreaProperty.SetWindowTop(Model, value, OnPropertyChanged);
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

		#region IHavingNonProcess

		public INonProcess NonProcess { get; private set; }

		#endregion

		#region IHavingClipboardWatcher

		public IClipboardWatcher ClipboardWatcher { get; private set; }

		#endregion

		#region IWindowHitTestData

		/// <summary>
		/// ヒットテストを行うか
		/// </summary>
		public bool UsingHitTest { get{return true;} }

		/// <summary>
		/// タイトルバーとして認識される領域。
		/// </summary>
		[PixelKind(Px.Logical)]
		public Rect CaptionArea {
			get 
			{
				var resizeThickness = ResizeThickness;
				var rect = new Rect(
					resizeThickness.Left,
					resizeThickness.Top,
					View.Caption.ActualWidth,
					View.Caption.ActualHeight
				);

				return rect; 
			} 
		}
		/// <summary>
		/// サイズ変更に使用する境界線。
		/// </summary>
		[PixelKind(Px.Logical)]
		public Thickness ResizeThickness { get { return new Thickness(8); } }

		#endregion

		#region IWindowAreaCorrectionData

		/// <summary>
		/// ウィンドウサイズの倍数制御を行うか。
		/// </summary>
		public bool UsingMultipleResize { get { return false; } }
		/// <summary>
		/// ウィンドウサイズの倍数制御に使用する元となる論理サイズ。
		/// </summary>
		[PixelKind(Px.Logical)]
		public Size MultipleSize { get { throw new NotImplementedException(); } }
		/// <summary>
		/// タイトルバーとかボーダーを含んだ領域。
		/// </summary>
		[PixelKind(Px.Logical)]
		public Thickness MultipleThickness { get { throw new NotImplementedException(); } }
		/// <summary>
		/// 移動制限を行うか。
		/// </summary>
		public bool UsingMoveLimitArea { get { return false; } }
		/// <summary>
		/// 移動制限に使用する論理領域。
		/// </summary>
		[PixelKind(Px.Logical)]
		public Rect MoveLimitArea { get { throw new NotImplementedException(); } }
		/// <summary>
		/// 最大化・最小化を抑制するか。
		/// </summary>
		public bool UsingMaxMinSuppression { get { return true; } }

		#endregion

		#region ICaptionDoubleClickData

		public void OnCaptionDoubleClick(object sender, CancelEventArgs e)
		{ }

		#endregion

		#region IHavingAppSender

		public IAppSender AppSender { get; private set; }

		#endregion

		private void View_UserClosing(object sender, CancelEventArgs e)
		{
			Visible = false;
			if(HasView) {
				AppSender.SendWindowRemove(View);
			}
		}

	}
}
