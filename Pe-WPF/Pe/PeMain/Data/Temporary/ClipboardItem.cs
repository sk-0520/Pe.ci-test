namespace ContentTypeTextNet.Pe.PeMain.Data.Temporary
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	public class ClipboardItem
	{
		public ClipboardItem()
		{
			Type = ClipboardType.None;
			Body = new ClipboardBodyItemModel();
		}

		#region property

		public ClipboardType Type { get; set; }
		public ClipboardBodyItemModel Body { get; set; }

		#endregion

		#region function

		private IEnumerable<ClipboardType> GetEnabledClipboardTypeList(IEnumerable<ClipboardType> list)
		{
			return list.Where(t => Type.HasFlag(t));
		}

		/// <summary>
		/// このアイテムが保持する有効なデータ種別を列挙する。
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ClipboardType> GetClipboardTypeList()
		{
			Debug.Assert(Type != ClipboardType.None);

			var list = new[] {
				ClipboardType.Text,
				ClipboardType.Rtf,
				ClipboardType.Html,
				ClipboardType.Image,
				ClipboardType.File,
			};
			/*
			foreach(var type in list) {
				if((ClipboardTypes & type) == type) {
					yield return type;
				}
			}
			*/
			return GetEnabledClipboardTypeList(list);
		}


		#endregion

	}
}
