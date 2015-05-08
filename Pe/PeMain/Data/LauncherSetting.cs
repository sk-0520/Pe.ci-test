namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Drawing;
	using System.IO;
	using System.Linq;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Pe.Library.Skin;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Kind;

	/// <summary>
	/// ランチャーアイテム統括。
	/// </summary>
	[Serializable]
	public class LauncherSetting: DisposableItem, IDisposable, IDeepClone
	{
		public LauncherSetting()
		{
			Items = new HashSet<LauncherItem>();
		}
		
		/// <summary>
		/// 各ランチャアイテム
		/// </summary>
		[XmlIgnoreAttribute()]
		public HashSet<LauncherItem> Items { get; set; }

		#region DisposableItem

		protected override void Dispose(bool disposing)
		{
			foreach(var item in Items) {
				item.ToDispose();
			}

			base.Dispose(disposing);
		}

		#endregion

		#region override

		public override void CorrectionValue()
		{
			base.CorrectionValue();
		}

		#endregion

		#region IDeepClone

		public IDeepClone DeepClone()
		{
			var result = new LauncherSetting();

			foreach(var item in Items) {
				result.Items.Add((LauncherItem)item.Clone());
			}

			return result;
		}

		#endregion
	}
}
