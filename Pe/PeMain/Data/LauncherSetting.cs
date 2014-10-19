/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/15
 * 時刻: 19:52
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using PeUtility;

namespace PeMain.Data
{
	/// <summary>
	/// ランチャー種別。
	/// </summary>
	public enum LauncherType
	{
		/// <summary>
		/// 何かのファイル。
		/// </summary>
		File,
		/// <summary>
		/// ディレクトリ。
		/// </summary>
		Directory,
		/// <summary>
		/// URI。
		/// </summary>
		URI,
		/// <summary>
		/// 組み込み
		/// </summary>
		Embedded
	}
	
	/// <summary>
	/// 実行履歴
	/// </summary>
	[Serializable]
	public class LauncherHistory: Item, ICloneable
	{
		public LauncherHistory()
		{
			WorkDirs = new List<string>();
			Options = new List<string>();
			DateHistory = new DateHistory();
		}
		/// <summary>
		/// 実行回数
		/// </summary>
		public uint ExecuteCount { get; set; }
		/// <summary>
		/// 作業ディレクトリ
		/// </summary>
		public List<string> WorkDirs { get; set; }
		/// <summary>
		/// オプション
		/// </summary>
		public List<string> Options { get; set; }
		/// <summary>
		/// アイテム登録及び更新日時
		/// </summary>
		public DateHistory DateHistory { get; set; }
		
		/// <summary>
		/// 複製
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			var result = new LauncherHistory();
			result.ExecuteCount = ExecuteCount;
			result.WorkDirs.AddRange(WorkDirs);
			result.Options.AddRange(Options);
			result.DateHistory = (DateHistory)DateHistory.Clone();
			
			return result;
		}
	}
	
	/// <summary>
	/// ランチャー設定データ。
	/// 
	/// 名前をキーとする。
	/// </summary>
	[Serializable]
	public class LauncherItem: DisposableNameItem, IDisposable, ICloneable
	{
		/// <summary>
		/// 見つからなかった時用アイコン。
		/// </summary>
		private static readonly Dictionary<IconScale, Icon> _notfoundIconMap = new Dictionary<IconScale, Icon>() {
			{ IconScale.Small,  Icon.FromHandle(PeMain.Properties.Images.NotFound_016.GetHicon()) },
			{ IconScale.Normal, Icon.FromHandle(PeMain.Properties.Images.NotFound_032.GetHicon()) },
			{ IconScale.Big,    Icon.FromHandle(PeMain.Properties.Images.NotFound_048.GetHicon()) },
			{ IconScale.Large,  Icon.FromHandle(PeMain.Properties.Images.NotFound_256.GetHicon()) },
		};
		
		/// <summary>
		/// 現在のアイテムが保持するアイコン一覧。
		/// </summary>
		private Dictionary<IconScale, Icon> _iconMap;
		
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			LauncherItem item = obj as LauncherItem;
			if(item == null) {
				return false;
			}
			return IsNameEqual(item.Name);
		}
		
		public override int GetHashCode()
		{
			//if(this.Name == null) {
			//	return default(int);
			//}
			return Name.GetHashCode();
		}
		#endregion
		
		public LauncherItem()
		{
			this._iconMap = new Dictionary<IconScale, Icon>();
			
			HasError = false;
			
			LauncherHistory = new LauncherHistory();
			EnvironmentSetting = new EnvironmentSetting();
			Tag = new List<string>();
		}
		/// <summary>
		/// 登録データ種別。
		/// </summary>
		public LauncherType LauncherType { get; set; }
		/// <summary>
		/// 実行時に使用される値。
		/// </summary>
		public string Command { get; set; }
		/// <summary>
		/// 実行時の作業ディレクトリ。
		/// </summary>
		public string WorkDirPath { get; set; }
		/// <summary>
		/// 実行時に渡されるオプション。
		/// </summary>
		public string Option { get; set; }
		/// <summary>
		/// 表示アイコンパス
		/// </summary>
		public string IconPath { get; set; }
		/// <summary>
		/// 表示アイコンインデックス
		/// </summary>
		public int IconIndex { get; set; }
		/// <summary>
		/// 実行履歴
		/// </summary>
		public LauncherHistory LauncherHistory { get; set; }
		/// <summary>
		/// コメント
		/// </summary>
		public string Note { get; set; }
		/// <summary>
		/// タグ
		/// </summary>
		public List<string> Tag { get; set; }
		/// <summary>
		/// 標準出力(とエラー)の監視
		/// </summary>
		public bool StdOutputWatch { get; set; }
		/// <summary>
		/// 管理者として実行
		/// </summary>
		public bool Administrator { get; set; }
		/// <summary>
		/// 環境変数
		/// </summary>
		public EnvironmentSetting EnvironmentSetting { get; set; }
		
		public bool IsExtExec
		{
			get
			{
				if(LauncherType != LauncherType.File) {
					return false;
				}
				
				return Path.GetExtension(Command).ToLower() == ".exe";
			}
		}
		
		/// <summary>
		/// 存在するか
		/// </summary>
		public bool IsExists
		{
			get
			{
				if(LauncherType != LauncherType.File) {
					return false;
				}
				
				var expandCommand = Environment.ExpandEnvironmentVariables(Command);
				var result =  (File.Exists(Command) || Directory.Exists(Command)) || (File.Exists(expandCommand) || Directory.Exists(expandCommand));
				return result;
			}
		}
		
		/// <summary>
		/// アイテムは実行形式か
		/// </summary>
		public bool IsExecteFile
		{
			get
			{
				if(IsExtExec && IsExists) {
					return Path.GetExtension(Command).ToLower() == ".exe";
				}
				return false;
			}
		}
		public bool IsNormalFile
		{
			get
			{
				if(IsExists) {
					return !IsExecteFile;
				}
				return false;
			}
		}
		public bool IsDirectory
		{
			get
			{
				if(LauncherType != LauncherType.File) {
					return false;
				}
				var expandCommand = Environment.ExpandEnvironmentVariables(Command);
				return Directory.Exists(expandCommand);
			}
		}
		
		
		[XmlIgnoreAttribute()]
		public bool HasError { get; set; }
		
		public override void Dispose()
		{
			base.Dispose();
			
			foreach(var icon in _iconMap.Values) {
				icon.ToDispose();
			}
		}
		
		public bool IsNameEqual(string name)
		{
			return Name == name;
		}
		
		public object Clone()
		{
			var result = new LauncherItem();
			result.Name = Name;
			result.LauncherType = LauncherType;
			result.Command = Command;
			result.WorkDirPath = WorkDirPath;
			result.Option = Option;
			result.IconPath = IconPath;
			result.IconIndex = IconIndex;
			result.LauncherHistory = (LauncherHistory)LauncherHistory.Clone();
			result.EnvironmentSetting = (EnvironmentSetting)EnvironmentSetting.Clone();
			result.Note = Note;
			result.Tag.AddRange(Tag);
			result.StdOutputWatch = StdOutputWatch;
			result.Administrator = Administrator;
			
			foreach(KeyValuePair<IconScale, Icon> pair in this._iconMap) {
				result._iconMap.Add(pair.Key, (Icon)pair.Value.Clone());
			}
			
			return result;
		}
		
		/// <summary>
		/// アイコン取得。
		/// </summary>
		/// <param name="iconSize">アイコンサイズ</param>
		/// <param name="iconIndex">アイコンインデックス</param>
		/// <returns>アイコン</returns>
		public Icon GetIcon(IconScale iconScale, int iconIndex)
		{
			var hasIcon = this._iconMap.ContainsKey(iconScale);
			if(!hasIcon) {
				string useIconPath = null;
				if(!string.IsNullOrWhiteSpace(IconPath)) {
					var expandIconPath = Environment.ExpandEnvironmentVariables(IconPath);
					hasIcon = File.Exists(expandIconPath) || Directory.Exists(expandIconPath);
					useIconPath = expandIconPath;
				}
				if(!hasIcon &&  LauncherType == LauncherType.File) {
					if(!string.IsNullOrWhiteSpace(Command)) {
						var expandFilePath = Environment.ExpandEnvironmentVariables(Command);
						hasIcon = File.Exists(expandFilePath) || Directory.Exists(expandFilePath);
						useIconPath = expandFilePath;
					}
				}
				if(hasIcon) {
					Debug.Assert(useIconPath != null);
					
					var icon = IconUtility.Load(useIconPath, iconScale, iconIndex);
					this._iconMap[iconScale] = icon;
				}
			}
			if(hasIcon) {
				return this._iconMap[iconScale];
			} else {
				return _notfoundIconMap[iconScale];
			}
		}
		
		public void ClearIcon()
		{
			this._iconMap.Clear();
		}
		
		/// <summary>
		/// ファイルパスからランチャーアイテムの生成。
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="useShortcut"></param>
		/// <returns></returns>
		public static LauncherItem FileLoad(string filePath, bool useShortcut = false)
		{
			var item = new LauncherItem();
			
			item.Name = Path.GetFileNameWithoutExtension(filePath);
			if(item.Name.Length == 0 && filePath.Length >= @"C:\".Length) {
				var drive = DriveInfo.GetDrives().SingleOrDefault(d => d.Name == filePath);
				if(drive != null) {
					item.Name = drive.VolumeLabel;
				}
			}
			if(item.Name.Length == 0) {
				item.Name = filePath;
			}
			item.Command = filePath;
			item.IconPath = filePath;
			item.IconIndex = 0;
			
			if(Directory.Exists(filePath)) {
				// ディレクトリ
				item.LauncherType = LauncherType.Directory;
			} else {
				// ファイルとかもろもろ
				item.LauncherType = LauncherType.File;
				
				var dotExt = Path.GetExtension(filePath);
				switch(dotExt.ToLower()) {
					case ".lnk":
						{
							var shortcut = new ShortcutFile(filePath, false);
							item.Command = shortcut.TargetPath;
							item.Option = shortcut.Arguments;
							item.WorkDirPath = shortcut.WorkingDirectory;

							item.IconPath = shortcut.IconPath;
							item.IconIndex = shortcut.IconIndex;
							item.Note = shortcut.Description;
						}
						break;
						
						/*
					case ".url":
						item.LauncherType = LauncherType.URI;
						break;
						 */
						
					case ".exe":
						{
							var verInfo = FileVersionInfo.GetVersionInfo(item.Command);
							if(!string.IsNullOrEmpty(verInfo.ProductName)) {
								item.Name = verInfo.ProductName;
							}
							item.Note = verInfo.Comments;
							if(!string.IsNullOrEmpty(verInfo.CompanyName)) {
								item.Tag.Add(verInfo.CompanyName);
							}
						}
						break;
						
					default:
						break;
				}
			}
			
			
			Debug.Assert(item.Name.Length > 0);
			
			return item;
		}
		
		static public string GetUniqueName(LauncherItem item, IEnumerable<LauncherItem> seq)
		{
			return item.Name.ToUnique(seq.Select(i => i.Name));
		}
		
		public bool IsValueEqual(LauncherItem target)
		{
			return
				LauncherType == target.LauncherType
				&& Administrator == target.Administrator
				&& IconIndex == target.IconIndex
				&& StdOutputWatch == target.StdOutputWatch
				&& Command == target.Command
				&& WorkDirPath == target.WorkDirPath
				&& Option == target.Option
				&& IconPath == target.IconPath
				&& EnvironmentSetting.EditEnvironment == target.EnvironmentSetting.EditEnvironment
				&& Note == target.Note
				&& Tag.SequenceEqual(target.Tag)
				&& EnvironmentSetting.Remove.SequenceEqual(target.EnvironmentSetting.Remove)
				&& EnvironmentSetting.Update.SequenceEqual(target.EnvironmentSetting.Update)
				;
		}
		
		/// <summary>
		/// リスト構造の整理。
		/// </summary>
		/// <param name="list"></param>
		/// <param name="value"></param>
		void IncrementList(List<string> list, string value)
		{
			if(!string.IsNullOrEmpty(value)) {
				var index = list.FindIndex(s => s == value);
				if(index != -1) {
					list.RemoveAt(index);
				}
				list.Insert(0, value);
			}
		}
		/// <summary>
		/// 使用回数をインクリメント。
		/// </summary>
		/// <param name="option">オプション履歴に追加する文字列</param>
		/// <param name="workDirPath">作業ディレクトリに追加する文字列</param>
		public void Increment(string option = null, string workDirPath = null)
		{
			LauncherHistory.ExecuteCount += 1;
			LauncherHistory.DateHistory.Update = DateTime.UtcNow;
			IncrementList(LauncherHistory.Options, option);
			IncrementList(LauncherHistory.WorkDirs, workDirPath);
		}
	}
	
	/// <summary>
	/// ランチャーアイテム統括。
	/// </summary>
	[Serializable]
	public class LauncherSetting: DisposableItem, IDisposable
	{
		public LauncherSetting()
		{
			Items = new HashSet<LauncherItem>();
			
			StreamFontSetting = new FontSetting();
		}
		
		/// <summary>
		/// 各ランチャアイテム
		/// </summary>
		[XmlIgnoreAttribute()]
		public HashSet<LauncherItem> Items { get; set; }
		
		/// <summary>
		/// 標準出力フォント。
		/// </summary>
		public FontSetting StreamFontSetting { get; set; }
		
		public override void Dispose()
		{
			base.Dispose();
			
			foreach(var item in Items) {
				item.ToDispose();
			}
		}
	}
}
