/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/03/12
 * 時刻: 4:29
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic;
using ContentTypeTextNet.Pe.Library.Skin;
using ContentTypeTextNet.Pe.Library.Utility;
using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	/// <summary>
	/// ノート。
	/// </summary>
	public partial class NoteForm : Form, ISetCommonData
	{
		public NoteForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			Initialize();
		}

		protected override CreateParams CreateParams {
			get {
				CreateParams createParams = base.CreateParams;
				createParams.ExStyle |= (int)WS_EX.WS_EX_TOOLWINDOW;
				createParams.ClassStyle |= (int)CS.CS_DROPSHADOW;
				return createParams;
			}
		}
		
		protected override bool ShowWithoutActivation {
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
				HiddenInputBodyArea();
			}
			
			
			DrawFullActivaChanged(false);
			
			SaveItem();
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
								{ "NOTE", NoteItem.Title },
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
		
		void NoteForm_DoubleClick(object sender, EventArgs e)
		{
			if (!NoteItem.Locked) {
				ShowInputBodyArea(RECURSIVE);
			}
		}
		
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
			HiddenInputBodyArea();
		}
		
		void ContextMenu_title_Click(object sender, EventArgs e)
		{
			ShowInputTitleArea(RECURSIVE);
		}
		
		void ContextMenu_body_Click(object sender, EventArgs e)
		{
			ShowInputBodyArea(RECURSIVE);
		}

		
		void NoteForm_Load(object sender, EventArgs e)
		{
			// 生成前の高さがWindowsにより補正されるためここでリサイズ
			ChangeCompact(NoteItem.Compact, NoteItem.Size);
		}
		
		void ContextMenu_itemCopy_Click(object sender, EventArgs e)
		{
			Debug.Assert(!string.IsNullOrEmpty(NoteItem.Body));
			Clipboard.SetText(NoteItem.Body);
		}
		
		void ContextMenu_font_change_Click(object sender, EventArgs e)
		{
			using (var dialog = new FontDialog()) {
				if (NoteItem.Style.FontSetting.IsDefault) {
					dialog.Font = NoteItem.Style.FontSetting.Font;
				}
				
				if (dialog.ShowDialog() == DialogResult.OK) {
					NoteItem.Style.FontSetting.Import(dialog.Font);
				}
			}
			Refresh();
		}
		
		void ContextMenu_font_reset_Click(object sender, EventArgs e)
		{
			if (!NoteItem.Style.FontSetting.IsDefault) {
				NoteItem.Style.FontSetting.Dispose();
				NoteItem.Style.FontSetting = new FontSetting();
			}
			Refresh();
		}
		/*
		void ContextMenu_fore_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(contextMenu.Created) {
				var color = GetSelectedColor(this.contextMenu_itemForeColor);
				var result = SetAcceptColor(color, this._prevForeColor);
				if(result != this._prevForeColor) {
					NoteItem.Style.ForeColor = result;
					Refresh();
				} else {
					contextMenu.Close();
				}
			}
		}
		
		void ContextMenu_back_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(contextMenu.Created) {
				var color = GetSelectedColor(this.contextMenu_itemBackColor);
				var result = SetAcceptColor(color, this._prevBackColor);
				if(result != this._prevBackColor) {
					NoteItem.Style.BackColor = result;
					Refresh();
				} else {
					contextMenu.Close();
				}
				Refresh();
			}
		}
		 */
		
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
		
		/*
		void ContextMenu_itemRemove_Click(object sender, EventArgs e)
		{
			ToClose(true);
		}
		 */
		
		void ContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// 本文入力
			this.contextMenu_itemBody.Enabled = !NoteItem.Compact;
			
			// クリップボード
			this.contextMenu_itemCopy.Enabled = !string.IsNullOrEmpty(NoteItem.Body);
			
			// 状態チェック
			var lockImage = NoteItem.Locked ? global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Lock : global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Unlock;
			this.contextMenu_itemLock.Image = lockImage;
			this.contextMenu_itemLock.Checked = NoteItem.Locked;
			this.contextMenu_itemCompact.Checked = NoteItem.Compact;
			this.contextMenu_itemTopmost.Checked = NoteItem.Topmost;
			
			// フォント
			this.contextMenu_font_change.Text = LanguageUtility.FontSettingToDisplayText(CommonData.Language, NoteItem.Style.FontSetting);
			
			// 色
			/*
			this._prevForeColor = ((ColorDisplayValue)this.contextMenu_itemForeColor.ComboBox.SelectedItem).Value;
			this._prevBackColor = ((ColorDisplayValue)this.contextMenu_itemBackColor.ComboBox.SelectedItem).Value;
			var foreColor = this.contextMenu_itemForeColor.ComboBox.Items.Cast<ColorDisplayValue>().SingleOrDefault(cd => cd.Value == NoteItem.Style.ForeColor);
			if(foreColor != null) {
				this.contextMenu_itemForeColor.SelectedItem = foreColor;
			} else {
				this.contextMenu_itemForeColor.SelectedItem = this.contextMenu_itemForeColor.Items.Cast<ColorDisplayValue>().Single(cd => IsCustomColor(cd.Value));
			}
			var backColor = this.contextMenu_itemBackColor.ComboBox.Items.Cast<ColorDisplayValue>().SingleOrDefault(cd => cd.Value == NoteItem.Style.BackColor);
			if(backColor != null) {
				this.contextMenu_itemBackColor.SelectedItem = backColor;
			} else {
				this.contextMenu_itemBackColor.SelectedItem = this.contextMenu_itemBackColor.Items.Cast<ColorDisplayValue>().Single(cd => IsCustomColor(cd.Value));
			}
			 */
			Action<ToolStripItem, IList<ColorMenuItem>, ToolStripItem, Color> checkColor = (parentItem, colorItemList, customItem, nowColor) => {
				var plainColor = false;
				
				parentItem.Image.ToDispose();
				parentItem.Image = AppUtility.CreateNoteBoxImage(nowColor, menuIconSize);
				
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
						customMenuItem.Image = AppUtility.CreateNoteBoxImage(nowColor, menuIconSize);
					} else {
						customMenuItem.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_CustomColor;
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
			this.contextMenu_itemCompact.Image = AppUtility.CreateNoteBoxImage(NoteItem.Style.BackColor, new Size(menuIconSize.Width, menuIconSize.Height / 2));
			
			// 入出力
			this.contextMenu_itemExport.Enabled = NoteItem.Body.Length > 0;

			// #120
			//// 拡張メニュー
			//var extension = AppUtility.IsExtension();
			//this.contextMenu_itemRemove.Visible = extension;
		}
		
		[System.Security.Permissions.UIPermission(
			System.Security.Permissions.SecurityAction.Demand,
			Window = System.Security.Permissions.UIPermissionWindow.AllWindows
		)]
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (this.inputTitle.Focused) {
				var key = keyData & Keys.KeyCode;
				switch (key) {
					case Keys.Enter:
						{
							HiddenInputTitleArea();
							return true;
						}
						
					case Keys.Escape:
						{
							_bindItem.Title = this._prevTitle;
							HiddenInputTitleArea();
							return true;
						}
						
					default:
						break;
				}
			}
			if (this.inputBody.Focused) {
				var key = keyData & Keys.KeyCode;
				if (key == Keys.Escape) {
					_bindItem.Body = this._prevBody;
					HiddenInputBodyArea();
					return true;
				}
			}
			
			return base.ProcessDialogKey(keyData);
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
	}
}