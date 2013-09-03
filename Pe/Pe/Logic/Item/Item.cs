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
	/// <summary>
	/// 
	/// </summary>
	public abstract class Item: ItemBase
	{
		const string AttributeId = "id";
		
		/// <summary>
		/// 
		/// </summary>
		public static string TagName { get { return "item"; } }
		
		static Regex _reg = new Regex(
			""
			+ "([" 
			+ @"\s\\\^\$\(\)\|\.\[\-\]\*\+\?\{\,\}" 
			+ "\"'=~@<>;:!#%&" 
			+ "])"
			, 
			RegexOptions.None
		);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="source"></param>
		/// <param name="index"></param>
		/// <returns></returns>
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
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static string ToSafeId(string id) 
		{
			var result = _reg.Replace(id, "_");
			return result;
		}
		
		private string id;
		
		/// <summary>
		/// 
		/// </summary>
		public sealed override string Name { get { return TagName; } }
		
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
		/// <summary>
		/// 
		/// </summary>
		public string Comment { get; set; }
		
		/// <summary>
		/// 
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();
		}
		
		/// <summary>
		/// 
		/// </summary>
		public override void Clear()
		{
			base.Clear();
			
			this.id = default(string);
			Comment = default(string);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="xml"></param>
		/// <param name="expArg"></param>
		/// <returns></returns>
		public override XmlElement ToXmlElement(XmlDocument xml, ExportArgs expArg)
		{
			var result = base.ToXmlElement(xml, expArg);
			
			result.SetAttribute(AttributeId, Id);
			
			if(!Comment.IsEmpty()) {
				var commentElement = xml.CreateComment(Comment);
				result.AppendChild(commentElement);
			}
			
			return result;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="element"></param>
		/// <param name="impArg"></param>
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

	/// <summary>
	/// 
	/// </summary>
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
		
		/// <summary>
		/// 
		/// </summary>
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
		
		/// <summary>
		/// 
		/// </summary>
		public void Clear()
		{
			this.map.Clear();
		}
		
		/// <summary>
		/// 
		/// </summary>
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
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool ContainsId(string id)
		{
			return this.map.ContainsKey(id);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="value"></param>
		/// <returns></returns>
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
		/// <summary>
		/// 
		/// </summary>
		/// <param name="items"></param>
		public void SetRange(IEnumerable<TItem> items)
		{
			foreach(var item in items) {
				Set(item);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fromId"></param>
		/// <param name="toId"></param>
		public void ChangeId(string fromId, string toId)
		{
			Debug.Assert(this.map.ContainsKey(fromId));
			Debug.Assert(!this.map.ContainsKey(toId));
			
			var tempItem = this.map[fromId];
			this.map.Remove(fromId);
			tempItem.Id = toId;
			this.Set(tempItem);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="itemId"></param>
		/// <returns></returns>
		public TItem Remove(string itemId)
		{
			var item = this.map[itemId];
			this.map.Remove(itemId);
			return item;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public TItem Remove(TItem item)
		{
			return Remove(item.Id);
		}
		
	}

}
