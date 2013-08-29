/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 08/29/2013
 * 時刻: 23:22
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Text.RegularExpressions;
using System.Xml;

using Pe.IF;

namespace Pe.Logic
{
	public abstract class Item: IImportExportXmlElement
	{
		static Regex _reg = new Regex(
			""
			+ "([" 
			+ @"\s\\\^\$\(\)\|\.\[\-\]\*\+\?\{\,\}" 
			+ "\"'=~@<>;:!#%&" 
			+ "])"
			, 
			RegexOptions.None
		);
		
		const string _id = "id";
		
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
		
		public Item()
		{
			
		}
		
		/// <summary>
		/// 
		/// </summary>
		public virtual string Id 
		{ 
			get { return this.id; }
			set 
			{
				if(this.id != null && !IsSafeId(value)) {
					throw new Exception(id);
				}
				this.id = value;  
			}
		}
		
		/// <summary>
		/// XML要素出力。
		/// 
		/// メソッドをオーバーライドする場合、スーパークラスのメソッド戻り値を使用すること。
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		public virtual XmlElement ToXmlElement(XmlDocument xml, ExportArgs exExport)
		{
			var result = xml.CreateElement(_id);
			
			result.SetAttribute(_id, Id);
			
			return result;
		}
		
		/// <summary>
		/// XML要素入力
		/// 
		/// メソッドをオーバーライドする場合、スーパークラスから先に呼び出すこと。
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		public virtual void FromXmlElement(XmlElement element)
		{
			var id = element.GetAttribute(_id);
			Id = id;
		}
	}
}
