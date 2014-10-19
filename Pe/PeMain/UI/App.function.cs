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
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.IO.Packaging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using PeMain.Data;
using PeMain.Logic;
using PeMain.Logic.DB;
using PeUtility;
using PInvoke.Windows;

namespace PeMain.UI
{
	/// <summary>
	/// Description of Pe_functions.
	/// </summary>
	partial class App
	{
		[Conditional("DEBUG")]
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
			
			/*
			var info = new PeInformation();
			foreach(var g in info.Get()) {
				Debug.WriteLine(string.Format("[ {0} ]============", g.Title));
				foreach(var item in g.Items) {
					Debug.WriteLine(string.Format("{0} = {1}", item.Key, item.Value));
				}
			}
			//*/
		}
		
		/// <summary>
		/// 保持するウィンドウ(Form)をすべて取得する。
		/// </summary>
		/// <returns></returns>
		IEnumerable<Form> GetWindows()
		{
			var result = new List<Form>();
			result.AddRange(this._toolbarForms.Values);
			result.Add(this._logForm);
			
			foreach(var f in this._toolbarForms.Values.Where(f => f.OwnedForms.Length > 0)) {
				result.AddRange(f.OwnedForms);
			}
			
			result.AddRange(this._noteWindowList);

			return result;
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="func">ウィンドウ再構築を独自に行う場合は真を返す処理を返す。</param>
		void PauseOthers(Func<Func<bool>> func)
		{
			var recursion = this._pause;
			var windowVisible = new Dictionary<Form, bool>();
			if(!recursion) {
				foreach(var window in GetWindows()) {
					windowVisible[window] = window.Visible;
					window.Visible = false;
				}
				this._notifyIcon.Visible = false;
				
				this._pause = true;
			}
			var action = func();
			var customWindow = false;
			if(action != null) {
				customWindow = action();
			}
			if(!recursion) {
				if(!customWindow) {
					foreach(var pair in windowVisible) {
						// すでに表示している場合はポーズ中に処理が走ったため復帰は無視する
						if(!pair.Key.Visible) {
							pair.Key.Visible = pair.Value;
						}
					}
				}
				this._pause = false;
				this._notifyIcon.Visible = true;
			}
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
			
			var fileName = Literal.NowTimestampFileName + ".zip";
			var saveFilePath = Path.Combine(saveDirPath, fileName);
			FileUtility.MakeFileParentDirectory(saveFilePath);
			
			// zip
			using(var zip = new ZipArchive(new FileStream(saveFilePath, FileMode.Create), ZipArchiveMode.Create)) {
				foreach(var filePath in enabledFiles) {
					var entry = zip.CreateEntry(Path.GetFileName(filePath));
					using(var entryStream = new BinaryWriter(entry.Open())) {
						var buffer = FileUtility.ToBinary(filePath);
						//var buffer = File.ReadAllBytes(filePath);
						/*
						using(var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
							var buffer = new byte[Literal.fileTempBufferLength];
							int readLength;
							while((readLength = fileStream.Read(buffer, 0, buffer.Length)) > 0) {
								entryStream.Write(buffer, 0, readLength);
							}
						}
						 */
						entryStream.Write(buffer);
					}
				}
			}

		}
		
		/// <summary>
		/// 現在の設定データを保存する。
		/// </summary>
		void SaveSetting()
		{
			// バックアップ
			var backupFiles = new [] {
				Literal.UserMainSettingPath,
				Literal.UserLauncherItemsPath,
				Literal.UserDBPath,
			};
			BackupSetting(backupFiles, Literal.UserBackupDirPath, Literal.backupCount);
			
			// 保存開始
			// メインデータ
			Serializer.SaveFile(this._commonData.MainSetting, Literal.UserMainSettingPath);
			//ランチャーデータ
			var sortedSet = new HashSet<LauncherItem>();
			foreach(var item in this._commonData.MainSetting.Launcher.Items.OrderBy(item => item.Name)) {
				sortedSet.Add(item);
			}
			Serializer.SaveFile(sortedSet, Literal.UserLauncherItemsPath);
		}
		
		/// <summary>
		/// 終了する。
		/// </summary>
		/// <param name="save"></param>
		public void CloseApplication(bool save)
		{
			if(save) {
				SaveSetting();
			}
			
			Application.Exit();
		}
		
		/// <summary>
		/// ツール―バー状態のリセット。
		/// </summary>
		void ResetToolbar()
		{
			Debug.WriteLine("ResetToolbar");
			foreach(var toolbar in this._toolbarForms.Values) {
				//toolbar.SetCommonData(this._commonData);
				toolbar.ToDispose();
			}
			this._toolbarForms.Clear();
			
			InitializeToolbarForm(null, null);
			
			// メニュー構築
			var menuItem = this._contextMenu.MenuItems[menuNameWindowToolbar];
			foreach(MenuItem subItem in menuItem.MenuItems) {
				subItem.ToDispose();
			}
			menuItem.MenuItems.Clear();
			
			AttachmentToolbarSubMenu(menuItem);
		}
		
		/// <summary>
		/// 設定ダイアログを開く。
		/// </summary>
		/// <returns></returns>
		Func<bool> OpenSettingDialog()
		{
			using(var settingForm = new SettingForm(this._commonData.Language, this._commonData.MainSetting, this._commonData.Database)) {
				if(settingForm.ShowDialog() == DialogResult.OK) {
					foreach(var note in this._noteWindowList) {
						note.Close();
						note.Dispose();
					}
					this._noteWindowList.Clear();
					
					var mainSetting = settingForm.MainSetting;
					var check = mainSetting.RunningInfo.CheckUpdate != mainSetting.RunningInfo.CheckUpdate || mainSetting.RunningInfo.CheckUpdate;
					this._commonData.MainSetting = mainSetting;
					settingForm.SaveFiles();
					settingForm.SaveDB(this._commonData.Database);
					SaveSetting();
					InitializeLanguage(null, null);
					ApplyLanguage();
					
					return () => {
						this._logForm.SetCommonData(this._commonData);
						this._messageWindow.SetCommonData(this._commonData);
						/*
						foreach(var toolbar in this._toolbarForms.Values) {
							//toolbar.SetCommonData(this._commonData);
							toolbar.Dispose();
						}
						this._toolbarForms.Clear();
						InitializeToolbarForm(null, null);
						 */
						ResetToolbar();
						
						InitializeNoteForm(null, null);
						
						if(check) {
							#if !DISABLED_UPDATE_CHECK
							CheckUpdateProcessAsync(false);
							#endif
						}
						
						return true;
					};
				}
			}
			return null;
		}
		
		/// <summary>
		/// システム環境を変更する。
		/// </summary>
		/// <param name="nowValueDg"></param>
		/// <param name="changeValueDg"></param>
		/// <param name="messageTitleName"></param>
		/// <param name="showMessageName"></param>
		/// <param name="hiddenMessageName"></param>
		/// <param name="errorMessageName"></param>
		void ChangeShowSystemEnvironment(Func<bool> nowValueDg, Action<bool> changeValueDg, string messageTitleName, string showMessageName, string hiddenMessageName, string errorMessageName)
		{
			var prevValue = nowValueDg();
			changeValueDg(!prevValue);
			var nowValue = nowValueDg();
			SystemEnvironment.RefreshShell();
			
			ToolTipIcon icon;
			string messageName;
			if(prevValue != nowValue) {
				if(nowValue) {
					messageName = showMessageName;
				} else {
					messageName = hiddenMessageName;
				}
				icon = ToolTipIcon.Info;
				ResetLauncherFileList();
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
		
		/// <summary>
		/// ホットキー受信。
		/// </summary>
		/// <param name="hotKeyId"></param>
		/// <param name="mod"></param>
		/// <param name="key"></param>
		public void ReceiveHotKey(HotKeyId hotKeyId, MOD mod, Keys key)
		{
			if(this._pause) {
				return;
			}
			
			switch(hotKeyId) {
				case HotKeyId.HiddenFile:
					ChangeShowSystemEnvironment(SystemEnvironment.IsHiddenFileShow, SystemEnvironment.SetHiddenFileShow, "balloon/hidden-file/title", "balloon/hidden-file/show", "balloon/hidden-file/hide", "balloon/hidden-file/error");
					break;
					
				case HotKeyId.Extension:
					ChangeShowSystemEnvironment(SystemEnvironment.IsExtensionShow, SystemEnvironment.SetExtensionShow, "balloon/extension/title", "balloon/extension/show", "balloon/extension/hide", "balloon/extension/error");
					break;
					
				case HotKeyId.CreateNote:
					ShowBalloon(ToolTipIcon.Info, this._commonData.Language["balloon/note/title"], this._commonData.Language["balloon/note/create"]);
					CreateNote(Point.Empty);
					break;
				case HotKeyId.HiddenNote:
					ShowBalloon(ToolTipIcon.Info, this._commonData.Language["balloon/note/title"], this._commonData.Language["balloon/note/hidden"]);
					HiddenNote();
					break;
				case HotKeyId.CompactNote:
					ShowBalloon(ToolTipIcon.Info, this._commonData.Language["balloon/note/title"], this._commonData.Language["balloon/note/compact"]);
					CompactNote();
					break;
				case HotKeyId.ShowFrontNote:
					ShowBalloon(ToolTipIcon.Info, this._commonData.Language["balloon/note/title"], this._commonData.Language["balloon/note/show-front"]);
					ShowFrontNote();
					break;
					
				default:
					break;
			}
		}

		void CreateNote(Point point)
		{
			// アイテムをデータ設定
			var item = new NoteItem();
			item.Title = DateTime.Now.ToString();
			if(point.IsEmpty) {
				item.Location = Cursor.Position;
			} else {
				item.Location = point;
			}
			var noteDB = new NoteDB(this._commonData.Database);
			noteDB.InsertMaster(item);
			
			var noteForm = CreateNote(item);
			noteForm.Activate();
		}
		
		Form CreateNote(NoteItem noteItem)
		{
			var noteForm = new NoteForm();
			noteForm.NoteItem = noteItem;
			noteForm.SetCommonData(this._commonData);
			noteForm.Show();
			noteForm.Closed += (object sender, EventArgs e) => {
				if(noteForm.Visible) {
					this._noteWindowList.Remove(noteForm);
				}
			};
			this._noteWindowList.Add(noteForm);
			return noteForm;
		}
		
		void HiddenNote()
		{
			var list = this._noteWindowList
				.Where(note => !note.NoteItem.Locked)
				;
			foreach(var note in list.ToArray()) {
				note.ToClose(false);
			}
		}
		
		void CompactNote()
		{
			var list = this._noteWindowList
				.Where(note => !note.NoteItem.Compact)
				.Where(note => !note.NoteItem.Locked)
				;
			foreach(var note in list) {
				note.ToCompact();
			}
		}
		
		void ShowFrontNote()
		{
			var list = this._noteWindowList;
			foreach(var note in list) {
				UIUtility.ShowFront(note);
			}
		}
		
		UpdateData CheckUpdate(bool force)
		{
			var updateData = new UpdateData(Literal.UserDownloadDirPath, this._commonData.MainSetting.RunningInfo.CheckUpdateRC);
			if(force || !this._pause && this._commonData.MainSetting.RunningInfo.CheckUpdate) {
				updateData = new UpdateData(Literal.UserDownloadDirPath, this._commonData.MainSetting.RunningInfo.CheckUpdateRC);
				this._commonData.Logger.Puts(LogType.Information, this._commonData.Language["log/update/check"], Literal.UpdateURL);
				var updateInfo = updateData.Check();
			}
			return updateData;
		}
		
		void CheckedUpdate(bool force, UpdateData updateData)
		{
			if(force || !this._pause && this._commonData.MainSetting.RunningInfo.CheckUpdate) {
				if(updateData != null && updateData.Info != null) {
					if(updateData.Info.IsUpdate) {
						ShowUpdateDialog(updateData);
					} else if(updateData.Info.IsError) {
						this._commonData.Logger.Puts(LogType.Warning, this._commonData.Language["log/update/error"], updateData.Info.Log);
					} else {
						this._commonData.Logger.Puts(LogType.Information, this._commonData.Language["log/update/newest"], updateData.Info.Log);
					}
				} else {
					this._commonData.Logger.Puts(LogType.Error, this._commonData.Language["log/update/error"], "info is null");
				}
			} else if(this._pause) {
				this._commonData.Logger.Puts(LogType.Information, this._commonData.Language["log/update/check-stop"], "this._pause => true");
			}
		}
		
		void CheckUpdateProcessAsync(bool force)
		{
			Task.Factory.StartNew(
				() => {
					if(!force) {
						Thread.Sleep(Literal.updateWaitTime);
					}
					return CheckUpdate(force);
				}
			).ContinueWith(
				t => {
					CheckedUpdate(force, t.Result);
				},
				TaskScheduler.FromCurrentSynchronizationContext()
			);
		}
		
		void CheckUpdateProcessWait(bool force)
		{
			var updateData = CheckUpdate(force);
			CheckedUpdate(force, updateData);
		}
		
		/// <summary>
		/// アップデートダイアログ表示。
		/// </summary>
		/// <param name="updateData"></param>
		void ShowUpdateDialog(UpdateData updateData)
		{
			PauseOthers(
				() => {
					using(var dialog = new UpdateForm()) {
						dialog.UpdateData = updateData;
						dialog.SetCommonData(this._commonData);
						if(dialog.ShowDialog() == DialogResult.OK) {
							updateData.Execute();
						}
					}
					return null;
				}
			);
		}
		
		void ResetLauncherFileList()
		{
			// なんかこれそもそもが変な気がするんです
		}
		
		/// <summary>
		/// ディスプレイ数に変更があった。
		/// </summary>
		void ChangedScreenCount()
		{
			this._commonData.Logger.Puts(LogType.Information, this._commonData.Language["main/event/screen/count-change"], string.Empty);
			ResetToolbar();
		}
		
		/// <summary>
		/// ホームダイアログ表示。
		/// </summary>
		void ShowHomeDialog()
		{
			PauseOthers(
				() => {
					using(var dialog = new HomeForm()) {
						dialog.SetCommonData(this._commonData);
						dialog.ShowDialog();
						if(dialog.ItemFinded) {
							// TODO: 初期化
						}
					}
					
					return null;
				}
			);
		}
	}
}