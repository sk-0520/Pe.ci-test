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
using System.Windows.Forms;

using PeMain.Data;
using PI.Windows;

namespace PeMain.UI
{
	/// <summary>
	/// Description of Skin.
	/// </summary>
	public interface ISkin
	{
		void Start(Form target);
		void Refresh(Form target);
		void Close(Form target);
		
		/*
		void DrawToolbarEdge(Graphics g, Rectangle drawArea, bool active, ToolbarPosition position);
		void DrawToolbarBackground(Graphics g, Rectangle drawArea, bool active, ToolbarPosition position);
		void DrawToolbarCaption(Graphics g, Rectangle drawArea, bool active, ToolbarPosition position);
		void DrawToolbarButton(Graphics g, Rectangle drawArea, bool active);
		*/
	}
	
	public abstract class Skin: ISkin
	{
		public bool EnabledVisualStyle { get; set; }
		
		protected static bool IsEnabledVisualStyle()
		{
			bool isAero;
			API.DwmIsCompositionEnabled(out isAero);
			return isAero;
		}
		
		public virtual void Start(Form target)
		{
			EnabledVisualStyle = IsEnabledVisualStyle();
		}
		public virtual void Refresh(Form target)
		{
			EnabledVisualStyle = IsEnabledVisualStyle();
			
		}
		public abstract void Close(Form target);
		
		/*
		public abstract void DrawToolbarEdge(Graphics g, Rectangle drawArea, bool active, ToolbarPosition position);
		public abstract void DrawToolbarBackground(Graphics g, Rectangle drawArea, bool active, ToolbarPosition position);
		public abstract void DrawToolbarCaption(Graphics g, Rectangle drawArea, bool active, ToolbarPosition position);
		public abstract void DrawToolbarButton(Graphics g, Rectangle drawArea, bool active);
		*/
	}
	
	
}
