using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.PeMain.IF
{
	public interface IClipboardWatcher
	{
		/// <summary>
		/// クリップボード監視の設定
		/// </summary>
		/// <param name="watching">真の場合に監視する</param>
		void WatcheClipboard(bool watching);

	}
}
