/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/05
 * 時刻: 2:21
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using System.Windows.Forms;

using PeUtility;

namespace PeMain.UI
{
	/// <summary>
	/// Description of ToolbarForm_define.
	/// </summary>
	public partial class ToolbarForm
	{
		struct ButtonLayout
		{
			public int IconWidth { get; set; }
			public int TextWidth { get; set; }
			public int Separator { get; set; }
			public Padding Padding { get; set; }
			
			public Size ClientSize
			{
				get
				{
					return new Size(
						IconWidth + Separator + TextWidth + Padding.Horizontal,
						IconWidth + Padding.Vertical
					);
				}
			}
		}
		
		ButtonLayout GetButtonLayout()
		{
			var result = new ButtonLayout();
			
			result.IconWidth = ToolbarSetting.IconSize.ToHeight();
			if(ToolbarSetting.ShowText) {
				result.TextWidth = ToolbarSetting.Width;
				result.Separator = 1;
			} else {
				result.TextWidth = 0;
				result.Separator = 0;
			}
			result.Padding = new Padding(4);
			
			return result;
		}
	}
}

