namespace ContentTypeTextNet.Pe.Library.Skin
{
	using System;
	using System.Drawing;
	using System.Windows.Forms;

	/// <summary>
	///スキン。
	///
	/// これを継承すればPe側から使用される。
	/// </summary>
	public interface ISkin
	{
		#region Initialize

		/// <summary>
		/// 必要最低限の準備を行う。
		/// </summary>
		void Load();

		/// <summary>
		/// スキンを初期状態に戻す。
		/// 
		/// インスタンスとして有効であっても再使用には Load → Initialize が必要。
		/// </summary>
		void Unload();
		
		/// <summary>
		/// スキンとして処理可能な状態まで初期化する。
		/// 
		/// Load後に呼び出される。
		/// </summary>
		void Initialize();
		#endregion

		#region Setting About

		/// <summary>
		/// スキンが何かを示す。
		/// </summary>
		ISkinAbout About { get; }

		#endregion

		#region Style

		/// <summary>
		/// 指定フォームにスタイルを適用する。
		/// </summary>
		/// <param name="target"></param>
		void AttachmentStyle(Form target, SkinWindow skinWindow);

		/// <summary>
		/// 指定フォームのスタイルを再設定する。
		/// </summary>
		/// <param name="target"></param>
		void RefreshStyle(Form target, SkinWindow skinWindow);

		/// <summary>
		/// 指定フォームのスタイル適用を取り消す。
		/// </summary>
		/// <param name="target"></param>
		void DetachmentStyle(Form target, SkinWindow skinWindow);

		#endregion

		#region Resource

		/// <summary>
		/// 画像を取得する。
		/// </summary>
		/// <param name="skinImage">画像種別。</param>
		/// <returns>要求された画像。画像自体の所有者はあくまでスキン側でPe側で解放処理がなされることはない。</returns>
		Image GetImage(SkinImage skinImage);

		/// <summary>
		/// アイコンを取得する。
		/// </summary>
		/// <param name="skinIcon">アイコン種別。</param>
		/// <returns>要求されたアイコン。アイコン自体の所有権はあくまでスキン側でPe側で解放処理がなされることはない。</returns>
		Icon GetIcon(SkinIcon skinIcon);

		#endregion

		#region Create

		/// <summary>
		/// 矩形画像を生成する。
		/// </summary>
		/// <param name="borderColor">境界線の色。</param>
		/// <param name="backColor">描画される色。</param>
		/// <param name="size">矩形のサイズ。</param>
		/// <returns>指定値から作成された画像</returns>
		Image CreateColorBoxImage(Color borderColor, Color backColor, Size size);

		/// <summary>
		/// ノートアイコンとしての画像を生成する。
		/// </summary>
		/// <param name="color">基本色。</param>
		/// <param name="size">矩形サイズ。</param>
		/// <returns></returns>
		Image CreateNoteBoxImage(Color color, Size size);

		#endregion

		#region Layout Toolbar

		/// <summary>
		/// ツールバーの外枠を取得する。
		/// </summary>
		/// <param name="toolbarPosition"></param>
		/// <returns></returns>
		Padding GetToolbarWindowEdgePadding(ToolbarPosition toolbarPosition);
		/// <summary>
		/// ツールバーの境界線幅を取得する。
		/// </summary>
		/// <param name="toolbarPosition"></param>
		/// <returns></returns>
		Padding GetToolbarBorderPadding(ToolbarPosition toolbarPosition);
		/// <summary>
		/// ツールバーのキャプションエリアを取得する。
		/// </summary>
		/// <param name="toolbarPosition"></param>
		/// <param name="parentSize"></param>
		/// <returns></returns>
		Rectangle GetToolbarCaptionArea(ToolbarPosition toolbarPosition, System.Drawing.Size parentSize);
		/// <summary>
		/// ツールバーの、なんだったかなぁこれ。
		/// </summary>
		/// <param name="toolbarPosition"></param>
		/// <param name="parentSize"></param>
		/// <returns></returns>
		Padding GetToolbarTotalPadding(ToolbarPosition toolbarPosition, System.Drawing.Size parentSize);
		/// <summary>
		/// ツールバーボタンのレイアウトを取得する。
		/// </summary>
		/// <param name="iconScale"></param>
		/// <param name="showText"></param>
		/// <param name="textWidth">最小値, 使用値, 最大値</param>
		/// <returns></returns>
		SkinToolbarButtonLayout GetToolbarButtonLayout(IconScale iconScale, bool showText, Tuple<int, int, int> textWidth);
		/// <summary>
		/// ツールチップのリージョン設定。
		/// </summary>
		/// <param name="target"></param>
		void ApplyToolbarToolTipRegion(Form target);

		#endregion

		#region Layout Note

		/// <summary>
		/// ノートの外枠を取得する。
		/// </summary>
		/// <returns></returns>
		Padding GetNoteWindowEdgePadding();
		/// <summary>
		/// ノートのキャプションエリアを取得する。
		/// </summary>
		/// <param name="parentSize"></param>
		/// <returns></returns>
		Rectangle GetNoteCaptionArea(System.Drawing.Size parentSize);
		/// <summary>
		/// ノートのコマンドエリアを取得する。
		/// </summary>
		/// <param name="parentArea"></param>
		/// <param name="noteCommand"></param>
		/// <returns></returns>
		Rectangle GetNoteCommandArea(System.Drawing.Rectangle parentArea, SkinNoteCommand noteCommand);

		#endregion

		#region Draw Toolbar

		void DrawToolbarWindowBackground(Graphics g, Rectangle drawArea, bool active, ToolbarPosition toolbarPosition);
		void DrawToolbarWindowEdge(Graphics g, Rectangle drawArea, bool active, ToolbarPosition toolbarPosition);
		void DrawToolbarWindowCaption(Graphics g, Rectangle drawArea, bool active, ToolbarPosition toolbarPosition);
		void DrawToolbarBackground(ToolStripRenderEventArgs e, bool active, ToolbarPosition toolbarPosition);
		void DrawToolbarBorder(ToolStripRenderEventArgs e, bool active, ToolbarPosition toolbarPosition);
		void DrawToolbarButtonImage(ToolStripItemImageRenderEventArgs e, bool active, IconScale iconScale);
		void DrawToolbarButtonText(ToolStripItemTextRenderEventArgs e, bool active, IconScale iconScale, bool showText, Tuple<int, int, int> textWidth);
		void DrawToolbarArrow(ToolStripArrowRenderEventArgs e, int menuWidth);
		void DrawToolbarDropDownButtonBackground(ToolStripItemRenderEventArgs e, ToolStripDropDownButton item, bool active, Rectangle itemArea);
		void DrawToolbarSplitButtonBackground(ToolStripItemRenderEventArgs e, ToolStripSplitButton item, bool active, Rectangle itemArea);
		void DrawToolbarButtonBackground(ToolStripItemRenderEventArgs e, ToolStripButton item, bool active, Rectangle itemArea);

		#endregion

		#region Draw Tooltip
		void DrawToolTipBackground(Graphics g, Rectangle drawArea);
		#endregion

		#region Draw Note

		void DrawNoteWindowBackground(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color backColor);
		void DrawNoteWindowEdge(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color foreColor, Color backColor);
		void DrawNoteCaption(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color foreColor, Color backColor, Font font, string caption);
		void DrawNoteCommand(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color foreColor, Color backColor, SkinNoteCommand noteCommand, SkinButtonState buttonState);
		void DrawNoteBody(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color foreColor, Color backColor);

		#endregion

		#region Property

		int MenuWidth { get; }
		int PaddingWidth { get; }

		bool IsDefaultDrawToolbarWindowBackground { get; }
		bool IsDefaultDrawToolbarWindowCaption { get; }
		bool IsDefaultDrawToolbarWindowEdge { get; }
		bool IsDefaultDrawToolbarBackground { get; }
		bool IsDefaultDrawToolbarBorder { get; }
		bool IsDefaultDrawToolbarArrow { get; }
		bool IsDefaultDrawToolbarButtonImage { get; }
		bool IsDefaultDrawToolbarButtonText { get; }
		bool IsDefaultDrawToolbarDropDownButtonBackground { get; }
		bool IsDefaultDrawToolbarSplitButtonBackground { get; }
		bool IsDefaultDrawToolbarButtonBackground { get; }
		bool IsDefaultDrawToolbarToolTipBackground { get; }

		#endregion
	}
}
