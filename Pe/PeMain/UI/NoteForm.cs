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
using System.Linq;
using System.Windows.Forms;
using PeMain.Data;
using PeMain.Logic;
using PInvoke.Windows;

namespace PeMain.UI
{
	/// <summary>
	/// Description of NoteForm.
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
		
		void NoteForm_Paint(object sender, PaintEventArgs e)
		{
			using(var bmp = new Bitmap(Width, Height, e.Graphics)) {
				using(var memG = Graphics.FromImage(bmp)) {
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
			if(this._initialized) {
				HiddenInputTitleArea();
				HiddenInputBodyArea();
			}
			
			
			DrawFullActivaChanged(false);
			
			SaveItem();
		}
		
		
		void NoteForm_MouseDown(object sender, MouseEventArgs e)
		{
			HiddenInputTitleArea();
			
			if(NoteItem.Locked) {
				return;
			}
			
			DrawCommand(
				e.Location,
				(isIn, nowState) => {
					var left = (MouseButtons & MouseButtons.Left) == MouseButtons.Left;
					if(left && isIn) {
						return ButtonState.Pressed;
					} else {
						return ButtonState.Normal;
					}
				},
				null,
				null,
				false
			);
		}
		
		void NoteForm_MouseUp(object sender, MouseEventArgs e)
		{
			if(NoteItem.Locked) {
				return;
			}
			
			DrawCommand(
				e.Location,
				(isIn, nowState) => {
					var left = (MouseButtons & MouseButtons.Left) == MouseButtons.Left;
					if(left && isIn) {
						return ButtonState.Selected;
					} else {
						return ButtonState.Normal;
					}
				},
				command => {
					if(this._commandStateMap[command] == ButtonState.Pressed) {
						var isRemove = (ModifierKeys & Keys.Shift) == Keys.Shift;
						if(isRemove) {
							var map = new Dictionary<string, string>() {
								{ "NOTE", NoteItem.Title },
							};
							var result = MessageBox.Show(CommonData.Language["note/dialog/message", map], CommonData.Language["note/dialog/caption"], MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
							if(result == DialogResult.Cancel) {
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
			if(!NoteItem.Locked) {
				ShowInputBodyArea(RECURSIVE);
			}
		}
		
		void NoteForm_Resize(object sender, EventArgs e)
		{
			if(!this._initialized && NoteItem.Compact) {
				//ChangeCompact(true, Size.Empty);
				Height = 20;
			} else {
				ResizeInputTitleArea();
				ResizeInputBodyArea();
				
				if(!NoteItem.Compact) {
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
			using(var dialog = new FontDialog()) {
				if(NoteItem.Style.FontSetting.IsDefault) {
					dialog.Font = NoteItem.Style.FontSetting.Font;
				}
				
				if(dialog.ShowDialog() == DialogResult.OK) {
					NoteItem.Style.FontSetting.Include(dialog.Font);
				}
			}
			Refresh();
		}
		
		void ContextMenu_font_reset_Click(object sender, EventArgs e)
		{
			if(!NoteItem.Style.FontSetting.IsDefault) {
				NoteItem.Style.FontSetting.Dispose();
				NoteItem.Style.FontSetting = new FontSetting();
			}
			Refresh();
		}
		
		void ContextMenu_fore_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(this._bindItem != null) {
				NoteItem.Style.ForeColor = GetSelectedColor(this.contextMenu_fore);
				Changed = true;
				Refresh();
			}
		}
		
		void ContextMenu_back_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(this._bindItem != null) {
				NoteItem.Style.BackColor = GetSelectedColor(this.contextMenu_back);
				Changed = true;
				Refresh();
			}
		}
		
		void ContextMenu_Closed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			if(Form.ActiveForm != this && Changed) {
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
		
		void ContextMenu_itemRemove_Click(object sender, EventArgs e)
		{
			ToClose(true);
		}
		
		void ContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// 本文入力
			this.contextMenu_itemBody.Enabled = !NoteItem.Compact;
			
			// クリップボード
			this.contextMenu_itemCopy.Enabled = !string.IsNullOrEmpty(NoteItem.Body);
			
			// 状態チェック
			this.contextMenu_itemLock.Checked = NoteItem.Locked;
			this.contextMenu_itemCompact.Checked = NoteItem.Compact;
			this.contextMenu_itemTopmost.Checked = NoteItem.Topmost;
			
			// 色
			var foreColor = this.contextMenu_fore.ComboBox.Items.Cast<ColorData>().SingleOrDefault(cd => cd.Value == NoteItem.Style.ForeColor);
			if(foreColor != null) {
				this.contextMenu_fore.SelectedItem = foreColor;
			}
			var backColor = this.contextMenu_back.ComboBox.Items.Cast<ColorData>().SingleOrDefault(cd => cd.Value == NoteItem.Style.BackColor);
			if(backColor != null) {
				this.contextMenu_back.SelectedItem = backColor;
			}
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
			if(this.inputBody.Focused) {
				var key = keyData & Keys.KeyCode;
				if(key == Keys.Escape) {
					_bindItem.Body = this._prevBody;
					HiddenInputBodyArea();
					return true;
				}
			}
			
			return base.ProcessDialogKey(keyData);
		}
	}
}