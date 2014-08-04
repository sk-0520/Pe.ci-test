/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/03/12
 * 時刻: 4:30
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using PeMain.Data;
using PeMain.Logic;

namespace PeMain.UI
{
	partial class NoteForm
	{
		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;
			
			ApplySetting();
		}
		
		void ApplySetting()
		{
			
		}
		
		SkinNoteStatus GetNoteStatus()
		{
			return new SkinNoteStatus();
		}
		
		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			//base.OnPaintBackground(pevent);
		}
		
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			this.Invalidate();
		}
		
		void clickCommand(NoteCommand noteCommand)
		{
			MessageBox.Show(noteCommand.ToString());
		}
		
		
		void DrawCommand(Point point, Func<bool, ButtonState, ButtonState> inFirstDg, Action<NoteCommand> prevDrawDg, Action lastInDg, bool elseProcess)
		{
			var captionArea = CommonData.Skin.GetNoteCaptionArea(ClientSize);
			if(!captionArea.Size.IsEmpty) {
				var commands = new [] { NoteCommand.Topmost, NoteCommand.Compact, NoteCommand.Close, };
				var active = this == Form.ActiveForm;
				var noteStatus = GetNoteStatus();
				if(captionArea.Contains(point)) {
					using(var g = CreateGraphics()) {
						foreach(var command in commands) {
							var commandArea = CommonData.Skin.GetNoteCommandArea(captionArea, command);
							var nowState = ButtonState.None;
							var prevState = this._commandStateMap[command];
							nowState = inFirstDg(commandArea.Contains(point), prevState);
							
							if(nowState != ButtonState.None) {
								if(prevDrawDg != null) {
									prevDrawDg(command);
								}
								if(nowState != prevState) {
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
								if(pair.Value != ButtonState.Normal) {
									var commandArea = CommonData.Skin.GetNoteCommandArea(captionArea, pair.Key);
									CommonData.Skin.DrawNoteCommand(g, commandArea, active, noteStatus, NoteItem.Style.ForeColor, NoteItem.Style.BackColor, pair.Key, ButtonState.Normal);
								}
							}
						}
					}
				}
			}
		}
		
		Rectangle GetBodyArea()
		{
			return GetBodyArea(
				this.CommonData.Skin.GetNoteWindowEdgePadding(),
				this.CommonData.Skin.GetNoteCaptionArea(ClientSize)
			);
		}
		Rectangle GetBodyArea(Padding edge, Rectangle captionArea)
		{
			return new Rectangle(
				 new Point(edge.Left, captionArea.Bottom),
				 new Size(ClientSize.Width - edge.Horizontal, ClientSize.Height - (edge.Vertical + captionArea.Height))
			);
		}

		void ResizeInputArea()
		{
			var bodyArea = GetBodyArea();
			this.inputBody.Location = bodyArea.Location;
			this.inputBody.Size = bodyArea.Size;
		}
		
		void ShowInputArea()
		{
			this.inputBody.Text = NoteItem.Body;
			this.inputBody.Font = NoteItem.Style.FontSetting.Font;
			
			if(!this.inputBody.Visible) {
				ResizeInputArea();
				this.inputBody.Visible = true;
				this.inputBody.Focus();
			}
		}
		
		void HiddenInputArea()
		{
			NoteItem.Body = this.inputBody.Text; 
			this.inputBody.Visible = false;
		}
		
		void ShowContextMenu(Point point)
		{
			this.contextMenu.Show(this, point);
		}

	}
}