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
			if(CommonData.Skin.IsDefaultDrawToolbarWindowEdge) {
				var edgePadding = CommonData.Skin.GetToolbarWindowEdgePadding(UseToolbarItem.ToolbarPosition);
				
				// 境界線
				var light = active ? SystemBrushes.ControlLight: SystemBrushes.ControlLightLight;
				var dark = active ? SystemBrushes.ControlDarkDark: SystemBrushes.ControlDark;
				
				// 下
				g.FillRectangle(dark, 0, drawArea.Height - edgePadding.Bottom, drawArea.Width, edgePadding.Bottom);
				// 右
				g.FillRectangle(dark, drawArea.Width - edgePadding.Right, 0, edgePadding.Right, drawArea.Height);
				// 左
				g.FillRectangle(dark, 0, 0, edgePadding.Left, drawArea.Height);
				// 上
				g.FillRectangle(dark, 0, 0, drawArea.Width, edgePadding.Top);
			} else {
				CommonData.Skin.DrawToolbarWindowEdge(g, drawArea, active, UseToolbarItem.ToolbarPosition);
			}
		}
		
		void DrawCaption(Graphics g, Rectangle drawArea, bool active)
		{
			if(CommonData.Skin.IsDefaultDrawToolbarWindowCaption) {
				Color headColor;
				Color tailColor;
				if(active) {
					headColor = SystemColors.GradientActiveCaption;
					tailColor = SystemColors.ActiveCaption;
				} else {
					headColor = SystemColors.GradientInactiveCaption;
					tailColor = SystemColors.InactiveCaption;
				}
				var mode = ToolbarPositionUtility.IsHorizonMode(UseToolbarItem.ToolbarPosition) ? LinearGradientMode.Vertical: LinearGradientMode.Horizontal;
				using(var brush = new LinearGradientBrush(drawArea, headColor, tailColor, mode)) {
					g.FillRectangle(brush, drawArea);
				}
			} else {
				CommonData.Skin.DrawToolbarWindowCaption(g, drawArea, active, UseToolbarItem.ToolbarPosition);
			}
		}
		
		void DrawNoClient(Graphics g, Rectangle drawArea, bool active)
		{
			if(!CommonData.Skin.IsDefaultDrawToolbarWindowBackground) {
				CommonData.Skin.DrawToolbarWindowBackground(g, drawArea, active, UseToolbarItem.ToolbarPosition);
			}
			
			var captionArea = CommonData.Skin.GetToolbarCaptionArea(UseToolbarItem.ToolbarPosition, ClientSize);
			if(!captionArea.Size.IsEmpty) {
				DrawCaption(g, captionArea, active);
			}
			DrawEdge(g, drawArea, active);
		}
		
		void DrawFull(Graphics g, Rectangle drawArea, bool active)
		{
			DrawNoClient(g, drawArea, active);
			this.toolLauncher.Refresh();
		}
		
		void DrawFullActivaChanged(bool active)
		{
			using(var g = CreateGraphics()) {
				using(var bmp = new Bitmap(Width, Height, g)) {
					using(var memG = Graphics.FromImage(bmp)) {
						var rect = new Rectangle(Point.Empty, Size);
						DrawFull(memG, rect, active);
						if(!CommonData.Skin.IsDefaultDrawToolbarWindowBackground) {
							g.CompositingMode = CompositingMode.SourceCopy;
						}
						g.DrawImage(bmp, 0, 0);
					}
				}
			}
		}
	}
}



