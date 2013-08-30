/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 08/29/2013
 * 時刻: 23:25
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Xml;

namespace Pe.IF
{
	public abstract class ImportArgs { }
	
	/// <summary>
	/// 
	/// </summary>
	public interface IImportXmlElement
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		void FromXmlElement(XmlElement element, ImportArgs impArg);
	}
	
	/// <summary>
	/// 
	/// </summary>
	public abstract class ExportArgs { }
	
	/// <summary>
	/// 
	/// </summary>
	public interface IExportXmlElement
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		XmlElement ToXmlElement(XmlDocument xml, ExportArgs expArg);
	}
	
	/// <summary>
	/// 
	/// </summary>
	public interface IImportExportXmlElement: IImportXmlElement, IExportXmlElement { }

}
