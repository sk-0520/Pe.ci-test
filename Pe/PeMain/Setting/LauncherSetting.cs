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
using System.Drawing;
using System.Xml.Serialization;
using PeMain.Logic;
using PeUtility;

namespace PeMain.Setting
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
		
		/// <summary>
		/// 
		/// </summary>
		private int _pId;
		
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			LauncherItem item = obj as LauncherItem;
			if(item == null) {
				return false;
			}
			return item.Name == Name;
		}
		
		public override int GetHashCode()
		{
			return this.Name.GetHashCode();
		}
		#endregion
		
		public LauncherItem()
		{
			this._pId = 0;
			this._iconMap = new Dictionary<IconSize, Icon>();
			
			HasError = false;
			
			LauncherHistory = new LauncherHistory();
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
		/// 実行時に渡されオプション。
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
		/// プロセス監視
		/// </summary>
		public bool ProcessWatch { get; set; }
		/// <summary>
		/// 標準出力(とエラー)の監視
		/// </summary>
		public bool StdOutputWatch { get; set; }
		/// <summary>
		/// 環境変数
		/// </summary>
		public EnvironmentSetting EnvironmentSetting { get; set; }
		
		public int PId { get { return this._pId; } }
		
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
		
		public object Clone()
		{
			var result = new LauncherItem();
			result.LauncherType = LauncherType;
			result.Command = Command;
			result.WorkDirPath = WorkDirPath;
			result.Option = Option;
			result.IconPath = IconPath;
			result.IconIndex = IconIndex;
			result.LauncherHistory = (LauncherHistory)LauncherHistory.Clone();
			result.Note = Note;
			result.Tag.AddRange(Tag);
			result.ProcessWatch = ProcessWatch;
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
			if(hasIcon && !string.IsNullOrWhiteSpace(IconPath)) {
				var icon = IconLoader.Load(IconPath, iconSize, 0);
				this._iconMap[iconSize] = icon;
			} else if(!hasIcon) {
				return null;
			}
			return this._iconMap[iconSize];
		}
		
		public void ClearIcon()
		{
			this._iconMap.Clear();
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
