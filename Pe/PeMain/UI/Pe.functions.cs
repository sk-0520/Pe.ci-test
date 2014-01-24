/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/16
 * 時刻: 23:29
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
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
		IEnumerable<Form> GetWindows()
		{
			var result = new List<Form>();
			result.Add(this._toolbarForm);
			result.Add(this._logForm);
			
			if(this._toolbarForm.OwnedForms.Length > 0) {
				result.AddRange(this._toolbarForm.OwnedForms);
			}
			
				
			return result;
		}
		/// <summary>
		/// TODO: 未実装
		/// </summary>
		/// <param name="action"></param>
		void PauseOthers(Action action)
		{
			var windowVisible = new Dictionary<Form, bool>();
			foreach(var window in GetWindows()) {
				windowVisible[window] = window.Visible;
				window.Visible = false;
			}
			
			action();
			
			foreach(var pair in windowVisible) {
				pair.Key.Visible = pair.Value;
			}
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
		
		void OpenSetting()
		{
			using(var settingForm = new SettingForm(this._language, this._mainSetting)) {
				if(settingForm.ShowDialog() == DialogResult.OK) {
					var mainSetting = settingForm.MainSetting;
					this._mainSetting = mainSetting;
					InitializeLanguage(null, null);
					this._logForm.SetSettingData(this._language, this._mainSetting);
					this._toolbarForm.SetSettingData(this._language, this._mainSetting);
				}
			}
		}
	}
}
