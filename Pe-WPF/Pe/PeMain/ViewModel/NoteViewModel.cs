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
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public class NoteViewModel : HavingViewSingleModelWrapperViewModelBase<NoteIndexItemModel, NoteWindow>, IHavingNonProcess, IHavingClipboardWatcher, IWindowHitTestData, IWindowAreaCorrectionData, ICaptionDoubleClickData, IHavingAppSender, IColorPair, INoteMenuItem
	{
		#region static

		public Thickness CaptionPadding { get { return Constants.noteCaptionPadding; } }

		#endregion

		#region variable

		IndexBodyItemModelBase _indexBody = null;
		double _compactHeight;
		Visibility _titleEditVisibility = Visibility.Collapsed;

		bool _editingTitle = false;
		bool _editingBody = false;

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

		public Brush BorderBrush
		{
			get
			{
				return new SolidColorBrush(Colors.Red);
			}
		}

		public double TitleHeight { get { return Constants.noteCaptionHeight + CaptionPadding.GetHorizon(); } }

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

		public Visibility TitleCaptionVisibility
		{
			get { return this._titleEditVisibility == Visibility.Visible ? Visibility.Collapsed: Visibility.Visible ; }
		}
		public Visibility TitleEditVisibility
		{
			get { return this._titleEditVisibility; }
			set
			{
				if(SetVariableValue(ref this._titleEditVisibility, value)) {
					OnPropertyChanged("TitleCaptionVisibility");
				}
			}
		}

		public string Name
		{
			get { return Model.Name; }
			set { SetModelValue(value); }
		}

		public bool IsLocked
		{
			get { return Model.IsLocked; }
			set
			{
				if(SetModelValue(value)) {
					OnPropertyChanged("CaptionButtonVisibility");
					OnPropertyChanged("IsBodyReadOnly");
				}
			}
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

		public bool IsBodyReadOnly
		{
			get
			{
				if(IsLocked) {
					return !this._editingBody;
				}

				return IsLocked;
			}
		}

		public string Body
		{
			get
			{
				if(IndexBody == null) {
					this._indexBody = AppSender.SendLoadIndexBody(Library.PeData.Define.IndexKind.Note, Model.Id);
					if(this._indexBody == null) {
						this._indexBody = new NoteBodyItemModel();
					}
				}
				var indexBody = IndexBody;
				return indexBody.Text ?? string.Empty;
			}
			set
			{
				if (IsTemporary) {
					return;
				}
				if(IndexBody == null) {
					this._indexBody = new NoteBodyItemModel();
				}
				var indexBody = IndexBody;
				if(SetPropertyValue(indexBody, value, "Text")) {
					indexBody.History.Update();
					AppSender.SendSaveIndexBody(IndexBody, Model.Id);
				}
			}
		}

		#endregion

		#region HavingViewSingleModelWrapperViewModelBase

		protected override void InitializeView()
		{
			SetCompactArea();
			OnPropertyChanged("IsBodyReadOnly");

			View.UserClosing += View_UserClosing;
			
			base.InitializeView();
		}

		protected override void UninitializeView()
		{
			View.UserClosing -= View_UserClosing;

			base.UninitializeView();
		}

		#endregion

		#region IColorPair

		public Color ForeColor
		{
			get { return ColorPairProperty.GetNoneAlphaForeColor(Model); }
			set { ColorPairProperty.SetNoneAlphaForekColor(Model, value, OnPropertyChanged); }
		}

		public Color BackColor
		{
			get { return ColorPairProperty.GetNoneAlphaBackColor(Model); }
			set { ColorPairProperty.SetNoneAlphaBackColor(Model, value, OnPropertyChanged); }
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

		public bool UsingCaptionHitTest
		{
			get
			{
				if(this._editingTitle) {
					return false;
				}

				return !IsLocked;
			}
		}

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
					View.caption.ActualWidth,
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

		#region INoteMenuItem

		public ImageSource MenuImage { get { return null; } }
		public override string DisplayText { get { return DisplayTextUtility.GetDisplayName(Model); } }

		public ICommand NoteMenuSelectedCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						if(HasView) {
							View.Activate();
						}
					}
				);

				return result;
			}
		}

		#endregion

		#region command

		public ICommand SaveIndexCommnad
		{
			get
			{
				var result = CreateCommand(
					o => {
						EndEditTitle();
						EndEditBody();

						if (IsTemporary) {
							return;
						}

						if(IsChanged) {
							Model.History.Update();
							AppSender.SendSaveIndex(IndexKind.Note);
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

		//public ICommand SwitchCompactCommand
		//{
		//	get
		//	{
		//		var result = CreateCommand(
		//			o => {
		//				IsCompacted = !IsCompacted;
		//			}
		//		);

		//		return result;
		//	}
		//}

		//public ICommand SwitchTopMostCommand
		//{
		//	get
		//	{
		//		var result = CreateCommand(
		//			o => {
		//				TopMost = !TopMost;
		//			}
		//		);

		//		return result;
		//	}
		//}

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

		public ICommand RemoveCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						if (HasView) {
							View.Close();
						}
						AppSender.SendRemoveIndex(IndexKind.Note, Model.Id);
					}
				);

				return result;
			}
		}

		public ICommand CopyBodyCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						if (string.IsNullOrEmpty(Body)) {
							NonProcess.Logger.Information("empty body");
							return;
						}

						ClipboardUtility.CopyText(Body, ClipboardWatcher);
					}
				);

				return result;
			}
		}

		public ICommand EditTitleCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						TitleEditVisibility = Visibility.Visible;
						this._editingTitle = true;
						if(HasView) {
							View.title.SelectAll();
							View.title.Focus();
						}
					}
				);

				return result;
			}
		}

		public ICommand HideTitleEditCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						EndEditTitle();
					}
				);

				return result;
			}
		}

		public ICommand EditBodyCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						this._editingBody = true;
						OnPropertyChanged("IsBodyReadOnly");
						if(HasView) {
							if(View.body.SelectionLength == 0) {
								View.body.SelectAll();
							}
							View.body.Focus();
						}
					}
				);

				return result;
			}
		}

		public ICommand ReturnTitleCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						if(this._editingTitle) {
							EndEditTitle();
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

		void EndEditTitle()
		{
			if(this._editingTitle) {
				this._editingTitle = false;
				if(HasView) {
					Name = View.title.Text;
				}
				TitleEditVisibility = Visibility.Collapsed;
			}
		}

		void EndEditBody()
		{
			if(this._editingBody) {
				this._editingBody = false;
				OnPropertyChanged("IsBodyReadOnly");
			}
		}

		#endregion

		private void View_UserClosing(object sender, CancelEventArgs e)
		{
			Visible = false;
			if (HasView) {
				AppSender.SendRemoveWindow(View);
			}
		}

	}
}
