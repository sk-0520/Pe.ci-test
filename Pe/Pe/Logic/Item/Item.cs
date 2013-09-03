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
}
