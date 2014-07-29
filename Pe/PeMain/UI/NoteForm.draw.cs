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
			CommonData.Skin.DrawNoteCaption(g, drawArea, active, noteStatus, NoteItem.Style.ForeColor, NoteItem.Style.BackColor, CommonData.MainSetting.Note.CaptionFontSetting.Font, NoteItem.Title);
			var commands = new [] { NoteCommand.Lock, NoteCommand.Compact, NoteCommand.Close, };
			foreach(var command in commands) {
				var commandArea = CommonData.Skin.GetNoteCommandArea(drawArea, command);
				CommonData.Skin.DrawNoteCommand(g, commandArea, active, noteStatus, NoteItem.Style.ForeColor, NoteItem.Style.BackColor, command);
			}
		}

		void DrawNoClient(Graphics g, Rectangle drawArea, bool active)
		{
			var noteStatus = GetNoteStatus();
			//if(!CommonData.Skin.IsDefaultDrawToolbarWindowBackground) {
				CommonData.Skin.DrawNoteWindowBackground(g, drawArea, active, noteStatus, NoteItem.Style.BackColor);
			//}
			
			DrawEdge(g, drawArea, active, noteStatus);
			var captionArea = CommonData.Skin.GetNoteCaptionArea(ClientSize);
			if(!captionArea.Size.IsEmpty) {
				DrawCaption(g, captionArea, active, noteStatus);
			}
		}
		
		void DrawBody(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus)
		{
			CommonData.Skin.DrawNoteBody(g, drawArea, active, noteStatus, NoteItem.Style.ForeColor, NoteItem.Style.BackColor, NoteItem.Style.FontSetting.Font, NoteItem.Body);
		}
	}
}
