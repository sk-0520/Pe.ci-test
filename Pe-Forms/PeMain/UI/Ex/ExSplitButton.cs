namespace ContentTypeTextNet.Pe.PeMain.UI.Ex
{
	using System;
	using System.Drawing;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic;

	/// <summary>
	/// 拡張スプリットボタン基底クラス。
	/// </summary>
	public abstract class ExSplitButton: wyDay.Controls.SplitButton
	{ }
	
	/// <summary>
	/// フォント選択用コントロール。
	/// </summary>
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
			Text = "{FAMILY} {PT} ...";

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
				dialog.SetFontSetting(FontSetting);
				
				if(dialog.ShowDialog() == DialogResult.OK) {
					FontSetting.Import(dialog.Font);
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
			Text = LanguageUtility.FontSettingToDisplayText(Language, FontSetting);
		}

	}
	
}
