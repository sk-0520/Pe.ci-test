namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Drawing;
	using System.Linq;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic;

	/// <summary>
	/// クリップボードのデータ。
	/// </summary>
	[Serializable]
	public class ClipboardItem: DisposableNameItem, IDeepClone
	{
		public ClipboardItem()
		{
			Timestamp = DateTime.Now;
			ClipboardTypes = ClipboardType.None;
		}

		/// <summary>
		/// 取り込み日時
		/// </summary>
		public DateTime Timestamp { get; set; }

		/// <summary>
		/// このアイテムが保持するクリップボードの種類。
		/// </summary>
		public ClipboardType ClipboardTypes { get; set; }
		/// <summary>
		/// 保持するプレーンテキスト。
		/// </summary>
		public string Text { get; set; }
		/// <summary>
		/// 保持するRTF。
		/// </summary>
		public string Rtf { get; set; }
		/// <summary>
		/// 保持するクリップボードとして認識可能なHTML。
		/// </summary>
		public string Html { get; set; }
		/// <summary>
		/// 保持する画像。
		/// </summary>
		[XmlIgnore]
		public Image Image { get; set; }
		[XmlElement("Image")]
		public byte[] _Image
		{
			get { return PropertyUtility.MixinImageGetter(Image); }
			set { Image = PropertyUtility.MixinImageSetter(value); }
		}

		/// <summary>
		/// 保持するファイル一覧。
		/// </summary>
		public List<string> Files { get; set; }

		#region DisposableNameItem

		protected override void Dispose(bool disposing)
		{
			Image.ToDispose();
			Image = null;
			base.Dispose(disposing);
		}

		#endregion

		#region IDeepClone

		public IDeepClone DeepClone()
		{
			var result = new ClipboardItem() {
				Name = this.Name,
				Timestamp = this.Timestamp,
				ClipboardTypes = this.ClipboardTypes,
				Text = this.Text,
				Rtf = this.Rtf,
				Html = this.Html,
				Image = this.Image == null ? null: (Image)this.Image.Clone()
			};
			if(Files != null && Files.Any()) {
				result.Files = new List<string>(Files);
			}

			return result;
		}

		#endregion

		private IEnumerable<ClipboardType> GetEnabledClipboardTypeList(IEnumerable<ClipboardType> list)
		{
			return list.Where(t => ClipboardTypes.HasFlag(t));
		}

		/// <summary>
		/// このアイテムが保持する有効なデータ種別を列挙する。
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ClipboardType> GetClipboardTypeList()
		{
			Debug.Assert(ClipboardTypes != ClipboardType.None);

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

		/// <summary>
		/// このアイテムが保持するデータ種別のうち一番優先されるものを取得。
		/// </summary>
		/// <returns></returns>
		public ClipboardType GetSingleClipboardType()
		{
			var list = new[] {
				ClipboardType.Html,
				ClipboardType.Rtf,
				ClipboardType.File,
				ClipboardType.Text,
				ClipboardType.Image,
			};
			/*
			foreach(var type in list) {
				if((ClipboardTypes & type) == type) {
					return type;
				}
			}
			*/
			return GetEnabledClipboardTypeList(list).First();
		}
	}
}
