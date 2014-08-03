/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/03/12
 * 時刻: 4:30
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
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
		
		/*
		protected override void OnMouseMove(MouseEventArgs e)
		{
			var captionArea = CommonData.Skin.GetNoteCaptionArea(ClientSize);
			if(!captionArea.Size.IsEmpty) {
				var commands = new [] { NoteCommand.Compact, NoteCommand.Close, };
				var active = this == Form.ActiveForm;
				var noteStatus = GetNoteStatus();
				if(captionArea.Contains(e.Location)) {
					using(var g = CreateGraphics()) {
						foreach(var command in commands) {
							var commandArea = CommonData.Skin.GetNoteCommandArea(captionArea, command);
							var nowState = ButtonState.None;
							
							if(commandArea.Contains(e.Location)) {
								// 入っている
								var isClick = e.Button == MouseButtons.Left;
								nowState = isClick ? ButtonState.Pressed: ButtonState.Selected;
							} else {
								// 外
								nowState = ButtonState.Normal;
							}
							
							if(nowState != ButtonState.None) {
								if(this._commandStateMap[command] != nowState) {
									CommonData.Skin.DrawNoteCommand(g, commandArea, active, noteStatus, NoteItem.Style.ForeColor, NoteItem.Style.BackColor, command, nowState);
									this._commandStateMap[command] = nowState;
								}
							}
						}
					}
				}
			}
		}
		*/
	}
}