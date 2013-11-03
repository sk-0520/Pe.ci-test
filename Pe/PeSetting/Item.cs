/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/10/31
 * 時刻: 0:04
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Linq;
using System.Xml;

namespace PeSetting
{
	public delegate XmlElement XmlElementCreater(string name);
	
	/// <summary>
	/// 最少構成要素
	/// </summary>
	[Serializable]
	public class Item
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static bool IsSafeName(string name)
		{
			var matches = @"<>[]{}!#$%&'=~|^\@:,./`*?"+"\""; 
			return !name.Any(c => {
				foreach(var match in matches) {
					if(c == match) {
						return true;
					}
				}
				return false;
			});
		}
		/// <summary>
		/// 
		/// </summary>
		private string name;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public Item(string name)
		{
			this.name = name;
		}
		/// <summary>
		/// 
		/// </summary>
		public string Name { get { return this.name; } }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dg"></param>
		/// <returns></returns>
		public virtual XmlElement ExportXML(XmlElementCreater dg)
		{
			Debug.Assert(dg != null);
			
			var element = dg(Name);
			
			return element;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="xml"></param>
		public virtual void ImportXML(XmlElement element)
		{
			if(this.name != element.Name) {
				throw new Exception(string.Format("element.name({0}) is not {1}", this.name, element.Name));
			}
		}
	}
}
