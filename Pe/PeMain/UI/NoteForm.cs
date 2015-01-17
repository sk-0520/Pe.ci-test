namespace ContentTypeTextNet.Pe.PeMain.UI
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Drawing;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;
	using ContentTypeTextNet.Pe.Library.Skin;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using ContentTypeTextNet.Pe.PeMain.Logic.DB;

	/// <summary>
	/// ノート。
	/// </summary>
	public partial class NoteForm : Form, ISetCommonData
	{
		#region define
		const int RECURSIVE = 2;
		static readonly Size menuIconSize = IconScale.Small.ToSize();

		private class NoteBindItem: INotifyPropertyChanged
		{
			public event PropertyChangedEventHandler PropertyChanged;

			public NoteBindItem(NoteItem item)
			{
				NoteItem = item;
			}

			public NoteItem NoteItem { get; private set; }

			public string Title
			{
				get
				{
					return NoteItem.Title;
				}
				set
				{
					NoteItem.Title = value;
					NotifyPropertyChanged("Title");
				}
			}

			public string Body
			{
				get
				{
					return NoteItem.Body;
				}
				set
				{
					NoteItem.Body = value;
					NotifyPropertyChanged("Body");
				}
			}

			protected void NotifyPropertyChanged(String s)
			{
				if(PropertyChanged != null) {
					PropertyChanged(this, new PropertyChangedEventArgs(s));
				}
			}
		}

		class ColorMenuItem
		{
			public ColorMenuItem(ToolStripItem item, Color color)
			{
				Item = item;
				Color = color;
			}

			public ToolStripItem Item { get; private set; }
			public Color Color { get; private set; }
		}

		struct HitState
		{
			private const uint leftBit = 0x0001;
			private const uint rightBit = 0x0002;
			private const uint topBit = 0x0004;
			private const uint bottomBit = 0x0008;

			private uint _flag;

			public bool HasTrue { get { return this._flag != 0; } }

			private bool Get(uint bit)
			{
				return (this._flag & bit) == bit;
			}
			private void Set(uint bit, bool value)
			{
				if(value) {
					this._flag |= bit;
				} else {
					this._flag &= ~(this._flag & bit);
				}
			}

			public bool Left
			{
				get { return Get(leftBit); }
				set { Set(leftBit, value); }
			}
			public bool Right
			{
				get { return Get(rightBit); }
				set { Set(rightBit, value); }
			}
			public bool Top
			{
				get { return Get(topBit); }
				set { Set(topBit, value); }
			}
			public bool Bottom
			{
				get { return Get(bottomBit); }
				set { Set(bottomBit, value); }
			}
		}
		#endregion ////////////////////////////////////

		#region static
		#endregion ////////////////////////////////////

		#region variable
		Dictionary<SkinNoteCommand, SkinButtonState> _commandStateMap;
		NoteBindItem _bindItem;
		bool _initialized = true;
		bool _changed = false;
		string _prevTitle;
		//string _prevBody;
		//Color _prevForeColor;
		//Color _prevBackColor;
		#endregion ////////////////////////////////////

		public NoteForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			Initialize();
		}

		#region property
		CommonData CommonData { get; set; }

		/// <summary>
		/// データそのものが削除された際に真。
		/// </summary>
		public bool Removed { get; private set; }

		public NoteItem NoteItem
		{
			get
			{
				return this._bindItem.NoteItem;
			}
			set
			{
				this._bindItem = new NoteBindItem(value);
			}
		}
		bool Changed
		{
			get
			{
				return this._changed;
			}
			set
			{
				if(this._initialized) {
					this._changed = value;
				}
			}
		}
		#endregion ////////////////////////////////////

		#region ISetCommonData
		public void SetCommonData(CommonData commonData)
		{
			this._initialized = false;

			CommonData = commonData;

			ApplySetting();

			this._changed = false;
			this._initialized = true;
		}
		#endregion ////////////////////////////////////

		#region override
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ExStyle |= (int)WS_EX.WS_EX_TOOLWINDOW;
				createParams.ClassStyle |= (int)CS.CS_DROPSHADOW;
				return createParams;
			}
		}

		protected override bool ShowWithoutActivation
		{
			get { return true; }
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			//base.OnPaintBackground(pevent);
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			Invalidate();
		}

		protected override void WndProc(ref Message m)
		{
			switch(m.Msg) {
				case (int)WM.WM_SYSCOMMAND: {
						switch(m.WParam.ToInt32() & 0xfff0) {
							case (int)SC.SC_MAXIMIZE:
								// #115
								if(!NoteItem.Locked) {
									ShowInputTitleArea(RECURSIVE);
								}
								return;
							case (int)SC.SC_MINIMIZE:
							case (int)SC.SC_RESTORE:
								return;
							default:
								break;
						}
						break;
					}

				case (int)WM.WM_NCPAINT: {
						//if(CommonData != null && (!this.inputBody.Visible || !this.inputTitle.Visible)) {
						if(CommonData != null) {
							var hDC = NativeMethods.GetWindowDC(Handle);
							try {
								using(var g = Graphics.FromHdc(hDC)) {
									DrawNoClient(g, new Rectangle(Point.Empty, Size), this == Form.ActiveForm);
								}
							} finally {
								NativeMethods.ReleaseDC(Handle, hDC);
							}
						}
					}
					break;

				case (int)WM.WM_NCHITTEST: {
						if(!NoteItem.Locked) {
							var point = PointToClient(WindowsUtility.ScreenPointFromLParam(m.LParam));
							var hitTest = HT.HTNOWHERE;

							var edgePadding = CommonData.Skin.GetNoteWindowEdgePadding();

							var noteArea = new Rectangle(Point.Empty, Size);
							Rectangle area;
							var pos = new HitState();
							// 上
							area = noteArea;
							area.Height = edgePadding.Top;
							pos.Top = area.Contains(point);
							// 下
							area = noteArea;
							area.Y = noteArea.Height - edgePadding.Bottom;
							area.Height = edgePadding.Bottom;
							pos.Bottom = area.Contains(point);
							// 左
							area = noteArea;
							area.Width = edgePadding.Left;
							pos.Left = area.Contains(point);
							// 右
							area = noteArea;
							area.X = noteArea.Width - edgePadding.Right;
							area.Width = edgePadding.Right;
							pos.Right = area.Contains(point);

							if(pos.HasTrue && !NoteItem.Compact) {
								if(pos.Left) {
									if(pos.Top) {
										hitTest = HT.HTTOPLEFT;
									} else if(pos.Bottom) {
										hitTest = HT.HTBOTTOMLEFT;
									} else {
										hitTest = HT.HTLEFT;
									}
								} else if(pos.Right) {
									if(pos.Top) {
										hitTest = HT.HTTOPRIGHT;
									} else if(pos.Bottom) {
										hitTest = HT.HTBOTTOMRIGHT;
									} else {
										hitTest = HT.HTRIGHT;
									}
								} else if(pos.Top) {
									hitTest = HT.HTTOP;
								} else if(pos.Bottom) {
									hitTest = HT.HTBOTTOM;
								}
							} else {
								var throwHittest = true;
								DrawCommand(
									point,
									(isIn, nowState) => {
										if(isIn) {
											throwHittest = false;
											if(nowState == SkinButtonState.Pressed) {
												return SkinButtonState.Pressed;
											} else {
												return SkinButtonState.Selected;
											}
										} else {
											return SkinButtonState.Normal;
										}
									},
									null,
									() => {
										if(throwHittest) {
											hitTest = HT.HTCAPTION;
										}
									},
									true
								);
							}

							if(hitTest != HT.HTNOWHERE) {
								m.Result = (IntPtr)hitTest;
								return;
							}
						}
					}
					break;

				case (int)WM.WM_SETCURSOR: {
						var hittest = WindowsUtility.HTFromLParam(m.LParam);
						if(hittest == HT.HTCAPTION) {
							NativeMethods.SetCursor(NativeMethods.LoadCursor(IntPtr.Zero, IDC.IDC_SIZEALL));
							return;
						} else {

						}
					}
					break;

				case (int)WM.WM_NCLBUTTONDOWN: {
						if(!NoteItem.Locked) {
							if(this.inputTitle.Visible) {
								HiddenInputTitleArea();
							}
							/*
							if(this.inputBody.Visible) {
								HiddenInputBodyArea();
							}
							 * */
						}
						this.inputBody.Focus();
					}
					break;

				case (int)WM.WM_NCRBUTTONUP: {
						if(!NoteItem.Locked) {
							switch(m.WParam.ToInt32()) {
								case (int)HT.HTCAPTION:
									var point = PointToClient(WindowsUtility.ScreenPointFromLParam(m.LParam));
									ShowContextMenu(point);
									break;
								default:
									break;
							}
						}
					}
					break;

				case (int)WM.WM_ENTERSIZEMOVE:
					{
						Opacity = Literal.noteMoveSizeOpacity;
					}
					break;

				case (int)WM.WM_EXITSIZEMOVE:
					{
						Opacity = Literal.noteNormalOpacity;
					}
					break;

				default:
					break;
			}

			base.WndProc(ref m);
		}

		[System.Security.Permissions.UIPermission(
			System.Security.Permissions.SecurityAction.Demand,
			Window = System.Security.Permissions.UIPermissionWindow.AllWindows
		)]
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if(this.inputTitle.Focused) {
				var key = keyData & Keys.KeyCode;
				switch(key) {
					case Keys.Enter: {
							HiddenInputTitleArea();
							return true;
						}

					case Keys.Escape: {
							_bindItem.Title = this._prevTitle;
							HiddenInputTitleArea();
							return true;
						}

					default:
						break;
				}
			}
			if(this.inputBody.Focused) {
				var key = keyData & Keys.KeyCode;
				if(key == Keys.Escape) {
					//_bindItem.Body = this._prevBody;
					//HiddenInputBodyArea();
					return true;
				}
			}

			return base.ProcessDialogKey(keyData);
		}

		#endregion ////////////////////////////////////

		#region initialize
		void InitializeUI()
		{
			/*
			var colorControls = new [] {
				new { Control = contextMenu_itemForeColor, Title = "note/style/color-fore", Default = Literal.noteFore },
				new { Control = contextMenu_itemBackColor, Title = "note/style/color-back", Default = Literal.noteBack },
			};
			foreach(var control in colorControls) {
				var isFore = control.Control == contextMenu_itemForeColor;
				var colorList = new [] {
					new ColorDisplayValue(isFore ? Literal.noteForeColorBlack:  Literal.noteBackColorBlack , control.Title, "note/style/color/black"),
					new ColorDisplayValue(isFore ? Literal.noteForeColorWhite:  Literal.noteBackColorWhite , control.Title, "note/style/color/white"),
					new ColorDisplayValue(isFore ? Literal.noteForeColorRed:    Literal.noteBackColorRed   , control.Title, "note/style/color/red"),
					new ColorDisplayValue(isFore ? Literal.noteForeColorGreen:  Literal.noteBackColorGreen , control.Title, "note/style/color/green"),
					new ColorDisplayValue(isFore ? Literal.noteForeColorBlue:   Literal.noteBackColorBlue  , control.Title, "note/style/color/blue"),
					new ColorDisplayValue(isFore ? Literal.noteForeColorYellow: Literal.noteBackColorYellow, control.Title, "note/style/color/yellow"),
					new ColorDisplayValue(isFore ? Literal.noteForeColorOrange: Literal.noteBackColorOrange, control.Title, "note/style/color/orange"),
					new ColorDisplayValue(isFore ? Literal.noteForeColorPurple: Literal.noteBackColorPurple, control.Title, "note/style/color/purple"),
					new ColorDisplayValue(Color.Transparent, control.Title, "note/style/color/custom"),
				};
				control.Control.ComboBox.BindingContext = BindingContext;
				control.Control.Attachment(colorList, control.Default);
			}
			*/

			ToolStripUtility.AttachmentOpeningMenuInScreen(this);
		}

		void Initialize()
		{
			this._commandStateMap = new Dictionary<SkinNoteCommand, SkinButtonState>() {
				{ SkinNoteCommand.Close, SkinButtonState.Normal },
				{ SkinNoteCommand.Compact, SkinButtonState.Normal },
				{ SkinNoteCommand.Topmost, SkinButtonState.Normal },
			};

			InitializeUI();
		}
		#endregion ////////////////////////////////////

		#region language
		void ApplyLanguageMenuItems(ToolStripItemCollection itemCollection)
		{
			if(itemCollection == null || itemCollection.Count == 0) {
				return;
			}

			foreach(ToolStripItem item in itemCollection) {
				var menuItem = item as ToolStripMenuItem;
				if(menuItem != null) {
					ApplyLanguageMenuItems(menuItem.DropDownItems);
				}
				item.SetLanguage(CommonData.Language);
			}
		}

		void ApplyLanguage()
		{
			ApplyLanguageMenuItems(this.contextMenu.Items);

			/*
			foreach(var combo in new [] { this.contextMenu_itemForeColor, this.contextMenu_itemBackColor } ) {
				foreach(var item in (IEnumerable<ColorDisplayValue>)combo.ComboBox.DataSource) {
					item.SetLanguage(CommonData.Language);
				}
			}
			*/
		}
		#endregion ////////////////////////////////////

		#region skin
		void ApplySkin()
		{
			this.contextMenu_itemTitle.Image = CommonData.Skin.GetImage(SkinImage.NoteTitle);
			this.contextMenu_itemBody.Image = CommonData.Skin.GetImage(SkinImage.NoteBody);
			this.contextMenu_itemCopy.Image = CommonData.Skin.GetImage(SkinImage.ClipboardCopy);
			this.contextMenu_font.Image = CommonData.Skin.GetImage(SkinImage.Font);
			this.contextMenu_font_change.Image = CommonData.Skin.GetImage(SkinImage.FontStyle);
			this.contextMenu_font_reset.Image = CommonData.Skin.GetImage(SkinImage.Reset);
			this.contextMenu_itemLock.Image = CommonData.Skin.GetImage(SkinImage.Lock);
			this.contextMenu_itemTopmost.Image = CommonData.Skin.GetImage(SkinImage.Pin);
			this.contextMenu_itemHidden.Image = CommonData.Skin.GetImage(SkinImage.Close);
			this.contextMenu_itemRemove.Image = CommonData.Skin.GetImage(SkinImage.Remove);
			this.contextMenu_itemExport.Image = CommonData.Skin.GetImage(SkinImage.Save);
			this.contextMenu_itemImport.Image = CommonData.Skin.GetImage(SkinImage.OpenDir);

			var colorItems = new[] {
				new { Menu = contextMenu_itemForeColor, Default = Literal.noteFore, IsFore = true, },
				new { Menu = contextMenu_itemBackColor, Default = Literal.noteBack, IsFore = false, },
			};

			foreach(var colorItem in colorItems) {
				var colors = colorItem.IsFore ? Literal.GetNoteForeColorList() : Literal.GetNoteBackColorList();
				var pairs = GetColorMenuList(colorItem.Menu, colors);
				foreach(var pair in pairs) {
					pair.Item.Image = CommonData.Skin.CreateNoteBoxImage(pair.Color, menuIconSize);
				}
			}
		}
		#endregion ////////////////////////////////////

		#region function
		void ApplySetting()
		{
			/*
			this.inputTitle.Text = NoteItem.Title;
			this.inputBody.Text = NoteItem.Body;
			 */
			this.inputTitle.DataBindings.Add("Text", this._bindItem, "Title", false, DataSourceUpdateMode.OnPropertyChanged);
			this.DataBindings.Add("Text", this._bindItem, "Title", false, DataSourceUpdateMode.Never);
			//this.inputBody.DataBindings.Add("Text", this._bindItem, "Body", false, DataSourceUpdateMode.OnPropertyChanged);

			var bindBody = new Binding("Text", this._bindItem, "Body", false, DataSourceUpdateMode.OnPropertyChanged);
			bindBody.ControlUpdateMode = ControlUpdateMode.Never;
			this.inputBody.DataBindings.Add(bindBody);
			this.inputBody.Text = this._bindItem.Body;

			Location = NoteItem.Location;
			Size = NoteItem.Size;

			TopMost = NoteItem.Topmost;

			// 最小サイズ
			var parentArea = CommonData.Skin.GetNoteCaptionArea(Size);
			var edge = CommonData.Skin.GetNoteWindowEdgePadding();
			var commandSize = CommonData.Skin.GetNoteCommandArea(parentArea, GetCommandList().First());
			var minSize = new Size(edge.Horizontal + commandSize.Width, edge.Vertical + commandSize.Height);
			MinimumSize = minSize;

			//NoteItem.Style.ForeColor;
			//			if(!this.contextMenu_fore.ComboBox.Items.Cast<ColorDisplayValue>().Any(cd => cd.Value == NoteItem.Style.ForeColor)) {
			//				this.contextMenu_fore.SelectedItem = this.contextMenu_fore.ComboBox.Items.Cast<ColorDisplayValue>().Single(cd => cd.Value == Color.Transparent).Value;
			//			}
			//NoteItem.Style.BackColor;
			ApplyBodyStyle();
			ApplySkin();
			ApplyLanguage();

			this.inputBody.ReadOnly = NoteItem.Locked;

			this.inputBody.Focus();
		}

		IEnumerable<SkinNoteCommand> GetCommandList()
		{
			return new[] {
				SkinNoteCommand.Topmost,
				SkinNoteCommand.Compact,
				SkinNoteCommand.Close,
			};
		}

		SkinNoteStatus GetNoteStatus()
		{
			var status = new SkinNoteStatus();

			status.Compact = NoteItem.Compact;
			status.Topmost = NoteItem.Topmost;
			status.Locked = NoteItem.Locked;

			return status;
		}

		public void ToClose(bool removeData)
		{
			ExecCommand(SkinNoteCommand.Close, removeData);
		}

		public void ToCompact()
		{
			ExecCommand(SkinNoteCommand.Compact, false);
		}

		public void ToTopmost()
		{
			ExecCommand(SkinNoteCommand.Topmost, false);
		}

		public void ToLock()
		{
			ExecCommand(SkinNoteCommand.Lock, false);
		}

		void ExecCommand(SkinNoteCommand noteCommand, bool removeData)
		{
			switch(noteCommand) {
				case SkinNoteCommand.Topmost: {
						NoteItem.Topmost = !NoteItem.Topmost;
						TopMost = NoteItem.Topmost;
						Changed = true;
						Refresh();
					}
					break;

				case SkinNoteCommand.Compact: {
						NoteItem.Compact = !NoteItem.Compact;
						Changed = true;

						ChangeCompact(NoteItem.Compact, NoteItem.Size);
					}
					break;

				case SkinNoteCommand.Close: {
						if(removeData) {
							// TODO: 論理削除
							Removed = true;
							RemoveItem();
						} else {
							NoteItem.Visible = false;
							Changed = true;
							SaveItem();
						}
						Close();
					}
					break;

				case SkinNoteCommand.Lock: {
						HiddenInputTitleArea();
						//HiddenInputBodyArea();
						NoteItem.Locked = !NoteItem.Locked;
						this.inputBody.ReadOnly = NoteItem.Locked;

						Changed = true;
						Refresh();
					}
					break;

				default:
					Debug.Assert(false, noteCommand.ToString());
					break;
			}
		}

		void ChangeCompact(bool compact, Size size)
		{
			if(compact) {
				var edge = this.CommonData.Skin.GetNoteWindowEdgePadding();
				var titleArea = GetTitleArea();
				Size = new Size(titleArea.Width + edge.Horizontal, titleArea.Height + edge.Vertical);
			} else {
				Size = size;
			}
		}

		Rectangle GetTitleArea()
		{
			return this.CommonData.Skin.GetNoteCaptionArea(Size);
		}

		Rectangle GetBodyArea()
		{
			return GetBodyArea(
				this.CommonData.Skin.GetNoteWindowEdgePadding(),
				this.CommonData.Skin.GetNoteCaptionArea(Size)
			);
		}
		Rectangle GetBodyArea(Padding edge, Rectangle captionArea)
		{
			return new Rectangle(
				new Point(edge.Left, captionArea.Bottom),
				new Size(Size.Width - edge.Horizontal, Size.Height - (edge.Vertical + captionArea.Height))
			);
		}

		void ResizeInputTitleArea()
		{
			var titleArea = GetTitleArea();
			this.inputTitle.Location = titleArea.Location;
			this.inputTitle.Size = titleArea.Size;
		}

		void ResizeInputBodyArea()
		{
			var bodyArea = GetBodyArea();
			this.inputBody.Location = bodyArea.Location;
			this.inputBody.Size = bodyArea.Size;
		}

		void ShowInputTitleArea(int recursive)
		{
			this._prevTitle = NoteItem.Title;
			//this.inputTitle.Text = NoteItem.Title;
			this.inputTitle.Font = CommonData.MainSetting.Note.CaptionFontSetting.Font;

			if(!this.inputTitle.Visible) {
				ResizeInputTitleArea();
				this.inputTitle.Visible = true;
				this.inputTitle.Focus();
			}
			if(!this.inputTitle.Visible && recursive > 0) {
				ShowInputTitleArea(recursive - 1);
			}
		}

		//void ShowInputBodyArea(int recursive)
		//{
		//	this._prevBody = NoteItem.Body;
		//	/*
		//	//this.inputBody.Text = NoteItem.Body;
		//	this.inputBody.Font = NoteItem.Style.FontSetting.Font;

		//	if(!this.inputBody.Visible) {
		//		ResizeInputBodyArea();
		//		this.inputBody.Visible = true;
		//		this.inputBody.Focus();
		//	}
		//	if(!this.inputBody.Visible && recursive > 0) {
		//		ShowInputBodyArea(recursive - 1);
		//	}
		//	*/
		//	if(inputBody.ReadOnly) {
		//		inputBody.ContextMenuStrip = null;
		//		inputBody.ReadOnly = false;
		//		inputBody.Cursor = Cursors.IBeam;
		//	}
		//	inputBody.Focus();
		//}

		void HiddenInputTitleArea()
		{
			if(!this.inputTitle.Visible) {
				return;
			}
			/*
			var value = this.inputTitle.Text.Trim();
			if(value.Length == 0 && NoteItem.Body.Length > 0) {
				value = TextUtility.SplitLines(NoteItem.Body).First().Trim();
			}
			var change = NoteItem.Title != value;
			if(change) {
				NoteItem.Title = value;
				this._changed |= true;
			}
			 */
			this._changed = true;
			this.inputTitle.Visible = false;
		}

		//void HiddenInputBodyArea()
		//{
		//	//inputBody.InvalidateEx();
		//	inputBody.ContextMenuStrip = this.contextMenu;
		//	inputBody.ReadOnly = true;
		//	inputBody.Cursor = Cursors.Default;

		//	if(!this.inputBody.Visible) {
		//		return;
		//	}
		//	/*
		//	var value = this.inputBody.Text.Trim();
		//	var change = NoteItem.Body != value;
		//	if(change) {
		//		NoteItem.Body = value;
		//		this._changed |= true;
		//	}
		//	if(value.Length > 0 && NoteItem.Title.Trim().Length == 0) {
		//		NoteItem.Title = TextUtility.SplitLines(value).First().Trim();
		//	}
		//	 */

		//	this._changed = true;
		//	//this.inputBody.Visible = false;
		//}

		void ShowContextMenu(Point point)
		{
			this.contextMenu.Show(this, point);
		}

		public void SaveItem()
		{
			if(this._changed) {
				lock(CommonData.Database) {
					var noteDB = new NoteDB(CommonData.Database);
					using(var tran = noteDB.BeginTransaction()) {
						try {
							noteDB.Resist(new[] { NoteItem });
							tran.Commit();
						} catch(Exception ex) {
							tran.Rollback();
							CommonData.Logger.Puts(LogType.Error, ex.Message, ex);
						}
					}
				}

				this._changed = false;
#if DEBUG
				var map = new Dictionary<string, string>() {
					{ AppLanguageName.noteTitle, NoteItem.Title },
				};
				CommonData.Logger.Puts(LogType.Information, CommonData.Language["note/save", map], NoteItem);
#endif
			}
		}

		void RemoveItem()
		{
			lock(CommonData.Database) {
				var noteDB = new NoteDB(CommonData.Database);
				using(var tran = noteDB.BeginTransaction()) {
					try {
						noteDB.ToDisabled(new[] { NoteItem });
						tran.Commit();
					} catch(Exception ex) {
						tran.Rollback();
						CommonData.Logger.Puts(LogType.Error, ex.Message, ex);
					}
				}
			}
		}

		/*
		Color GetSelectedColor(ToolStripComboBox control)
		{
			var index = control.ComboBox.SelectedIndex;
			Debug.Assert(index >= 0, control.ComboBox.SelectedIndex.ToString());
			var item = control.ComboBox.Items[index] as ColorDisplayValue;
			return item.Value;
		}
		
		bool IsCustomColor(Color color)
		{
			return color == Color.Transparent;
		}
		
		Color SetAcceptColor(Color color, Color revertColor)
		{
			var resultColor = color;
			var accept = false;
			if(IsCustomColor(color)) {
				using(var dialog = new ColorDialog()) {
					if(dialog.ShowDialog() == DialogResult.OK) {
						resultColor = dialog.Color;
						accept = true;
					} else {
						resultColor = revertColor;
					}
				}
			} else {
				accept = true;
			}
			if(accept) {
				Changed = true;
			}
			return resultColor;
		}
		 */

		IList<ColorMenuItem> GetColorMenuList(ToolStripMenuItem parentItem, IList<Color> colorList)
		{
			return colorList
				.Zip(
					parentItem.DropDownItems
					.Cast<ToolStripItem>()
					.Where(i => i is ToolStripMenuItem)
					.Take(colorList.Count),
					(color, item) => new ColorMenuItem(item, color)
				)
				.ToList()
				;
		}

		Color SelectedPlainColor(ToolStripItem selectItem, IList<ColorMenuItem> colorItemList)
		{
			return colorItemList.Single(c => c.Item == selectItem).Color;
		}

		Color SelectedCustomColor(Color nowColor)
		{
			var resultColor = nowColor;
			using(var dialog = new ColorDialog()) {
				dialog.CustomColors[0] = nowColor.ToArgb();
				if(dialog.ShowDialog() == DialogResult.OK) {
					resultColor = dialog.Color;
				}
			}

			return resultColor;
		}

		void ApplyBodyStyle()
		{
			this.inputBody.Font = NoteItem.Style.FontSetting.Font;
			this.inputBody.ForeColor = NoteItem.Style.ForeColor;
		}

		#endregion ////////////////////////////////////

		#region draw
		void DrawEdge(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus)
		{
			CommonData.Skin.DrawNoteWindowEdge(g, drawArea, active, noteStatus, NoteItem.Style.ForeColor, NoteItem.Style.BackColor);
		}
		
		void DrawCaption(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus)
		{
			var buttonState = SkinButtonState.Normal;
			
			var title = NoteItem.Title;
			#if DEBUG
			title = string.Format("(DEBUG) {0}", title);
			#endif
			CommonData.Skin.DrawNoteCaption(g, drawArea, active, noteStatus, NoteItem.Style.ForeColor, NoteItem.Style.BackColor, CommonData.MainSetting.Note.CaptionFontSetting.Font, title);
			foreach(var command in GetCommandList()) {
				var commandArea = CommonData.Skin.GetNoteCommandArea(drawArea, command);
				CommonData.Skin.DrawNoteCommand(g, commandArea, active, noteStatus, NoteItem.Style.ForeColor, NoteItem.Style.BackColor, command, buttonState);
			}
		}

		void DrawNoClient(Graphics g, Rectangle drawArea, bool active)
		{
			var noteStatus = GetNoteStatus();
			//if(!CommonData.Skin.IsDefaultDrawToolbarWindowBackground) {
				CommonData.Skin.DrawNoteWindowBackground(g, drawArea, active, noteStatus, NoteItem.Style.BackColor);
			//}
			
			var captionArea = CommonData.Skin.GetNoteCaptionArea(Size);
			if(!captionArea.Size.IsEmpty) {
				DrawCaption(g, captionArea, active, noteStatus);
			}
			var bodyArea = GetBodyArea();
			DrawBody(g, bodyArea, active, noteStatus);
			DrawEdge(g, drawArea, active, noteStatus);
		}
		
		void DrawBody(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus)
		{
			if(!noteStatus.Compact) {
				CommonData.Skin.DrawNoteBody(g, drawArea, active, noteStatus, NoteItem.Style.ForeColor, NoteItem.Style.BackColor);
			}
		}
		
		void DrawFull(Graphics g, Rectangle drawArea, bool active)
		{
			DrawNoClient(g, drawArea, active);
		}
		void DrawFullActivaChanged(bool active)
		{
			using(var g = CreateGraphics()) {
				using(var bmp = new Bitmap(Width, Height, g)) {
					using(var memG = Graphics.FromImage(bmp)) {
						var rect = new Rectangle(Point.Empty, Size);
						DrawFull(memG, rect, active);
						g.DrawImage(bmp, 0, 0);
					}
				}
			}
		}
		
		void DrawCommand(Point point, Func<bool, SkinButtonState, SkinButtonState> inFirstDg, Action<SkinNoteCommand> prevDrawDg, Action lastInDg, bool elseProcess)
		{
			var captionArea = CommonData.Skin.GetNoteCaptionArea(Size);
			if(!captionArea.Size.IsEmpty) {
				var active = this == Form.ActiveForm;
				var noteStatus = GetNoteStatus();
				if(captionArea.Contains(point)) {
					using(var g = CreateGraphics()) {
						foreach(var command in GetCommandList()) {
							var commandArea = CommonData.Skin.GetNoteCommandArea(captionArea, command);
							var nowState = SkinButtonState.None;
							var prevState = this._commandStateMap[command];
							nowState = inFirstDg(commandArea.Contains(point), prevState);
							
							if(nowState != SkinButtonState.None) {
								if(prevDrawDg != null) {
									prevDrawDg(command);
								}
								if(nowState != prevState && this.Created) {
									CommonData.Skin.DrawNoteCommand(g, commandArea, active, noteStatus, NoteItem.Style.ForeColor, NoteItem.Style.BackColor, command, nowState);
									this._commandStateMap[command] = nowState;
								}
							}
						}
					}
					if(lastInDg != null) {
						lastInDg();
					}
				} else {
					if(elseProcess) {
						using(var g = CreateGraphics()) {
							foreach(var pair in this._commandStateMap) {
								if(pair.Value != SkinButtonState.Normal) {
									var commandArea = CommonData.Skin.GetNoteCommandArea(captionArea, pair.Key);
									CommonData.Skin.DrawNoteCommand(g, commandArea, active, noteStatus, NoteItem.Style.ForeColor, NoteItem.Style.BackColor, pair.Key, SkinButtonState.Normal);
								}
							}
							foreach(var key in this._commandStateMap.Keys.ToArray()) {
								this._commandStateMap[key] = SkinButtonState.Normal;
							}
						}
					}
				}
			}
		}
		#endregion ////////////////////////////////////

		void NoteForm_Paint(object sender, PaintEventArgs e)
		{
			using (var bmp = new Bitmap(Width, Height, e.Graphics)) {
				using (var memG = Graphics.FromImage(bmp)) {
					var rect = new Rectangle(Point.Empty, Size);
					//var rect = e.ClipRectangle;
					DrawFull(memG, rect, this == Form.ActiveForm);
					e.Graphics.DrawImage(bmp, 0, 0);
				}
			}
		}
		
		void NoteForm_Activated(object sender, EventArgs e)
		{
			DrawFullActivaChanged(true);
		}
		
		void NoteForm_Deactivate(object sender, EventArgs e)
		{
			if (this._initialized) {
				HiddenInputTitleArea();
				//HiddenInputBodyArea();
			}
			
			DrawFullActivaChanged(false);

			SaveItem();

			this.inputBody.ReadOnly = NoteItem.Locked;
			Refresh();
		}
		
		void NoteForm_MouseDown(object sender, MouseEventArgs e)
		{
			HiddenInputTitleArea();
			
			if (NoteItem.Locked) {
				return;
			}
			
			DrawCommand(
				e.Location,
				(isIn, nowState) => {
					var left = (MouseButtons & MouseButtons.Left) == MouseButtons.Left;
					if (left && isIn) {
						return SkinButtonState.Pressed;
					} else {
						return SkinButtonState.Normal;
					}
				},
				null,
				null,
				false
			);
		}
		
		void NoteForm_MouseUp(object sender, MouseEventArgs e)
		{
			if (NoteItem.Locked) {
				return;
			}
			
			DrawCommand(
				e.Location,
				(isIn, nowState) => {
					var left = (MouseButtons & MouseButtons.Left) == MouseButtons.Left;
					if (left && isIn) {
						return SkinButtonState.Selected;
					} else {
						return SkinButtonState.Normal;
					}
				},
				command => {
					if (this._commandStateMap[command] == SkinButtonState.Pressed) {
						var isRemove = AppUtility.IsExtension();
						if (isRemove) {
							var map = new Dictionary<string, string>() {
								{ AppLanguageName.noteTitle, NoteItem.Title },
							};
							var result = MessageBox.Show(CommonData.Language["note/dialog/message", map], CommonData.Language["note/dialog/caption"], MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
							if (result == DialogResult.Cancel) {
								return;
							}
							isRemove = result == DialogResult.Yes;
						}
						ExecCommand(command, isRemove);
					}
				},
				null,
				true
			);
		}
		
		/*
		void NoteForm_DoubleClick(object sender, EventArgs e)
		{
			if (!NoteItem.Locked) {
				ShowInputBodyArea(RECURSIVE);
			}
		}
		*/

		void NoteForm_Resize(object sender, EventArgs e)
		{
			if (!this._initialized && NoteItem.Compact) {
				//ChangeCompact(true, Size.Empty);
				Height = 20;
			} else {
				ResizeInputTitleArea();
				ResizeInputBodyArea();
				
				if (!NoteItem.Compact) {
					NoteItem.Size = Size;
					Changed = true;
				}
			}
		}
		
		void NoteForm_Move(object sender, EventArgs e)
		{
			NoteItem.Location = Location;
			Changed = true;
		}
		
		void Input_Leave(object sender, EventArgs e)
		{
			HiddenInputTitleArea();
			//HiddenInputBodyArea();
		}
		
		void ContextMenu_title_Click(object sender, EventArgs e)
		{
			ShowInputTitleArea(RECURSIVE);
		}
		
		/*
		void ContextMenu_body_Click(object sender, EventArgs e)
		{
			ShowInputBodyArea(RECURSIVE);
		}
		*/

		void NoteForm_Load(object sender, EventArgs e)
		{
			// 生成前の高さがWindowsにより補正されるためここでリサイズ
			ChangeCompact(NoteItem.Compact, NoteItem.Size);
		}
		
		void ContextMenu_itemCopy_Click(object sender, EventArgs e)
		{
			Debug.Assert(!string.IsNullOrEmpty(NoteItem.Body));
			string copyText;
			if(this.inputBody.SelectionLength > 0) {
				copyText = this.inputBody.SelectedText;
			} else {
				copyText = NoteItem.Body;
			}
			ClipboardUtility.CopyText(copyText, CommonData);
		}
		
		void ContextMenu_font_change_Click(object sender, EventArgs e)
		{
			using(var dialog = new FontDialog()) {
				dialog.SetFontSetting(NoteItem.Style.FontSetting);
				
				if(dialog.ShowDialog() == DialogResult.OK) {
					NoteItem.Style.FontSetting.Import(dialog.Font);
				}
			}
			ApplyBodyStyle();
			Refresh();
		}
		
		void ContextMenu_font_reset_Click(object sender, EventArgs e)
		{
			if (!NoteItem.Style.FontSetting.IsDefault) {
				NoteItem.Style.FontSetting.Dispose();
				NoteItem.Style.FontSetting = new FontSetting();
			}
			ApplyBodyStyle();
			Refresh();
		}
		
		void ContextMenu_Closed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			if (Form.ActiveForm != this && Changed) {
				SaveItem();
			}
		}
		
		void ContextMenu_itemCompact_Click(object sender, EventArgs e)
		{
			ToCompact();
		}
		
		void ContextMenu_itemTopmost_Click(object sender, EventArgs e)
		{
			ToTopmost();
		}
		
		void ContextMenu_itemHidden_Click(object sender, EventArgs e)
		{
			ToClose(false);
		}
		
		void ContextMenu_itemLock_Click(object sender, EventArgs e)
		{
			ToLock();
		}
		
		void ContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// 本文入力
			this.contextMenu_itemBody.Enabled = !NoteItem.Compact;
			
			// クリップボード
			this.contextMenu_itemCopy.Enabled = !string.IsNullOrEmpty(NoteItem.Body);
			
			// 状態チェック
			var lockImage = NoteItem.Locked 
				? CommonData.Skin.GetImage(SkinImage.Lock)
				: CommonData.Skin.GetImage(SkinImage.Unlock)
			;
			this.contextMenu_itemLock.Image = lockImage;
			this.contextMenu_itemLock.Checked = NoteItem.Locked;
			this.contextMenu_itemCompact.Checked = NoteItem.Compact;
			this.contextMenu_itemTopmost.Checked = NoteItem.Topmost;
			
			// フォント
			this.contextMenu_font_change.Text = LanguageUtility.FontSettingToDisplayText(CommonData.Language, NoteItem.Style.FontSetting);
			
			// 色
			Action<ToolStripItem, IList<ColorMenuItem>, ToolStripItem, Color> checkColor = (parentItem, colorItemList, customItem, nowColor) => {
				var plainColor = false;
				
				parentItem.Image.ToDispose();
				parentItem.Image = CommonData.Skin.CreateNoteBoxImage(nowColor, menuIconSize);
				
				foreach (var colorItem in colorItemList) {
					var menuItem = colorItem.Item as ToolStripMenuItem;
					if (menuItem != null) {
						plainColor |= menuItem.Checked = colorItem.Color == nowColor;
					}
				}
				var customMenuItem = customItem as ToolStripMenuItem;
				if (customMenuItem != null) {
					customMenuItem.Checked = !plainColor;
					customMenuItem.Image.ToDispose();
					if (customMenuItem.Checked) {
						customMenuItem.Image = CommonData.Skin.CreateNoteBoxImage(nowColor, menuIconSize);
					} else {
						customMenuItem.Image = (Image)CommonData.Skin.GetImage(SkinImage.CustomColor).Clone();
					}
				}
			};
			
			var foreMenuList = GetColorMenuList(this.contextMenu_itemForeColor, Literal.GetNoteForeColorList());
			var backMenuList = GetColorMenuList(this.contextMenu_itemBackColor, Literal.GetNoteBackColorList());
			checkColor(this.contextMenu_itemForeColor, foreMenuList, this.contextMenu_itemForeColor_itemCustom, NoteItem.Style.ForeColor);
			checkColor(this.contextMenu_itemBackColor, backMenuList, this.contextMenu_itemBackColor_itemCustom, NoteItem.Style.BackColor);
			// 最小化状態
			this.contextMenu_itemCompact.ImageScaling = ToolStripItemImageScaling.None;
			this.contextMenu_itemCompact.Image.ToDispose();
			this.contextMenu_itemCompact.Image = CommonData.Skin.CreateNoteBoxImage(NoteItem.Style.BackColor, new Size(menuIconSize.Width, menuIconSize.Height / 2));
			
			// 入出力
			this.contextMenu_itemExport.Enabled = NoteItem.Body.Length > 0;

			// #120
			//// 拡張メニュー
			//var extension = AppUtility.IsExtension();
			//this.contextMenu_itemRemove.Visible = extension;
		}
		
		void NoteForm_MouseLeave(object sender, EventArgs e)
		{
			Refresh();
		}
		
		void NotemenuexportToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (var dialog = new SaveFileDialog()) {
				var filter = new DialogFilter();
				filter.Items.Add(new DialogFilterItem("*.txt", "*.txt"));
				filter.Attachment(dialog);
				if (dialog.ShowDialog() == DialogResult.OK) {
					var path = dialog.FileName;
					File.WriteAllText(path, NoteItem.Body, Encoding.UTF8);
				}
			}
		}
		
		void ContextMenu_itemForeColor_itemClick(object sender, EventArgs e)
		{
			var colorItemList = GetColorMenuList(this.contextMenu_itemForeColor, Literal.GetNoteForeColorList());
			NoteItem.Style.ForeColor = SelectedPlainColor((ToolStripItem)sender, colorItemList);
			ApplyBodyStyle();
			Refresh();
		}
		
		void ContextMenu_itemBackColor_itemClick(object sender, EventArgs e)
		{
			var colorItemList = GetColorMenuList(this.contextMenu_itemBackColor, Literal.GetNoteBackColorList());
			NoteItem.Style.BackColor = SelectedPlainColor((ToolStripItem)sender, colorItemList);
			Refresh();
		}
		
		void ContextMenu_itemForeColor_itemCustom_Click(object sender, EventArgs e)
		{
			NoteItem.Style.ForeColor = SelectedCustomColor(NoteItem.Style.ForeColor);
			ApplyBodyStyle();
			Refresh();
		}
		
		void ContextMenu_itemBackColor_itemCustom_Click(object sender, EventArgs e)
		{
			NoteItem.Style.BackColor = SelectedCustomColor(NoteItem.Style.BackColor);
			Refresh();
		}
		
		void ContextMenu_itemRemove_Click(object sender, EventArgs e)
		{
			ExecCommand(SkinNoteCommand.Close, true);
		}

		/*
		private void inputBody_DoubleClick(object sender, EventArgs e)
		{
			if(!NoteItem.Locked) {
				ShowInputBodyArea(RECURSIVE);
			}
		}
		*/
		
		private void contextMenu_Opened(object sender, EventArgs e)
		{
			Refresh();
		}

		private void contextMenu_itemBody_Click(object sender, EventArgs e)
		{
			this.inputBody.ReadOnly = false;
			this.inputBody.Focus();
		}


	}
}