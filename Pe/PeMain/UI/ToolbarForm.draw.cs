/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/11
 * 時刻: 1:18
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using PeMain.Data;

namespace PeMain.UI
{
	/// <summary>
	/// Description of ToolbarForm_draw.
	/// </summary>
	public partial class ToolbarForm
	{
		void DrawEdge(Graphics g, Rectangle drawArea, bool active)
		{
			CommonData.Skin.DrawToolbarEdge(g, drawArea, active, UseToolbarItem.ToolbarPosition);
		}
		
		void DrawCaption(Graphics g, Rectangle drawArea, bool active)
		{
			CommonData.Skin.DrawToolbarCaption(g, drawArea, active, UseToolbarItem.ToolbarPosition);
		}
		
		void DrawFull(Graphics g, Rectangle drawArea, bool active)
		{
			DrawEdge(g, drawArea, active);
			var captionArea = CommonData.Skin.GetToolbarCaptionArea(UseToolbarItem.ToolbarPosition, ClientSize);
			DrawCaption(g, captionArea, active);
			this.toolLauncher.Refresh();
		}
		
		void DrawFullActivaChanged(bool active)
		{
			using(var g = CreateGraphics())
			using(var bmp = new Bitmap(Width, Height, g))
			using(var memG = Graphics.FromImage(bmp)) {
				DrawFull(memG, ClientRectangle, active);
				g.DrawImage(bmp, 0, 0);
			}
		}
	}
}


