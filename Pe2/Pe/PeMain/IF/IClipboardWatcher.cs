namespace ContentTypeTextNet.Pe.PeMain.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public interface IClipboardWatcher
	{
		/// <summary>
		/// クリップボード監視の設定。
		/// </summary>
		/// <param name="watch">真の場合に監視する</param>
		void ClipboardWatchingChange(bool watch);
		/// <summary>
		/// 監視しているか。
		/// </summary>
		bool ClipboardWatching { get; }
		/// <summary>
		/// クリップボード
		/// </summary>
		bool ClipboardEnabledApplicationCopy { get; }
	}
}
