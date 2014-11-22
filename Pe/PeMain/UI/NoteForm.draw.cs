/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/07/29
 * 時刻: 2:21
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using PeMain.Data;
using PeMain.IF;

namespace PeMain.UI
{
	partial class NoteForm
	{
		void DrawEdge(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus)
		{
			CommonData.Skin.DrawNoteWindowEdge(g, drawArea, active, noteStatus, NoteItem.Style.ForeColor, NoteItem.Style.BackColor);
		}
		
		void DrawCaption(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus)
		{
			PeMain.IF.SkinButtonState buttonState = PeMain.IF.SkinButtonState.Normal;
			
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
				CommonData.Skin.DrawNoteBody(g, drawArea, active, noteStatus, NoteItem.Style.ForeColor, NoteItem.Style.BackColor, NoteItem.Style.FontSetting.Font, NoteItem.Body);
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
		
		void DrawCommand(Point point, Func<bool, PeMain.IF.SkinButtonState, PeMain.IF.SkinButtonState> inFirstDg, Action<NoteCommand> prevDrawDg, Action lastInDg, bool elseProcess)
		{
			var captionArea = CommonData.Skin.GetNoteCaptionArea(Size);
			if(!captionArea.Size.IsEmpty) {
				var active = this == Form.ActiveForm;
				var noteStatus = GetNoteStatus();
				if(captionArea.Contains(point)) {
					using(var g = CreateGraphics()) {
						foreach(var command in GetCommandList()) {
							var commandArea = CommonData.Skin.GetNoteCommandArea(captionArea, command);
							var nowState = PeMain.IF.SkinButtonState.None;
							var prevState = this._commandStateMap[command];
							nowState = inFirstDg(commandArea.Contains(point), prevState);
							
							if(nowState != PeMain.IF.SkinButtonState.None) {
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
								if(pair.Value != PeMain.IF.SkinButtonState.Normal) {
									var commandArea = CommonData.Skin.GetNoteCommandArea(captionArea, pair.Key);
									CommonData.Skin.DrawNoteCommand(g, commandArea, active, noteStatus, NoteItem.Style.ForeColor, NoteItem.Style.BackColor, pair.Key, PeMain.IF.SkinButtonState.Normal);
								}
							}
							foreach(var key in this._commandStateMap.Keys.ToArray()) {
								this._commandStateMap[key] = PeMain.IF.SkinButtonState.Normal;
							}
						}
					}
				}
			}
		}
		
	}
}
