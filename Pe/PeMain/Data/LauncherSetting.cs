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
using System.Windows.Forms;
using System.Xml.Serialization;

using PeMain.Logic;
using PeMain.UI;
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
		/// URI。
		/// </summary>
		URI,
		/// <summary>
		/// 組み込み
		/// </summary>
		Pe
	}
	
	public class LauncherTypeItem: UseLanguageItemData<LauncherType>
	{
		public LauncherTypeItem(LauncherType value): base(value) { }
		public LauncherTypeItem(LauncherType value, Language lang): base(value, lang) { }
		
		public override string Display { get { return Value.ToText(Language); } }
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
	public class LauncherItem: NameItem, IDisposable, ICloneable
	{
		/// <summary>
		/// 
		/// </summary>
		private Dictionary<IconSize, Icon> _iconMap;
		
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
			this._iconMap = new Dictionary<IconSize, Icon>();
			
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
		/// 環境変数
		/// </summary>
		public EnvironmentSetting EnvironmentSetting { get; set; }
		
		public bool IsExtExcec
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
				
				return File.Exists(Command) || Directory.Exists(Command);
			}
		}
		
		/// <summary>
		/// アイテムは実行形式か
		/// </summary>
		public bool IsExecteFile
		{
			get
			{
				if(IsExtExcec && IsExists) {
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
				return Directory.Exists(Command);
			}
		}
		
		
		[XmlIgnoreAttribute()]
		public bool HasError { get; set; }
		
		public void Dispose()
		{
			foreach(var icon in _iconMap.Values) {
				if(icon != null) {
					icon.Dispose();
				}
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
			result.Note = Note;
			result.Tag.AddRange(Tag);
			result.StdOutputWatch = StdOutputWatch;
			
			// アイコンは再読み込みかったるいのでこぴっておく
			foreach(KeyValuePair<IconSize, Icon> pair in this._iconMap) {
				result._iconMap.Add(pair.Key, pair.Value);
			}
			
			return result;
		}
		
		public Icon GetIcon(IconSize iconSize)
		{
			var hasIcon = this._iconMap.ContainsKey(iconSize);
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
					
					var icon = IconLoader.Load(useIconPath, iconSize, 0);
					this._iconMap[iconSize] = icon;
				}
			}
			if(hasIcon) {
				return this._iconMap[iconSize];
			} else {
				return null;
			}
		}
		
		public void ClearIcon()
		{
			this._iconMap.Clear();
		}
		
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
			item.LauncherType = LauncherType.File;
			
			var dotExt = Path.GetExtension(filePath);
			switch(dotExt.ToLower()) {
					// ショートカットの場合リンク元をファイルとする
					// TODO: IWshRuntimeLibrary参照でAxImp.exe読めない、SDKインストールできない、とりあえず後回し
				case ".lnk":
					//var wsh = new IWshRuntimeLibrary.WshShell();
					break;
					
				case ".url":
					item.LauncherType = LauncherType.URI;
					break;
					
				case ".exe":
					var verInfo = FileVersionInfo.GetVersionInfo(filePath);
					if(!string.IsNullOrEmpty(verInfo.ProductName)) {
						item.Name = verInfo.ProductName;
					}
					item.Note = verInfo.Comments;
					if(!string.IsNullOrEmpty(verInfo.CompanyName)) {
						item.Tag.Add(verInfo.CompanyName);
					}
					break;
					
				default:
					break;
			}
			
			Debug.Assert(item.Name.Length > 0);
			
			return item;
		}
		
		static public string GetUniqueName(LauncherItem item, IEnumerable<LauncherItem> seq)
		{
			return item.Name.ToUnique(seq.Select(i => i.Name));
		}
		
		static void ExecuteFile(ILogger logger, Language language, MainSetting mainSetting, LauncherItem launcherItem, Form parentForm)
		{
			Debug.Assert(launcherItem.LauncherType == LauncherType.File);
			
			var process = new Process();
			var startInfo = process.StartInfo;
			startInfo.FileName = launcherItem.Command;
			if(launcherItem.IsExecteFile) {
				startInfo.UseShellExecute = false;
				
				startInfo.WorkingDirectory = launcherItem.WorkDirPath;
				startInfo.Arguments = launcherItem.Option;
				
				// 環境変数
				if(launcherItem.EnvironmentSetting.EditEnvironment) {
					var env = startInfo.EnvironmentVariables;
					// 追加・更新
					foreach(var pair in launcherItem.EnvironmentSetting.Update) {
						env[pair.Key] = pair.Value;
					}
					// 削除
					launcherItem.EnvironmentSetting.Remove
						.Where(s => env.ContainsKey(s))
						.Transform(s => env.Remove(s))
					;
				}
				
				// 出力取得
				startInfo.CreateNoWindow = launcherItem.StdOutputWatch;
				if(launcherItem.StdOutputWatch) {
					startInfo.RedirectStandardOutput = true;
					startInfo.RedirectStandardError = true;
					var streamForm = new StreamForm();
					streamForm.SetParameter(process, launcherItem);
					streamForm.SetSettingData(language, mainSetting);
					streamForm.Show(parentForm);
				}
			}
			
			process.Start();
			
			if(launcherItem.StdOutputWatch) {
				process.BeginOutputReadLine();
				process.BeginErrorReadLine();
			}
		}
		
		public void Execute(ILogger logger, Language language, MainSetting mainSetting, Form parentForm)
		{
			if(LauncherType == LauncherType.File) {
				ExecuteFile(logger, language, mainSetting, this, parentForm);
			}
		}
		
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
		public void Increment(string option = null, string workDirPath = null)
		{
			LauncherHistory.ExecuteCount += 1;
			LauncherHistory.DateHistory.UpdateUTC = DateTime.UtcNow;
			IncrementList(LauncherHistory.Options, option);
			IncrementList(LauncherHistory.WorkDirs, workDirPath);
		}
	}
	
	/// <summary>
	/// ランチャーアイテム統括。
	/// </summary>
	[Serializable]
	public class LauncherSetting: Item, IDisposable
	{
		public LauncherSetting()
		{
			Items = new HashSet<LauncherItem>();
		}
		/// <summary>
		/// 各ランチャアイテム
		/// </summary>
		public HashSet<LauncherItem> Items { get; set; }
		
		public void Dispose()
		{
			foreach(var item in Items) {
				item.Dispose();
			}
		}
	}
}
