using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeSkin
{
	/// <summary>
	/// ノートアイテムに対して目に見えてなんかする。
	/// </summary>
	public enum SkinNoteCommand
	{
		/// <summary>
		/// 閉じる。
		/// </summary>
		Close,
		/// <summary>
		/// 最小化。
		/// </summary>
		Compact,
		/// <summary>
		/// 最前面。
		/// </summary>
		Topmost,
		/// <summary>
		/// 固定。
		/// </summary>
		Lock,
	}
}
