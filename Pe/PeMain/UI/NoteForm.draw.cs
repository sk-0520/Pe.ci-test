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

namespace PeMain.UI
{
	partial class NoteForm
	{
		void DrawEdge(Graphics g, Rectangle drawArea, bool active)
		{
			
		}
		
		void DrawCaption(Graphics g, Rectangle drawArea, bool active)
		{
			
		}

		void DrawNoClient(Graphics g, Rectangle drawArea, bool active)
		{
			if(!CommonData.Skin.IsDefaultDrawToolbarWindowBackground) {
				CommonData.Skin.DrawNoteWindowBackground(g, drawArea, active, NoteItem.Style.BaclColor);
			}
			
			DrawEdge(g, drawArea, active);
			var captionArea = CommonData.Skin.GetNoteCaptionArea(ClientSize);
			if(!captionArea.Size.IsEmpty) {
				DrawCaption(g, captionArea, active);
			}
		}
	}
}
