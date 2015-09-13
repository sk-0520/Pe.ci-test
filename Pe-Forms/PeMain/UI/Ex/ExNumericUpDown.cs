namespace ContentTypeTextNet.Pe.PeMain.UI.Ex
{
	using System.Windows.Forms;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;

	public abstract class ExNumericUpDown: NumericUpDown
	{ }

	public class DefaultValueNumericUpDown:ExNumericUpDown
	{
		public decimal DefaultValue { get; set; }
	}

	public class RevertDefaultValueNumericUpDown: DefaultValueNumericUpDown, ISetLanguage, ILanguage
	{
		const string MenuItemName_revert = "menuitem_revert";

		protected ContextMenu _contextMenu;

		public RevertDefaultValueNumericUpDown()
		{
			this._contextMenu = new ContextMenu();

			var menuItemRevert = new MenuItem() {
				Name = MenuItemName_revert,
			};
			menuItemRevert.Click += MenuItemRevert_Click;

			this._contextMenu.MenuItems.Add(menuItemRevert);

			ContextMenu = this._contextMenu;
		}

		public Language Language { get; private set; }

		public void SetLanguage(Language language)
		{
			Language = language;

			this._contextMenu.MenuItems[MenuItemName_revert].Text = Language["updown/revert"];
		}

		void MenuItemRevert_Click(object sender, System.EventArgs e)
		{
			Value = DefaultValue;
		}

	}
}
