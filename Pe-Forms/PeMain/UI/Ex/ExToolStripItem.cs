namespace ContentTypeTextNet.Pe.PeMain.UI.Ex
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Drawing;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.Skin;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic;

	#region abstract

	public abstract class ExToolStripSeparator: ToolStripSeparator
	{ }

	public abstract class ExToolStripItem: ToolStripItem
	{ }

	public abstract class ExToolStripButton: ToolStripButton
	{ }

	public abstract class ExToolStripMenuItem: ToolStripMenuItem
	{ }

	public abstract class ExToolStripDropDownButton: ToolStripDropDownButton
	{ }

	public abstract class ExToolStripSplitButton: ToolStripSplitButton
	{ }
	
	public abstract class ExToolStripTextBox: ToolStripTextBox
	{ }

	#endregion

	#region ExDisableCloseToolStripSeparator

	/// <summary>
	/// クリックしても閉じないセパレータ。
	/// </summary>
	public class DisableCloseToolStripSeparator: ExToolStripSeparator
	{
		public DisableCloseToolStripSeparator()
			: base()
		{
			// http://blogs.wankuma.com/youryella/archive/2007/08/19/91000.aspx
			Enabled = false;
		}
	}

	#endregion

	#region ExToolStripMenuItem

	/// <summary>
	/// 共通データを保持するメニューアイテム。
	/// </summary>
	/// <param name="commonData"></param>
	public abstract class CommonDataToolStripMenuItem: ExToolStripMenuItem, ICommonData
	{
		public CommonDataToolStripMenuItem(CommonData commonData)
			: base()
		{
			CommonData = commonData;
		}

		public CommonData CommonData { get; private set; }
	}

	/// <summary>
	/// ファイルパスを保持するメニューアイテム。
	/// </summary>
	public class FileToolStripMenuItem: CommonDataToolStripMenuItem
	{
		public FileToolStripMenuItem(CommonData commonData)
			: base(commonData)
		{ }

		public string Path { get; set; }
	}

	/// <summary>
	/// ファイルパスに関連するアイコンを保持するメニューアイテム。
	/// 
	/// アイコンイメージの取得処理は上位で管理する。
	/// </summary>
	public class FileImageToolStripMenuItem: FileToolStripMenuItem
	{
		public FileImageToolStripMenuItem(CommonData commonData)
			: base(commonData)
		{ }

		public Image FileImage { get; set; }

		public override Image Image
		{
			get
			{
				if(FileImage != null) {
					return FileImage;
				} else {
					return base.Image;
				}
			}
			set
			{
				base.Image = value;
			}
		}
	}

	/// <summary>
	/// スクリーン情報を保持するメニューアイテム。
	/// </summary>
	public class ScreenToolStripMenuItem: CommonDataToolStripMenuItem, IScreen
	{
		protected Screen _screen;

		public ScreenToolStripMenuItem(CommonData commonData)
			: base(commonData)
		{ }

		public Screen Screen
		{
			get { return this._screen; }
			set
			{
				this._screen = value;
				Text = ScreenUtility.GetScreenName(Screen);
			}
		}
	}

	public class ToolbarGroupItemToolStripMenuItem: CommonDataToolStripMenuItem, IToolbarGroupItem
	{
		public ToolbarGroupItemToolStripMenuItem(CommonData commonData)
			: base(commonData)
		{ }

		public ToolbarGroupItem ToolbarGroupItem { get; set; }
	}

	/// <summary>
	/// ランチャーアイテムを保持するメニューアイテム。
	/// </summary>
	public class LauncherItemToolStripMenuItem: CommonDataToolStripMenuItem, ILauncherItem
	{
		public LauncherItemToolStripMenuItem(CommonData commonData)
			: base(commonData)
		{ }

		public LauncherItem LauncherItem { get; set; }
	}

	public class ApplicationItemToolStripMenuItem: LauncherItemToolStripMenuItem, IApplicationItem
	{
		public ApplicationItemToolStripMenuItem(CommonData commonData)
			: base(commonData)
		{ }

		public ApplicationItem ApplicationItem { get; set; }
	}

	/// <summary>
	/// ノートを保持するメニューアイテム。
	/// </summary>
	public class NoteItemToolStripMenuItem: CommonDataToolStripMenuItem
	{
		public NoteItemToolStripMenuItem(CommonData commonData)
			: base(commonData)
		{ }

		public NoteItem NoteItem { get; set; }
	}


	#endregion

	#region ExToolStripButton

	/// <summary>
	/// 共通データを保持するドロップダウンボタンアイテム。
	/// </summary>
	/// <param name="commonData"></param>
	public class CommonDataToolStripButton: ExToolStripButton, ICommonData
	{
		/// <summary>
		/// 共通データを保持するドロップダウンボタンアイテム。
		/// </summary>
		/// <param name="commonData"></param>
		public CommonDataToolStripButton(CommonData commonData)
			: base()
		{
			CommonData = commonData;
		}

		public CommonData CommonData { get; private set; }
	}

	/// <summary>
	/// ランチャーアイテムを保持するスプリットボタンアイテム。
	/// </summary>
	public class LauncherItemToolStripButton: CommonDataToolStripButton, ILauncherItem
	{
		public LauncherItemToolStripButton(CommonData commonData)
			: base(commonData)
		{ }

		public LauncherItem LauncherItem { get; set; }
	}

	#endregion

	#region ExToolStripDropDownButton

	/// <summary>
	/// 共通データを保持するドロップダウンボタンアイテム。
	/// </summary>
	/// <param name="commonData"></param>
	public class CommonDataToolStripDropDownButton: ExToolStripDropDownButton, ICommonData
	{
		/// <summary>
		/// 共通データを保持するドロップダウンボタンアイテム。
		/// </summary>
		/// <param name="commonData"></param>
		public CommonDataToolStripDropDownButton(CommonData commonData)
			: base()
		{
			CommonData = commonData;
		}

		public CommonData CommonData { get; private set; }
	}

	/// <summary>
	/// ランチャーアイテムを保持するスプリットボタンアイテム。
	/// </summary>
	public class LauncherItemToolStripDropDownButton: CommonDataToolStripDropDownButton, ILauncherItem
	{
		public LauncherItemToolStripDropDownButton(CommonData commonData)
			: base(commonData)
		{ }

		public LauncherItem LauncherItem { get; set; }
	}

	#endregion

	#region ExToolStripSplitButton

	public abstract class CommonDataToolStripSplitButton: ExToolStripSplitButton, ICommonData
	{
		/// <summary>
		/// 共通データを保持するスプリットボタンアイテム。
		/// </summary>
		/// <param name="commonData"></param>
		public CommonDataToolStripSplitButton(CommonData commonData)
			: base()
		{
			CommonData = commonData;
		}

		public CommonData CommonData { get; private set; }
	}

	/// <summary>
	/// ランチャーアイテムを保持するスプリットボタンアイテム。
	/// </summary>
	public class LauncherItemToolStripSplitButton: CommonDataToolStripSplitButton, ILauncherItem
	{
		public LauncherItemToolStripSplitButton(CommonData commonData)
			: base(commonData)
		{ }

		public LauncherItem LauncherItem { get; set; }
	}

	#endregion

	#region ExToolStripTextBox

	/// <summary>
	/// https://msdn.microsoft.com/ja-jp/library/ms404304%28v=vs.110%29.aspx
	/// </summary>
	public class FillToolStripTextBox: ExToolStripTextBox
	{
		public override Size GetPreferredSize(Size constrainingSize)
		{
			// Use the default size if the text box is on the overflow menu
			// or is on a vertical ToolStrip.
			if(IsOnOverflow || Owner.Orientation == Orientation.Vertical) {
				return DefaultSize;
			}

			// Declare a variable to store the total available width as 
			// it is calculated, starting with the display width of the 
			// owning ToolStrip.
			Int32 width = Owner.DisplayRectangle.Width;

			// Subtract the width of the overflow button if it is displayed. 
			if(Owner.OverflowButton.Visible) {
				width = width - Owner.OverflowButton.Width -
					Owner.OverflowButton.Margin.Horizontal;
			}

			// Declare a variable to maintain a count of ToolStripSpringTextBox 
			// items currently displayed in the owning ToolStrip. 
			Int32 springBoxCount = 0;

			foreach(ToolStripItem item in Owner.Items) {
				// Ignore items on the overflow menu.
				if(item.IsOnOverflow) continue;

				if(item is FillToolStripTextBox) {
					// For ToolStripSpringTextBox items, increment the count and 
					// subtract the margin width from the total available width.
					springBoxCount++;
					width -= item.Margin.Horizontal;
				} else {
					// For all other items, subtract the full width from the total
					// available width.
					width = width - item.Width - item.Margin.Horizontal;
				}
			}

			// If there are multiple ToolStripSpringTextBox items in the owning
			// ToolStrip, divide the total available width between them. 
			if(springBoxCount > 1) width /= springBoxCount;

			// If the available width is less than the default width, use the
			// default width, forcing one or more items onto the overflow menu.
			if(width < DefaultSize.Width) width = DefaultSize.Width;

			// Retrieve the preferred size from the base class, but change the
			// width to the calculated width. 
			Size size = base.GetPreferredSize(constrainingSize);
			size.Width = width;
			return size;
		}
	}

	#endregion
}
