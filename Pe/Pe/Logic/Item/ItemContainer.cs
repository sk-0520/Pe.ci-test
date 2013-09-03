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
	public class ItemContainer<TItem>: ItemBase
		where TItem: Item, new()
	{
		/// <summary>
		/// 
		/// </summary>
		private SortedDictionary<string, TItem> map = null;
		private string name;
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public ItemContainer(string name)
		{
			this.name = name;
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
		/// データアクセス。
		/// 
		/// 添え字とアイテムIDの相違を避けるためsetterは未実装なのでSetメソッドを使用する。
		/// </summary>
		public SortedDictionary<string, TItem>.ValueCollection Items
		{
			get { return this.map.Values; }
		}
		
		/// <summary>
		/// 
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();
			
			this.map = new SortedDictionary<string, TItem>();
		}
		/// <summary>
		/// 
		/// </summary>
		public override void Clear()
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
		/// <summary>
		/// 
		/// </summary>
		/// <param name="xml"></param>
		/// <param name="expArg"></param>
		/// <returns></returns>
		public override XmlElement ToXmlElement(XmlDocument xml, ExportArgs expArg)
		{
			var result = base.ToXmlElement(xml, expArg);
			
			foreach(var item in Items) {
				var itemElement = item.ToXmlElement(xml, expArg);
				result.AppendChild(itemElement);
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
			var list = new List<TItem>(element.ChildNodes.Count);
			var hasChild = false;
			
			foreach(XmlElement itemElement in element.GetElementsByTagName(Item.TagName)) {
				var item = new TItem();
				item.FromXmlElement(itemElement, impArg);
				list.Add(item);
				
				hasChild = true;
			}
			if(hasChild) {
				SetRange(list);
			}
		}
	}
}
