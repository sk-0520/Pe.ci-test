/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/02/25
 * 時刻: 16:48
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Windows.Forms;
using PeMain.Data;
using PeMain.Logic;

namespace PeMain.UI
{
	/// <summary>
	/// Description of ExSplitButton.
	/// </summary>
	public abstract class ExSplitButton: wyDay.Controls.SplitButton
	{ }
	
	public class FontSplitButton: ExSplitButton, ISetLanguage
	{
		const string nameMenuReset = "reset";
		
		public Language Language { get; private set; }
		
		public FontSetting FontSetting { get; private set; }
		
		public FontSplitButton()
		{
			Initialize();
		}
		
		void Initialize()
		{
			var menu = new ContextMenuStrip();
			var itemReset = new ToolStripMenuItem();
			itemReset.Name = nameMenuReset;
			itemReset.Text = "{RESET}";
			itemReset.Click += new EventHandler(itemReset_Click);
			
			menu.Items.Add(itemReset);
			
			SplitMenuStrip = menu;
			
			FontSetting = new FontSetting();
			
			Click += new EventHandler(FontSplitButton_Click);
		}

		void FontSplitButton_Click(object sender, EventArgs e)
		{
			using(var dialog = new FontDialog()) {
				if(FontSetting.IsDefault) {
					dialog.Font = FontSetting.Font;
				}
				
				if(dialog.ShowDialog() == DialogResult.OK) {
					var result = new FontSetting();
					var font = dialog.Font;
					FontSetting.Family = font.FontFamily.Name;
					FontSetting.Height = font.Size;

					RefreshView();
				}
			}
		}

		void itemReset_Click(object sender, EventArgs e)
		{
			FontSetting.Dispose();
			FontSetting = new FontSetting();
			RefreshView();
		}
		
		public void SetLanguage(Language language)
		{
			Language = language;
			SplitMenuStrip.Items[nameMenuReset].Text = Language["common/command/default-font-reset"];
		}
		
		public void RefreshView()
		{
			string viewText = Language["common/command/default-font"];
			if(FontSetting != null && !FontSetting.IsDefault) {
				viewText = string.Format("{0} {1}", FontSetting.Family, FontSetting.Height);
			}
			Text = viewText;
		}

	}
	
}
