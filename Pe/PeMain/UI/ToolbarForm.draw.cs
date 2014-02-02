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

namespace PeMain.UI
{
	/// <summary>
	/// Description of ToolbarForm_draw.
	/// </summary>
	public partial class ToolbarForm
	{
		void DrawEdge(Graphics g, Rectangle drawArea, bool active)
		{
			var light = active ? SystemBrushes.ControlLight: SystemBrushes.ControlLightLight;
			var dark = active ? SystemBrushes.ControlDarkDark: SystemBrushes.ControlDark;
			
			// 下
			g.FillRectangle(dark, 0, drawArea.Height - Padding.Bottom, Width, Padding.Bottom);
			// 右
			g.FillRectangle(dark, drawArea.Width - Padding.Right, 0, Padding.Right, Height);
			// 左
			g.FillRectangle(dark, 0, 0, Padding.Left, Height);
			// 上
			g.FillRectangle(dark, 0, 0, Width, Padding.Top);
		}
		
		void DrawCaption(Graphics g, Rectangle drawArea, bool active, bool horizon)
		{
			Color headColor;
			Color tailColor;
			if(active) {
				headColor = SystemColors.GradientActiveCaption;
				tailColor = SystemColors.ActiveCaption;
			} else {
				headColor = SystemColors.GradientInactiveCaption;
				tailColor = SystemColors.InactiveCaption;
			}
			var mode = IsHorizonMode(UseToolbarItem.ToolbarPosition) ? LinearGradientMode.Vertical: LinearGradientMode.Horizontal;
			using(var brush = new LinearGradientBrush(drawArea, headColor, tailColor, mode)) {
				g.FillRectangle(brush, drawArea);
			}
		}
		
		void DrawFull(Graphics g, Rectangle drawArea, bool active)
		{
			DrawEdge(g, drawArea, active);
			var captionArea = GetCaptionArea(UseToolbarItem.ToolbarPosition);
			DrawCaption(g, captionArea, active, IsHorizonMode(UseToolbarItem.ToolbarPosition));
		}
		
		void DrawFullActivaChanged(bool active)
		{
			using(var g = CreateGraphics()) {
				DrawFull(g, ClientRectangle, active);
			}
		}
	}
}


