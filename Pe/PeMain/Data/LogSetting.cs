namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Drawing;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Kind;

	/// <summary>
	/// ログ設定。
	/// </summary>
	[Serializable]
	public class LogSetting: Item, ICloneable
	{
		public LogSetting()
		{
			Size = new Size(
				Screen.PrimaryScreen.Bounds.Width / 4,
				Screen.PrimaryScreen.Bounds.Height / 2
			);
			var screenSize = Screen.PrimaryScreen.WorkingArea.Size;
			Point = new Point(screenSize.Width - Size.Width, screenSize.Height - Size.Height);
			AddShow = true;
			AddShowTrigger = LogType.Warning | LogType.Error;

			Debugging = false;
#if DEBUG
			Debugging = true;
#endif
		}
		
		/// <summary>
		/// ログダイアログの表示状態。
		/// </summary>
		public bool Visible { get; set; }
		/// <summary>
		/// ログダイアログの位置。
		/// </summary>
		public Point Point { get; set; }
		/// <summary>
		/// ログダイアログのサイズ。
		/// </summary>
		public Size Size { get; set; }
		/// <summary>
		/// ログ追加時にログダイアログを表示するか。
		/// </summary>
		public bool AddShow { get; set; }
		/// <summary>
		/// ログダイアログを表示する際にどの種別で表示するか。
		/// </summary>
		public LogType AddShowTrigger { get; set; }

		/// <summary>
		/// デバッグ用出力を行うか。
		/// </summary>
		public bool Debugging { get; set; }
		
		/// <summary>
		/// 詳細部を全面表示。
		/// </summary>
		public bool FullDetail { get; set; }

		#region ICloneable

		public object Clone()
		{
			return new LogSetting() {
				Visible = this.Visible,
				Point = this.Point,
				Size = this.Size,
				AddShow = this.AddShow,
				AddShowTrigger = this.AddShowTrigger,
				Debugging = this.Debugging,
				FullDetail = this.FullDetail,
			};
		}

		#endregion
	}
}
