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

namespace PeMain.UI
{
	/// <summary>
	/// Description of ToolbarForm_draw.
	/// </summary>
	public partial class ToolbarForm
	{
		void DrawEdge(Graphics g, Rectangle drawArea, bool active)
		{
			var light = SystemBrushes.ControlLight;
			var dark = SystemBrushes.ControlDark;
			
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
			Brush brush = active ? SystemBrushes.ActiveCaption: SystemBrushes.InactiveCaption;
			g.FillRectangle(brush, drawArea);
		}
		
		void DrawFull(Graphics g, Rectangle drawArea, bool active)
		{
			DrawEdge(g, drawArea, active);
			var captionArea = GetCaptionArea(MainSetting.Toolbar.ToolbarPosition);
			DrawCaption(g, captionArea, active, IsHorizonMode(MainSetting.Toolbar.ToolbarPosition));
		}
	}
}


