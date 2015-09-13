namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Drawing;
	using System.IO;
	using System.Linq;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Pe.Library.Skin;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Kind;
	using ContentTypeTextNet.Pe.PeMain.Logic;

	/// <summary>
	/// ランチャー設定データ。
	/// 
	/// 名前をキーとする。
	/// </summary>
	[Serializable]
	public class LauncherItem: DisposableNameItem, IDisposable, IDeepClone
	{
		/// <summary>
		/// 見つからなかった時用アイコン。
		/// </summary>
		public static IReadOnlyDictionary<IconScale, Icon> notfoundIconMap;
		public static IReadOnlyDictionary<IconScale, Icon> commandIconMap;

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

			IconItem = new IconItem();
			LauncherType = LauncherType.File;
			LauncherHistory = new LauncherHistory();
			EnvironmentSetting = new EnvironmentSetting();
			Tag = new List<string>();
		}

		public override void CorrectionValue()
		{
			base.CorrectionValue();

			if(IconItem == null) {
				IconItem = new IconItem();
			}

			if(LauncherType == LauncherType.URI) {
				LauncherType = LauncherType.Command;
			}
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
		/// 表示アイコンパス。
		/// </summary>
		public IconItem IconItem { get; set; }

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

		/*
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
		*/

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
				var result = (File.Exists(Command) || Directory.Exists(Command)) || (File.Exists(expandCommand) || Directory.Exists(expandCommand));
				return result;
			}
		}

		/// <summary>
		/// 現在アイテムが管理者として実行可能か
		/// </summary>
		[Obsolete("ちょっと隠居してくれ")]
		public bool CanAdministratorExecute
		{
			get
			{
				if(IsDirectory) {
					return false;
				}

				var dotExt = Path.GetExtension(Command).ToLower();
				return new[] { ".exe", ".bat", ".cmd" }.Any(ext => ext == dotExt);
			}
		}

		public bool IsExexuteFile
		{
			get
			{
				return Path.GetExtension(Command).ToLower() == ".exe";
			}
		}

		public bool IsDirectory
		{
			get
			{
				if((new[] { LauncherType.File, LauncherType.Directory }).All(lt => lt != LauncherType)) {
					return false;
				}
				var expandCommand = Environment.ExpandEnvironmentVariables(Command);
				return Directory.Exists(expandCommand);
			}
		}


		[XmlIgnoreAttribute()]
		public bool HasError { get; set; }

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		public bool IsNameEqual(string name)
		{
			return Name == name;
		}

		#region IDeepClone

		public IDeepClone DeepClone()
		{
			var result = new LauncherItem();
			result.Name = Name;
			result.LauncherType = LauncherType;
			result.Command = Command;
			result.WorkDirPath = WorkDirPath;
			result.Option = Option;
			/*
			result.IconPath = IconPath;
			result.IconIndex = IconIndex;
			 */
			result.IconItem = (IconItem)IconItem.Clone();
			result.LauncherHistory = (LauncherHistory)LauncherHistory.DeepClone();
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

		#endregion

		public bool IsValueEqual(LauncherItem target)
		{
			return
				LauncherType == target.LauncherType
				&& Administrator == target.Administrator
				//&& IconIndex == target.IconIndex
				&& StdOutputWatch == target.StdOutputWatch
				&& Command == target.Command
				&& WorkDirPath == target.WorkDirPath
				&& Option == target.Option
				//&& IconPath == target.IconPath
				&& IconItem.Equals(target.IconItem)
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
		public void Increment(string option, string workDirPath)
		{
			LauncherHistory.ExecuteCount += 1;
			LauncherHistory.DateHistory.Update = DateTime.UtcNow;
			IncrementList(LauncherHistory.Options, option);
			IncrementList(LauncherHistory.WorkDirs, workDirPath);
		}
	}

}
