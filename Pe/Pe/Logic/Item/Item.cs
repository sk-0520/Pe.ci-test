/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 08/29/2013
 * 時刻: 23:22
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml;
using Pe.IF;
using ShareLib;

namespace Pe.Logic
{
	public abstract class Item: ItemBase
	{
		const string AttributeId = "id";
		
		static Regex _reg = new Regex(
			""
			+ "([" 
			+ @"\s\\\^\$\(\)\|\.\[\-\]\*\+\?\{\,\}" 
			+ "\"'=~@<>;:!#%&" 
			+ "])"
			, 
			RegexOptions.None
		);
		
		public static string UniqueItemId(string source, int index) 
		{
			return string.Format("{0}_{1}", source, index);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static bool IsSafeId(string id)
		{
			if(id.Length == 0) {
				return false;
			}
			return !_reg.Match(id).Success;
		}
		public static string ToSafeId(string id) 
		{
			var result = _reg.Replace(id, "_");
			return result;
		}
		
		private string id;
		
		public Item() { }
		
		public sealed override string Name { get { return "item"; } }
		
		/// <summary>
		/// 
		/// </summary>
		public string Id 
		{ 
			get { return this.id; }
			set 
			{
				if(!IsSafeId(value)) {
					throw new PeException(value);
				}
				this.id = value;  
			}
		}
		
		public string Comment { get; set; }
		
		/// <summary>
		/// XML要素出力。
		/// 
		/// メソッドをオーバーライドする場合、スーパークラスのメソッド戻り値を使用すること。
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		public override XmlElement ToXmlElement(XmlDocument xml, ExportArgs expArg)
		{
			var result = base.ToXmlElement(xml, expArg);
			
			result.SetAttribute(AttributeId, Id);
			
			if(Comment.IsEmpty()) {
				var commentElement = xml.CreateComment(Comment);
				result.AppendChild(commentElement);
			}
			
			return result;
		}
		
		/// <summary>
		/// XML要素入力
		/// 
		/// メソッドをオーバーライドする場合、スーパークラスから先に呼び出すこと。
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		public override void FromXmlElement(XmlElement element, ImportArgs impArg)
		{
			base.FromXmlElement(element, impArg);
			
			var id = element.GetAttribute(AttributeId);
			Id = id;
			
			foreach(XmlNode node in element.ChildNodes) {
				if(node.NodeType == XmlNodeType.Comment) {
					Comment = ((XmlComment)node).Data;
					break;
				}
			}
		}
	}

	public class ItemContainer<TItem>
		where TItem: Item
	{		
		/// <summary>
		/// 
		/// </summary>
		SortedDictionary<string, TItem> map = new SortedDictionary<string, TItem>();
		
		/// <summary>
		/// 
		/// </summary>
		public ItemContainer()
		{
		}
		
		public int Count 
		{
			get { return this.map.Count; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public virtual TItem this[string id] 
		{
			get 
			{
				return this.map[id];
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SortedDictionary<string, TItem>.KeyCollection Ids
		{
			get { return this.map.Keys; }
		}
		/// <summary>
		/// 
		/// </summary>
		public SortedDictionary<string, TItem>.ValueCollection Items
		{
			get { return this.map.Values; }
		}
		
		public void Clear()
		{
			this.map.Clear();
		}
		
		public void Valid()
		{
			var list = new List<string>();
			foreach(var data in this.map)
			{
				if(data.Key != data.Value.Id) {
					list.Add(data.Key);
				} else if(!Item.IsSafeId(data.Key)) {
					list.Add(data.Key);
				}
			}
			if(list.Count > 0) {
				throw new Exception(string.Join(Environment.NewLine, list));
			}
		}
		
		public bool ContainsId(string id)
		{
			return this.map.ContainsKey(id);
		}
		
		public bool TryGetItem(string id, out TItem value)
		{
			return this.map.TryGetValue(id, out value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		public void Set(TItem item)
		{
			this.map[item.Id] = item;
		}
		
		public void SetRange(IEnumerable<TItem> items)
		{
			foreach(var item in items) {
				Set(item);
			}
		}
		
		public void ChangeId(string fromId, string toId)
		{
			Debug.Assert(this.map.ContainsKey(fromId));
			Debug.Assert(!this.map.ContainsKey(toId));
			
			var tempItem = this.map[fromId];
			this.map.Remove(fromId);
			tempItem.Id = toId;
			this.Set(tempItem);
		}
		
		public TItem Remove(string itemId)
		{
			var item = this.map[itemId];
			this.map.Remove(itemId);
			return item;
		}
		
		public TItem Remove(TItem item)
		{
			return Remove(item.Id);
		}
		
	}

}
