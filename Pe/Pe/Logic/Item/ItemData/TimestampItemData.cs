/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/09/01
 * 時刻: 18:46
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Xml;
using Pe.IF;

namespace Pe.Logic
{
	/// <summary>
	/// 
	/// </summary>
	public class TimestampItemData: ItemData
	{
		const string AttributeCreate = "create";
		const string AttributeUpdate = "update";
		
		/// <summary>
		/// 
		/// </summary>
		public override string Name { get { return "timestamp"; } }
		/// <summary>
		/// 
		/// </summary>
		public DateTime Create { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime Update { get; set; }
		
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
			
			Create = DateTime.MinValue;
			Update = DateTime.MinValue;
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
						
			var create = Create.ToString("o");
			var update = Update.ToString("o");
			
			result.SetAttribute(AttributeCreate, create);
			result.SetAttribute(AttributeUpdate, update);
			
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
			
			var unsafeCreate = element.GetAttribute(AttributeCreate);
			var unsafeUpdate = element.GetAttribute(AttributeUpdate);
			
			DateTime outDateTime;
			
			if(DateTime.TryParse(unsafeCreate, out outDateTime)) {
				Create = outDateTime;
			}
			if(DateTime.TryParse(unsafeUpdate, out outDateTime)) {
				Update = outDateTime;
			}
		}
	}
}
