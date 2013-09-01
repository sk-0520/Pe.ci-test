/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/09/01
 * 時刻: 10:51
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Pe.IF;
using ShareLib;

namespace Pe.Logic
{
	public abstract class TitleItem: Item
	{
		const string AttributeTitle = "title";
		
		public static bool IsSafeTitle(string s)
		{
			return s.SplitLines().Count() == 1;
		}
		public static string ToSafeTitle(string s)
		{
			return string.Join("-", s.SplitLines());
		}
		
		private string title;
		
		public TitleItem()
		{
		}
		
		/// <summary>
		/// アイテム名
		/// </summary>
		public string Title { 
			get { return this.title; }
			set
			{
				if(!IsSafeTitle(value)) {
					throw new PeException(value);
				}
				this.title = value;
			}
		}
		
		public override XmlElement ToXmlElement(XmlDocument xml, ExportArgs expArg)
		{
			var result = base.ToXmlElement(xml, expArg);
			
			result.SetAttribute(AttributeTitle, Title);
			
			return result;
		}
		
		public override void FromXmlElement(XmlElement element, ImportArgs impArg)
		{
			base.FromXmlElement(element, impArg);
			
			var title = element.GetAttribute(AttributeTitle);
			Title = title;
		}
	}
}
