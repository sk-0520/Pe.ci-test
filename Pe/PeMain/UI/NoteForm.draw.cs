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
using System.Drawing.Drawing2D;
using PeMain.Data;

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
			ButtonState buttonState = ButtonState.Normal;
			
			CommonData.Skin.DrawNoteCaption(g, drawArea, active, noteStatus, NoteItem.Style.ForeColor, NoteItem.Style.BackColor, CommonData.MainSetting.Note.CaptionFontSetting.Font, NoteItem.Title);
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
			CommonData.Skin.DrawNoteBody(g, drawArea, active, noteStatus, NoteItem.Style.ForeColor, NoteItem.Style.BackColor, NoteItem.Style.FontSetting.Font, NoteItem.Body);
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
	}
}
