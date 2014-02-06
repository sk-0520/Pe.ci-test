/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/02/05
 * 時刻: 22:01
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using PeMain.Data;

namespace PeMain.UI
{
	/// <summary>
	/// Description of Skin.
	/// </summary>
	public interface ISkin
	{
		void DrawToolbarEdge(Graphics g, Rectangle drawArea, bool active, ToolbarPosition position);
		void DrawToolbarBackground(Graphics g, Rectangle drawArea, bool active, ToolbarPosition position);
		void DrawToolbarCaption(Graphics g, Rectangle drawArea, bool active, ToolbarPosition position);
		void DrawToolbarButton(Graphics g, Rectangle drawArea, bool active);
	}
}
