namespace ContentTypeTextNet.Pe.PeMain.UI
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Data;
	using System.Drawing;
	using System.Text;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.UI.Ex;
	using System.Text.RegularExpressions;
	using ContentTypeTextNet.Pe.PeMain.Kind;
	using System.Diagnostics;
	using System.IO;
	using ContentTypeTextNet.Pe.Library.Skin;
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using System.Drawing.Text;
	using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;

	public partial class CommandForm: CommonForm
	{
		#region variable

		bool _canExecute;
		readonly IReadOnlyList<CommandDisplayValue> _emptyCommandList = new List<CommandDisplayValue>();

		#endregion

		public CommandForm()
		{
			InitializeComponent();

			Initialize();
		}

		#region property

		IReadOnlyList<LauncherItem> LauncherList { get; set; }
		bool CallUpdateEvent { get; set; }
		bool CanExecute
		{
			get { return this._canExecute; }
			set
			{
				this._canExecute = value;
				this.commandExecute.Enabled = this._canExecute;
			}
		}

		IEnumerable<string> UriPattern { get { return Literal.UriPattern; } }

		#endregion

		#region override

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);
			if(m.Msg == (int)WM.WM_NCHITTEST) {
				var hittest = (HT)m.Result.ToInt32();
				var map = new Dictionary<HT, HT>() {
						{ HT.HTTOPLEFT, HT.HTLEFT },
						{ HT.HTBOTTOMLEFT, HT.HTLEFT },
						{ HT.HTTOPRIGHT, HT.HTRIGHT },
						{ HT.HTBOTTOMRIGHT, HT.HTRIGHT },
						{ HT.HTTOP, HT.HTNOWHERE },
						{ HT.HTBOTTOM, HT.HTNOWHERE },
					};
				HT result;
				if(map.TryGetValue(hittest, out result)) {
					m.Result = new IntPtr((int)result);
				}
			}
		}

		[System.Security.Permissions.UIPermission(
			System.Security.Permissions.SecurityAction.Demand,
			Window = System.Security.Permissions.UIPermissionWindow.AllWindows
		)]
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if(this.inputCommand.Focused) {
				var key = keyData & Keys.KeyCode;
				if(key == Keys.Escape) {
					if(this.inputCommand.DroppedDown) {
						this.inputCommand.DroppedDown = false;
					} else {
						Visible = false;
					}
					return true;
				} else if(key == Keys.Enter) {
					if(CanExecute) {
						ExecuteCommand(AppUtility.IsExtension());
					}
					return true;
				} else if(key == Keys.Tab) {
					//NextCommand();
					var dv = GetCommandDisplayValue();
					if(dv != null) {
						var items = this.inputCommand.Items.Cast<CommandDisplayValue>().ToArray();
						var nextItems = items
							.Cast<CommandDisplayValue>()
							.SkipWhile(i => i != dv)
							.Skip(1)
						;
						if(!nextItems.Any()) {
							nextItems = items.SkipWhile(i => i.CommandKind == CommandKind.None || i.CommandKind == CommandKind.Uri);
						} 
						if(nextItems.Any()) {
							var nextItem = nextItems.First();
							Debug.Assert(new[] { CommandKind.FilePath, CommandKind.LauncherItem_Name, CommandKind.LauncherItem_Tag }.Any(c => c == nextItem.CommandKind));
							this.inputCommand.SelectedItem = nextItem;
							return true;
						}
					}
				}
			}

			return base.ProcessDialogKey(keyData);
		}

		#endregion

		#region CommonForm

		protected override void ApplySetting()
		{
			base.ApplySetting();

			this.inputCommand.Font = CommonData.MainSetting.Command.FontSetting.Font;
			//this.inputCommand.ItemHeight = 32;// AppUtility.GetTuneItemHeight(this.inputCommand.Margin, CommonData.MainSetting.Command.IconScale, CommonData.MainSetting.Command.FontSetting.Font);
			//this.inputCommand.Height = 16; //CommonData.MainSetting.Command.FontSetting.Font.Height;// +this.inputCommand.Margin.Vertical + 1 * 2;
			//NativeMethods.SendMessage(this.inputCommand.Handle, WM.CB_SETITEMHEIGHT, new IntPtr(0), new IntPtr(30));
			this.timerHidden.Interval = (int)CommonData.MainSetting.Command.HiddenTime.TotalMilliseconds;
			Width = CommonData.MainSetting.Command.Width;
			this.inputCommand.Location = Point.Empty;
			SetLauncherItems();
		}

		protected override void ApplyLanguage()
		{
			UIUtility.SetDefaultText(this, CommonData.Language);
			this.commandExecute.SetLanguage(CommonData.Language);
			base.ApplyLanguage();
		}

		#endregion

		#region initialize
		void Initialize()
		{
			CallUpdateEvent = true;
			CanExecute = false;
		}
		#endregion

		#region functino

		void SetLauncherItems()
		{
			//LauncherList = CommonData.MainSetting.Launcher.Items.ToList();
			//this.imputCommand
			//this.inputCommand.DataBindings.Add("Items", LauncherList, "");
			LauncherList = CommonData.MainSetting.Launcher.Items.OrderBy(i => i.Name).ToList();
			//this.inputCommand.DataSource = LauncherList;
			ChangeLauncherItems(string.Empty);
		}

		void ChangeLauncherItems(string s)
		{
			IEnumerable<CommandDisplayValue> list = null;
			if(!string.IsNullOrWhiteSpace(s)) {
				//var reg = new Regex(TextUtility.RegexPatternToWildcard(s), RegexOptions.IgnoreCase);
				
				// アイテム名
				var nameList = LauncherList
					.Where(i => i.Name.StartsWith(s))
					.Select(i => new CommandDisplayValue(i, i.Name, CommandKind.LauncherItem_Name))
				;

				// タグ名
				IEnumerable<CommandDisplayValue> tagList = null;
				if(CommonData.MainSetting.Command.EnabledFindTag) {
					tagList = LauncherList
						.SelectMany(
							(i, index) => i.Tag,
							(i, tag) => new {
								Item = i,
								Tag = tag
							}
						)
						.Where(pair => pair.Tag.StartsWith(s))
						.Select(pair => new CommandDisplayValue(pair.Item, pair.Tag, CommandKind.LauncherItem_Tag))
					;
				}
				if(tagList == null) {
					tagList = _emptyCommandList;
				}

				// ファイルパス
				IEnumerable<CommandDisplayValue> fileList = null;
				if(CommonData.MainSetting.Command.EnabledFindFile) {
					var inputPath = Environment.ExpandEnvironmentVariables(s);
					var isDir = Directory.Exists(inputPath);
					string baseDir;
					try {
						baseDir = isDir
							? inputPath.Last() == Path.VolumeSeparatorChar
								? inputPath + Path.DirectorySeparatorChar
								: inputPath
							: Path.GetDirectoryName(inputPath)
						;
					} catch(ArgumentException) {
						baseDir = inputPath;
					}

					if(FileUtility.Exists(baseDir)) {
						Debug.WriteLine(inputPath);
						//var isDir = Directory.Exists(inputPath);
						//var baseDir = isDir ? inputPath : Path.GetDirectoryName(inputPath);
						var searchPattern = isDir ? "*" : Path.GetFileName(inputPath) + "*";
						var showHiddenFile = SystemEnvironment.IsHiddenFileShow();
						var directoryInfo = new DirectoryInfo(baseDir);
						try {
							fileList = directoryInfo
								.EnumerateFileSystemInfos(searchPattern, SearchOption.TopDirectoryOnly)
								.Where(fs => fs.Exists)
								.Where(fs => showHiddenFile ? true : !fs.IsHidden())
								.Select(fs => new CommandDisplayValue(
										new LauncherItem() {
											Name = fs.Name,
											Command = fs.FullName,
											LauncherType = LauncherType.File,
										},
										fs.FullName,
										CommandKind.FilePath
									)
								);
						} catch(IOException ex) {
							CommonData.Logger.Puts(LogType.Warning, ex.Message, ex);
						} catch(UnauthorizedAccessException ex) {
							CommonData.Logger.Puts(LogType.Warning, ex.Message, ex);
						}
					}
				}
				if(fileList == null) {
					fileList = _emptyCommandList;
				}
				list = nameList.Concat(tagList).Concat(fileList);
			}
			if(list == null || !list.Any()) {
				list = LauncherList.Select(i => new CommandDisplayValue(i, i.Name, CommandKind.LauncherItem_Name));
			}

			var nowCommandValue = new CommandDisplayValue(new LauncherItem(), s, CommandKind.None);
			if(!string.IsNullOrWhiteSpace(s)) {
				nowCommandValue = list.FirstOrDefault(i => i.Display.StartsWith(s, StringComparison.OrdinalIgnoreCase)) ?? nowCommandValue;
			}
			if(nowCommandValue.CommandKind == CommandKind.None) {
				list = new[] { nowCommandValue }.Concat(list);
			}
			try {
				this.inputCommand.TextUpdate -= inputCommand_TextUpdate;

				this.inputCommand.Attachment(list, nowCommandValue.Value);

				this.inputCommand.Select(s.Length, nowCommandValue.Display.Length);
				ChangeIcon();
			} finally {
				this.inputCommand.TextUpdate += inputCommand_TextUpdate;
			}
		}

		CommandKind GetCommandKindFromText(string s)
		{
			if(string.IsNullOrWhiteSpace(s)) {
				return CommandKind.None;
			}

			if(UriPattern.Any(u => s.StartsWith(u, StringComparison.OrdinalIgnoreCase))) {
				return CommandKind.Uri;
			}

			return CommandKind.FilePath;
		}

		CommandDisplayValue GetCommandDisplayValue()
		{
			return this.inputCommand.SelectedItem as CommandDisplayValue ?? this.inputCommand.Items.Cast<CommandDisplayValue>().SingleOrDefault(i => i.Display == this.inputCommand.Text);
		}

		CommandKind GetCommandKind()
		{
			var dv = GetCommandDisplayValue();
			if(dv != null && dv.CommandKind != CommandKind.None) {
				if(dv.CommandKind == CommandKind.FilePath) {
					return CommandKind.FilePath;
				}
				return CommandKind.LauncherItem_Name;
			} else {
				return GetCommandKindFromText(this.inputCommand.Text);
			}
		}

		void ClearIcon()
		{
			var oldImage = this.imageIcon.Image;
			this.imageIcon.Image = null;
			oldImage.ToDispose();
		}

		void ChangeIcon()
		{
			ClearIcon();

			var kind = GetCommandKind();
			switch(kind) {
				case CommandKind.LauncherItem_Name:
					{
						var dv = GetCommandDisplayValue();
						if(dv != null) {
							var item = dv.Value;
							var icon = item.GetIcon(IconScale.Normal, item.IconItem.Index, CommonData.ApplicationSetting, CommonData.Logger);
							this.imageIcon.Image = IconUtility.ImageFromIcon(icon, IconScale.Normal);
							CanExecute = true;
						}
					}
					break;

				case CommandKind.FilePath: 
					{
						var path = Environment.ExpandEnvironmentVariables(this.inputCommand.Text);
						if(FileUtility.Exists(path)) {
							this.imageIcon.Image = IconUtility.GetThumbnailImage(path, IconScale.Normal);
							CanExecute = true;
						} else {
							CanExecute = false;
						}
					}
					break;

				case CommandKind.Uri:
					{
						this.imageIcon.Image = (Image)CommonData.Skin.GetImage(SkinImage.Web).Clone();
						CanExecute = true;
					}
					break;

				case CommandKind.None:
					CanExecute = false;
					break;

				default:
					throw new NotImplementedException();
			}
		}

		public void SetCurrentLocation()
		{
			Location = Cursor.Position;
		}

		void ExecuteCommand(bool executeEx)
		{
			var kind = GetCommandKind();
			switch(kind) {
				case CommandKind.LauncherItem_Name: 
					{
						var dv = GetCommandDisplayValue();
						if(dv != null) {
							var item = dv.Value;
							if(executeEx) {
								AppUtility.ShowExecuteEx(CommonData, item, null);
							} else {
								AppUtility.ExecuteItem(CommonData, item);
							}
						} else {
							CommonData.Logger.Puts(LogType.Warning, CommonData.Language["command/error/execute/launcher-item"], this.inputCommand.SelectedValue);
						}
						Visible = false;
					}
					break;

				case CommandKind.Uri:
					{
						try {
							var inputUri = this.inputCommand.Text;
							var headHead = UriPattern.Single(u => inputUri.StartsWith(u, StringComparison.OrdinalIgnoreCase));
							var uriValue = inputUri.Substring(headHead.Length);
							var uri = headHead + uriValue;
							Executor.RunCommand(uri, CommonData);
						} catch(Exception ex) {
							CommonData.Logger.Puts(LogType.Warning, ex.Message, new ExceptionMessage(this.inputCommand.Text, ex));
						}
						Visible = false;
					}
					break;

				case CommandKind.FilePath: 
					{
						var inputPath = this.inputCommand.Text;
						var expandPath = Environment.ExpandEnvironmentVariables(inputPath);
						try {
							if(FileUtility.Exists(expandPath)) {
								if(executeEx) {
									Executor.OpenDirectoryWithFileSelect(expandPath, CommonData, null);
								} else {
									if(Directory.Exists(expandPath)) {
										Executor.OpenDirectory(expandPath, CommonData, null);
									} else {
										Executor.OpenFile(expandPath, CommonData);
									}
								}
							}
						} catch(Exception ex) {
							CommonData.Logger.Puts(LogType.Warning, ex.Message, new ExceptionMessage(this.inputCommand.Text, ex));
						}
						Visible = false;
					}
					break;

				case CommandKind.None: 
					CommonData.Logger.Puts(LogType.Warning, CommonData.Language["command/error/execute/none"], this.inputCommand.Text);
					break;

				default:
					throw new NotImplementedException();
			}
		}

		#endregion

		private void CommandForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if(e.CloseReason == CloseReason.UserClosing) {
				e.Cancel = true;
				Visible = false;
			}
		}

		private void commandExecute_Click(object sender, EventArgs e)
		{
			ExecuteCommand(AppUtility.IsExtension());
		}

		private void inputCommand_TextUpdate(object sender, EventArgs e)
		{
			if(CallUpdateEvent) {
				CallUpdateEvent = false;
				try {
					ChangeLauncherItems(this.inputCommand.Text);
				} finally {
					CallUpdateEvent = true;
				}
			}
		}

		private void inputCommand_KeyDown(object sender, KeyEventArgs e)
		{
			var noUpdateKeys = new[] {
				Keys.Delete,
				Keys.Back,
			};
			var directorySeparator = new[] {
				Keys.Oem5,
				Keys.OemBackslash,
			};
			if(noUpdateKeys.Any(k => k == e.KeyCode)) {
				CallUpdateEvent = false;
			} else if(directorySeparator.Any(k => k == e.KeyCode)) {
				var kind = GetCommandKind();
				if(kind == CommandKind.FilePath) {
					if(this.inputCommand.SelectionLength > 0) {
						if(this.inputCommand.Text[this.inputCommand.SelectionStart] != Path.DirectorySeparatorChar) {
							var path = this.inputCommand.Text;
							if(File.Exists(path)) {
								e.Handled = true;
							} else {
								this.inputCommand.SelectionStart = this.inputCommand.Text.Length;
							}
						}
					}
				}
			}
		}

		private void inputCommand_KeyUp(object sender, KeyEventArgs e)
		{
			if(!CallUpdateEvent) {
				CallUpdateEvent = true;
				if(this.inputCommand.Text.Length == 0) {
					ChangeLauncherItems(string.Empty);
				} else {
					ChangeIcon();
				}
			}
		}

		private void inputCommand_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(CallUpdateEvent) {
				ChangeIcon();
			}
		}

		private void inputCommand_DropDownClosed(object sender, EventArgs e)
		{
			if(CallUpdateEvent) {
				ChangeIcon();
			}
		}

		private void CommandForm_Deactivate(object sender, EventArgs e)
		{
			this.timerHidden.Start();
		}

		private void CommandForm_Activated(object sender, EventArgs e)
		{
			this.timerHidden.Stop();
			this.inputCommand.Focus();
		}

		private void timerHidden_Tick(object sender, EventArgs e)
		{
			this.timerHidden.Stop();
			Visible = false;
		}

		private void CommandForm_VisibleChanged(object sender, EventArgs e)
		{
			if(!Visible) {
				this.inputCommand.Text = string.Empty;
				ClearIcon();
			} else {
				this.inputCommand.Focus();
			}
		}

		private void CommandForm_SizeChanged(object sender, EventArgs e)
		{
			CommonData.MainSetting.Command.Width = Width;
		}

		private void inputCommand_DrawItem(object sender, DrawItemEventArgs e)
		{
			e.DrawBackground();

			var g = e.Graphics;
			if(e.Index != -1) {
				// TODO: アイコン位置と文字列位置の補正が必要
				var item = (CommandDisplayValue)this.inputCommand.Items[e.Index];
				var icon = item.Value.GetIcon(CommonData.MainSetting.Command.IconScale, item.Value.IconItem.Index, CommonData.ApplicationSetting, new NullLogger());
				if(icon != null) {
					var padding = e.Bounds.Height / 2 - CommonData.MainSetting.Command.IconScale.ToHeight() / 2;
					g.DrawIcon(icon, e.Bounds.X + padding, e.Bounds.Y + padding);
				}
				var textArea = new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
				textArea.X += this.inputCommand.ItemHeight;
				textArea.Width -= this.inputCommand.ItemHeight;
				using(var brush = new SolidBrush(e.ForeColor)) {
					using(var format = new StringFormat()) {
						format.Alignment = StringAlignment.Near;
						format.LineAlignment = StringAlignment.Center;
						g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
						g.DrawString(item.Value.Name, CommonData.MainSetting.Command.FontSetting.Font, brush, textArea, format);
					}
				}
			}

			e.DrawFocusRectangle();
		}

		private void inputCommand_MeasureItem(object sender, MeasureItemEventArgs e)
		{
			if(e.Index == -1)
				e.ItemHeight = 10;
			else
			e.ItemHeight = this.inputCommand.ItemHeight;
		}
	}
}
