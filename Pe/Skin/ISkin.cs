namespace ContentTypeTextNet.Pe.Library.Skin
{
	using System.Drawing;
	using System.Windows.Forms;

	/// <summary>
	///スキン
	/// </summary>
	public interface ISkin
	{
		#region Initialize

		/// <summary>
		/// 必要最低限の準備を行う。
		/// </summary>
		void Load();
		/// <summary>
		/// 
		/// </summary>
		void Unload();
		/// <summary>
		/// スキンとして処理可能な状態まで初期化する。
		/// 
		/// Load後に呼び出される。
		/// </summary>
		void Initialize();
		/// <summary>
		/// スキンを初期状態に戻す。
		/// 
		/// インスタンスとして有効であっても再使用には Load → Initialize が必要。
		/// </summary>
		#endregion

		#region Setting About

		ISkinAbout About { get; }

		#endregion

		#region Style

		/// <summary>
		/// 指定フォームにスタイルを適用する。
		/// </summary>
		/// <param name="target"></param>
		void AttachmentStyle(Form target, SkinWindow skinWindow);
		/// <summary>
		/// 指定フォームのスタイルを再生委呈する
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

		Image GetImage(SkinImage skinImage);
		Icon GetIcon(SkinIcon skinIcon);

		#endregion

		#region Create

		Image CreateColorBoxImage(Color borderColor, Color backColor, Size size);
		Image CreateNoteBoxImage(Color color, Size size);

		#endregion

		#region Layout Toolbar

		Padding GetToolbarWindowEdgePadding(ToolbarPosition toolbarPosition);
		Padding GetToolbarBorderPadding(ToolbarPosition toolbarPosition);
		Rectangle GetToolbarCaptionArea(ToolbarPosition toolbarPosition, System.Drawing.Size parentSize);
		Padding GetToolbarTotalPadding(ToolbarPosition toolbarPosition, System.Drawing.Size parentSize);
		SkinToolbarButtonLayout GetToolbarButtonLayout(IconScale iconSize, bool showText, int textWidth);
		void ApplyToolbarToolTipRegion(Form target);

		#endregion

		#region Layout Note

		Padding GetNoteWindowEdgePadding();
		Rectangle GetNoteCaptionArea(System.Drawing.Size parentSize);
		Rectangle GetNoteCommandArea(System.Drawing.Rectangle parentArea, SkinNoteCommand noteCommand);

		#endregion

		#region Draw Toolbar

		void DrawToolbarWindowBackground(Graphics g, Rectangle drawArea, bool active, ToolbarPosition toolbarPosition);
		void DrawToolbarWindowEdge(Graphics g, Rectangle drawArea, bool active, ToolbarPosition toolbarPosition);
		void DrawToolbarWindowCaption(Graphics g, Rectangle drawArea, bool active, ToolbarPosition toolbarPosition);
		void DrawToolbarBackground(ToolStripRenderEventArgs e, bool active, ToolbarPosition toolbarPosition);
		void DrawToolbarBorder(ToolStripRenderEventArgs e, bool active, ToolbarPosition toolbarPosition);
		void DrawToolbarButtonImage(ToolStripItemImageRenderEventArgs e, bool active, IconScale iconScale);
		void DrawToolbarButtonText(ToolStripItemTextRenderEventArgs e, bool active, IconScale iconScale, bool showText, int textWidth);
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
