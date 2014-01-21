/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/16
 * 時刻: 23:29
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

using PeMain.Data;
using PeUtility;

namespace PeMain.UI
{
	/// <summary>
	/// Description of Pe_functions.
	/// </summary>
	public partial class Pe
	{
		/// <summary>
		/// TODO: 未実装
		/// </summary>
		/// <param name="action"></param>
		void PauseOthers(Action action)
		{
			action();
		}
		
		void SaveSetting()
		{
			var mainSettingFilePath = Literal.UserMainSettingPath;
			SaveMainSetting(this._mainSetting, mainSettingFilePath);
		}
		
		void SaveMainSetting(MainSetting mainSetting, string mainSettingPath)
		{
			Debug.Assert(mainSetting != null);
			FileUtility.MakeFileParentDirectory(mainSettingPath);

			var serializer = new XmlSerializer(typeof(MainSetting));
			using(var stream = new FileStream(mainSettingPath, FileMode.Create)) {
				serializer.Serialize(stream, mainSetting);
			}
		}
		
		void CloseApplication(bool save)
		{
			SaveSetting();
			Application.Exit();
		}
		
	}
}
