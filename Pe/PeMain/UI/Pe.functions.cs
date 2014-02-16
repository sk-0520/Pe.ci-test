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
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using PeMain.Data;
using PeMain.Logic;
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
			result.AddRange(this._toolbarForms.Values);
			result.Add(this._logForm);
			
			foreach(var f in this._toolbarForms.Values.Where(f => f.OwnedForms.Length > 0)) {
				result.AddRange(f.OwnedForms);
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
			this._notifyIcon.Visible = false;
			
			action();

			foreach(var pair in windowVisible) {
				pair.Key.Visible = pair.Value;
			}
			this._notifyIcon.Visible = true;
		}
		
		void SaveSetting()
		{
			var mainSettingFilePath = Literal.UserMainSettingPath;
			SaveMainSetting(this._commonData.MainSetting, mainSettingFilePath);
		}
		
		void SaveMainSetting(MainSetting mainSetting, string mainSettingPath)
		{
			Debug.Assert(mainSetting != null);
			FileUtility.MakeFileParentDirectory(mainSettingPath);

			using(var stream = new FileStream(mainSettingPath, FileMode.Create)) {
				var sortedSet = new HashSet<LauncherItem>();
				foreach(var item in mainSetting.Launcher.Items.OrderBy(item => item.Name)) {
					sortedSet.Add(item);
				}
				var nowItems = mainSetting.Launcher.Items;
				mainSetting.Launcher.Items = sortedSet;
				var serializer = new XmlSerializer(typeof(MainSetting));
				serializer.Serialize(stream, mainSetting);
				mainSetting.Launcher.Items = nowItems;
			}
		}
		
		void CloseApplication(bool save)
		{
			SaveSetting();
			Application.Exit();
		}
		
		void OpenSetting()
		{
			using(var settingForm = new SettingForm(this._commonData.Language, this._commonData.MainSetting)) {
				if(settingForm.ShowDialog() == DialogResult.OK) {
					var mainSetting = settingForm.MainSetting;
					this._commonData.MainSetting = mainSetting;
					SaveSetting();
					InitializeLanguage(null, null);
					ApplyLanguage();
					this._logForm.SetCommonData(this._commonData);
					foreach(var toolbar in this._toolbarForms.Values) {
						toolbar.SetCommonData(this._commonData);
					}
				}
			}
		}
		
		void ChangeShowSysEnv(Func<bool> nowValueDg, Action<bool> changeValueDg, string messageTitleName, string showMessageName, string hiddenMessageName, string errorMessageName)
		{
			var prevValue = nowValueDg();
			changeValueDg(!prevValue);
			var nowValue = nowValueDg();
			SystemEnv.RefreshShell();
			
			ToolTipIcon icon;
			string messageName;
			if(prevValue != nowValue) {
				if(nowValue) {
					messageName = showMessageName;
				} else {
					messageName = hiddenMessageName;
				}
				icon = ToolTipIcon.Info;
			} else {
				messageName = errorMessageName;
				icon = ToolTipIcon.Error;
			}
			var title = this._commonData.Language[messageTitleName];
			var message = this._commonData.Language[messageName];
			if(icon == ToolTipIcon.Error) {
				this._commonData.Logger.Puts(LogType.Error, title, message);
			}
			ShowBalloon(icon, title, message);
		}
	}
}
