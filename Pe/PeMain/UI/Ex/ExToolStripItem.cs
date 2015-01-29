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

	public abstract class ExToolStripMenuItem: ToolStripMenuItem
	{ }

	public abstract class ExToolStripSplitButton: ToolStripSplitButton
	{ }

	#endregion

	#region ExDisableCloseToolStripSeparator

	/// <summary>
	/// クリックしても閉じないセパレータ。
	/// </summary>
	public class DisableCloseToolStripSeparator: ExToolStripSeparator
	{
		public DisableCloseToolStripSeparator()
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

	/// <summary>
	/// ランチャーアイテムを保持するメニューアイテム。
	/// </summary>
	public class LauncherToolStripMenuItem: CommonDataToolStripMenuItem, ILauncherItem
	{
		public LauncherToolStripMenuItem(CommonData commonData)
			: base(commonData)
		{ }

		public LauncherItem LauncherItem { get; set; }
	}

	#endregion

	#region ExToolStripSplitButton

	public abstract class CommonDataToolStripSplitButton: ExToolStripSplitButton, ICommonData
	{
		/// <summary>
		/// 共通データを保持するすぷちっとボタンアイテム。
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
	public class LauncherToolStripSplitButton: CommonDataToolStripSplitButton, ILauncherItem
	{
		public LauncherToolStripSplitButton(CommonData commonData)
			: base(commonData)
		{ }

		public LauncherItem LauncherItem { get; set; }
	}

	#endregion

}
