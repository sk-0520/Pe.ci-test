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
using System.IO.Compression;
using System.IO.Packaging;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

using PeMain.Data;
using PeMain.Logic;
using PeUtility;
using PInvoke.Windows;

namespace PeMain.UI
{
	/// <summary>
	/// Description of Pe_functions.
	/// </summary>
	public partial class Pe
	{
		#if DEBUG
		public void DebugProcess()
		{
			/*
			var db = this._commonData.Database;
			
			using(var tran = db.BeginTransaction()) {
				var entity = new PeMain.Data.DB.MNoteEntity();
				db.ExecuteDelete(new [] { entity } );
				db.ExecuteInsert(new [] { entity } );
				tran.Commit();
			}
			
			using(var reader = db.ExecuteReader("select * from M_NOTE")) {
				while(reader.Read()) {
					for(var i=0; i < reader.FieldCount;i ++) {
						var name = reader.GetName(i);
						var value= reader[name];
						Debug.WriteLine("{0} = {1}", name, value);
					}
				}
			}
			using(var tran = db.BeginTransaction()) {
				var entity = new PeMain.Data.DB.MNoteEntity();
				entity.Title = "a'b--c";
				db.ExecuteUpdate(new [] { entity } );
				tran.Commit();
			}
			using(var tran = db.BeginTransaction()) {
				var entity = new PeMain.Data.DB.MNoteEntity();
				entity.Id = 1;
				db.ExecuteInsert(new [] { entity } );
				tran.Rollback();
			}
			using(var reader = db.ExecuteReader("select * from M_NOTE")) {
				while(reader.Read()) {
					for(var i=0; i < reader.FieldCount;i ++) {
						var name = reader.GetName(i);
						var value= reader[name];
						Debug.WriteLine("{0} = {1}", name, value);
					}
				}
			}
			var e2 = new PeMain.Data.DB.MNoteEntity();
			var e3 = db.GetEntity(e2);
			Debug.WriteLine(e2.Title);
			Debug.WriteLine(e3.Title);
			//*/
			
			/*
			var note = new NoteForm();
			note.SetCommonData(this._commonData);
			note.Show();
			//*/
		}
		#endif
		
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
		/// <param name="func">ウィンドウ再構築を独自に行う場合は処理を返す。</param>
		void PauseOthers(Func<Action> func)
		{
			var windowVisible = new Dictionary<Form, bool>();
			foreach(var window in GetWindows()) {
				windowVisible[window] = window.Visible;
				window.Visible = false;
			}
			this._notifyIcon.Visible = false;
			
			this._pause = true;
			var action = func();
			this._pause = false;

			if(action != null) {
				action();
			} else {
				foreach(var pair in windowVisible) {
					pair.Key.Visible = pair.Value;
				}
			}
			
			this._notifyIcon.Visible = true;
		}
		
		void BackupSetting(IEnumerable<string> targetFiles, string saveDirPath, int count)
		{
			var enabledFiles = targetFiles.Where(s => File.Exists(s));
			if(enabledFiles.Count() == 0) {
				return;
			}
			
			// バックアップ世代交代
			if(Directory.Exists(saveDirPath)) {
				foreach(var path in Directory.GetFileSystemEntries(saveDirPath).OrderByDescending(s => Path.GetFileName(s)).Skip(count - 1)) {
					try {
						File.Delete(path);
					} catch(Exception ex) {
						this._commonData.Logger.Puts(LogType.Error, ex.Message, ex);
					}
				}
			}
			
			var fileName = DateTime.Now.ToString(Literal.timestampFileName) + ".zip";
			var saveFilePath = Path.Combine(saveDirPath, fileName);
			FileUtility.MakeFileParentDirectory(saveFilePath);
			
			// zip
			using(var stream = new FileStream(saveFilePath, FileMode.Create)) {
				using(var zip = new ZipArchive(stream, ZipArchiveMode.Create)) {
					foreach(var filePath in enabledFiles) {
						var entry = zip.CreateEntry(Path.GetFileName(filePath));
						using(var entryStream = new BinaryWriter(entry.Open())) {
							var buffer = File.ReadAllBytes(filePath);
							entryStream.Write(buffer);
						}
					}
				}
			}

		}
		
		void SaveSetting()
		{
			var mainSettingFilePath = Literal.UserMainSettingPath;
			var launcherItemsPath = Literal.UserLauncherItemsPath;
			
			// バックアップ
			var backupFiles = new [] {
				mainSettingFilePath,
				launcherItemsPath,
			};
			BackupSetting(backupFiles, Literal.UserBackupDirPath, Literal.backupCount);
			
			// 保存開始
			SaveSerialize(this._commonData.MainSetting, mainSettingFilePath);
			
			var sortedSet = new HashSet<LauncherItem>();
			foreach(var item in this._commonData.MainSetting.Launcher.Items.OrderBy(item => item.Name)) {
				sortedSet.Add(item);
			}
			SaveSerialize(sortedSet, launcherItemsPath);
		}
		
		
		static T LoadDeserialize<T>(string path, bool failToNew)
			where T: new()
		{
			if(File.Exists(path)) {
				var serializer = new XmlSerializer(typeof(T));
				using(var stream = new FileStream(path, FileMode.Open)) {
					return (T)serializer.Deserialize(stream);
				}
			}
			if(failToNew) {
				return new T();
			} else {
				return default(T);
			}
		}

		static void SaveSerialize<T>(T saveData, string savePath)
		{
			Debug.Assert(saveData != null);
			FileUtility.MakeFileParentDirectory(savePath);

			using(var stream = new FileStream(savePath, FileMode.Create)) {
				var serializer = new XmlSerializer(typeof(T));
				serializer.Serialize(stream, saveData);
			}
			
		}
		
		void CloseApplication(bool save)
		{
			SaveSetting();
			if(this._commonData.Database != null) {
				this._commonData.Database.Close();
			}
			Application.Exit();
		}
		
		Action OpenSetting()
		{
			using(var settingForm = new SettingForm(this._commonData.Language, this._commonData.MainSetting)) {
				if(settingForm.ShowDialog() == DialogResult.OK) {
					var mainSetting = settingForm.MainSetting;
					this._commonData.MainSetting = mainSetting;
					SaveSetting();
					InitializeLanguage(null, null, null);
					ApplyLanguage();
					
					return delegate() {
						this._logForm.SetCommonData(this._commonData);
						this._messageWindow.SetCommonData(this._commonData);
						foreach(var toolbar in this._toolbarForms.Values) {
							toolbar.SetCommonData(this._commonData);
						}
					};
				}
			}
			return null;
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
		
		public void ReceiveHotKey(HotKeyId hotKeyId, MOD mod, Keys key)
		{
			if(this._pause) {
				return;
			}
			
			switch(hotKeyId) {
				case HotKeyId.HiddenFile:
					ChangeShowSysEnv(SystemEnv.IsHiddenFileShow, SystemEnv.SetHiddenFileShow, "balloon/hidden-file/title", "balloon/hidden-file/show", "balloon/hidden-file/hide", "balloon/hidden-file/error");
					break;
					
				case HotKeyId.Extension:
					ChangeShowSysEnv(SystemEnv.IsExtensionShow, SystemEnv.SetExtensionShow, "balloon/extension/title", "balloon/extension/show", "balloon/extension/hide", "balloon/extension/error");
					break;
					
				default:
					break;
			}
		}

	}
}