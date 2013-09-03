/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/09/02
 * 時刻: 2:01
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
	public abstract class ItemBase: IImportExportXmlElement, IDataClean
	{
		/// <summary>
		/// 
		/// </summary>
		protected ItemBase()
		{
			Initialize();
			Clear();
		}
		/// <summary>
		/// アイテム名
		/// 
		/// タグ名として機能
		/// </summary>
		public virtual string Name
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		
		/// <summary>
		/// データ初期化
		/// </summary>
		protected virtual void Initialize()
		{
			
		}
		
		/// <summary>
		/// 
		/// </summary>
		public virtual void Clear() { }
		
		/// <summary>
		/// XML要素出力。
		/// 
		/// メソッドをオーバーライドする場合、スーパークラスのメソッド戻り値を使用すること。
		/// </summary>
		/// <param name="xml"></param>
		/// <param name="expArg"></param>
		/// <returns></returns>
		public virtual XmlElement ToXmlElement(XmlDocument xml, ExportArgs expArg)
		{
			var result = xml.CreateElement(Name);
			
			return result;
		}
		
		/// <summary>
		/// XML要素入力
		/// 
		/// メソッドをオーバーライドする場合、スーパークラスから先に呼び出すこと。
		/// </summary>
		/// <param name="element"></param>
		/// <param name="impArg"></param>
		public virtual void FromXmlElement(XmlElement element, ImportArgs impArg) 
		{
			Clear();
		}
	}
}
