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
	
	/// <summary>
	/// 実行履歴
	/// </summary>
	public class LauncherHistory: Item
	{
		public LauncherHistory()
		{
			WorkDirs = new List<string>();
			Options = new List<string>();
		}
		public uint ExecuteCount { get; set; }
		public List<string> WorkDirs { get; set; }
		public List<string> Options { get; set; }
	}
	
	/// <summary>
	/// ランチャー設定データ。
	/// 
	/// 名前をキーとする。
	/// </summary>
	[Serializable]
	public class LauncherItem: NameItem, IDisposable
	{
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
			// TODO: 処理実装がデフォのまま
			int hashCode = 0;
			unchecked {
				if (_iconMap != null)
					hashCode += 1000000007 * _iconMap.GetHashCode();
			}
			return hashCode;
		}
		#endregion

		/// <summary>
		/// 
		/// </summary>
		private Dictionary<IconSize, Icon> _iconMap;
		
		public LauncherItem()
		{
			this._iconMap = new Dictionary<IconSize, Icon>();
			
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
		
		public void Dispose()
		{
			foreach(var icon in _iconMap.Values) {
				if(icon != null) {
					icon.Dispose();
				}
			}
		}
	}
	
	/// <summary>
	/// ランチャーアイテム統括。
	/// </summary>
	public class LauncherSetting: Item, IDisposable
	{
		public LauncherSetting()
		{
			Items = new SortedSet<LauncherItem>();
		}
		/// <summary>
		/// 各ランチャアイテム
		/// </summary>
		public SortedSet<LauncherItem> Items { get; set; }
		
		public void Dispose()
		{
			foreach(var item in Items) {
				item.Dispose();
			}
		}
	}
}
