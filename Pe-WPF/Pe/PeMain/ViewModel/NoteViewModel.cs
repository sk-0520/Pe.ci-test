namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Input;
	using System.Windows.Media;
	using ContentTypeTextNet.Library.SharedLibrary.Attribute;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.IF.WindowsViewExtend;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.IF.ViewExtend;
	using ContentTypeTextNet.Pe.PeMain.Logic.Property;
	using ContentTypeTextNet.Pe.PeMain.View;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;

	public class NoteViewModel: HavingViewSingleModelWrapperViewModelBase<NoteIndexItemModel, NoteWindow>, IHavingNonProcess, IHavingClipboardWatcher, IWindowHitTestData, IWindowAreaCorrectionData, ICaptionDoubleClickData, IHavingAppSender
	{
		#region variable

		IndexBodyItemModelBase _indexBody = null;
		double _compactHeight;

		#endregion

		public NoteViewModel(NoteIndexItemModel model, NoteWindow view, INonProcess nonProcess, IClipboardWatcher clipboardWatcher, IAppSender appSender)
			: base(model, view)
		{
			NonProcess = nonProcess;
			ClipboardWatcher = clipboardWatcher;
			AppSender = appSender;

			SetCompactArea();
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

		public bool IsCompacted
		{
			get { return Model.IsCompacted; }
			set {
				if (SetModelValue(value)) {
					if (value) {
						SetCompactArea();
					}
					OnPropertyChanged("WindowHeight");
				}
			}
		}

		public Color ForeColor
		{
			get { return Model.ForeColor; }
			set { SetModelValue(value); }
		}

		public Color BackColor
		{
			get { return Model.BackColor; }
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
				if(SetPropertyValue(indexBody, value, "Text")) {
					indexBody.History.Update();
					AppSender.SendSaveIndexBody(IndexKind.Note, Model.Id, IndexBody);
				}
			}
		}

		#endregion

		#region HavingViewSingleModelWrapperViewModelBase

		protected override void InitializeView()
		{
			SetCompactArea();

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
			set 
			{
				if (!IsLocked) {
					WindowAreaProperty.SetWindowLeft(Model, value, OnPropertyChanged); 
				}
			}
		}

		public double WindowTop
		{
			get { return WindowAreaProperty.GetWindowTop(Model); }
			set
			{
				if (!IsLocked) {
					WindowAreaProperty.SetWindowTop(Model, value, OnPropertyChanged);
				}
			}
		}
		public double WindowWidth
		{
			get 
			{
				return WindowAreaProperty.GetWindowWidth(Model); 
			}
			set 
			{
				if (!IsLocked && !IsCompacted) {
					WindowAreaProperty.SetWindowWidth(Model, value, OnPropertyChanged);
				}
			}
		}
		public double WindowHeight
		{
			get {
				if (IsCompacted) {
					return this._compactHeight;
				} else {
					return WindowAreaProperty.GetWindowHeight(Model);
				}
			}
			set 
			{ 
				if (!IsLocked && !IsCompacted) {
					WindowAreaProperty.SetWindowHeight(Model, value, OnPropertyChanged);
				}
			}
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
		public bool UsingBorderHitTest { get { return !(IsCompacted || IsLocked); } }

		public bool UsingCaptionHitTest { get { return !IsLocked; } }

		/// <summary>
		/// タイトルバーとして認識される領域。
		/// </summary>
		[PixelKind(Px.Logical)]
		public Rect CaptionArea
		{
			get
			{
				var resizeThickness = ResizeThickness;
				var rect = new Rect(
					resizeThickness.Left,
					resizeThickness.Top,
					View.Caption.ActualWidth,
					TitleHeight
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

		#region command

		public ICommand SaveIndexCommnad
		{
			get
			{
				var result = CreateCommand(
					o => {
						if(IsChanged) {
							Model.History.Update();
							AppSender.SendIndexSave(IndexKind.Note);
							ResetChangeFlag();
						}
						if(HasView) {
							// フォーカス外れたときにうまいこと反映されない対策
							Body = View.body.Text;
							ResetChangeFlag();
						}
					}
				);

				return result;
			}
		}

		public ICommand SwitchCompactCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						IsCompacted = !IsCompacted;
					}
				);

				return result;
			}
		}

		public ICommand SwitchTopMostCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						TopMost = !TopMost;
					}
				);

				return result;
			}
		}

		public ICommand HideCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						if (HasView) {
							// 表示切替はイベント内で実施。
							View.UserClose();
						}
					}
				);

				return result;
			}
		}



		#endregion

		#region function

		void SetCompactArea()
		{
			this._compactHeight = CaptionArea.Height + ResizeThickness.GetVertical();
		}

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
