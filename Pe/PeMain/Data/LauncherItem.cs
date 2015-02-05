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
	public class LauncherItem: DisposableNameItem, IDisposable, ICloneable
	{
		/// <summary>
		/// 見つからなかった時用アイコン。
		/// </summary>
		public static IReadOnlyDictionary<IconScale, Icon> notfoundIconMap;
		public static IReadOnlyDictionary<IconScale, Icon> commandIconMap;

		public static void SetSkin(ISkin skin)
		{
			var iconScaleList = new[] { IconScale.Small, IconScale.Normal, IconScale.Big };
			// NotFound 
			var tempNotfoundIconMap = new Dictionary<IconScale, Icon>(iconScaleList.Length);
			foreach(var iconScale in iconScaleList) {
				var iconSize = iconScale.ToSize();
				var icon = new Icon(skin.GetIcon(SkinIcon.NotFound), iconSize);
				var image = new Bitmap(iconSize.Width, iconSize.Height);
				using(var g = Graphics.FromImage(image)) {
					g.DrawIcon(icon, new Rectangle(Point.Empty, iconSize));
				}
				tempNotfoundIconMap[iconScale] = icon;
			}
			notfoundIconMap = tempNotfoundIconMap;

			// URIアイコン構築
			var tempCommandIconMap = new Dictionary<IconScale, Icon>(iconScaleList.Length);
			foreach(var iconScale in iconScaleList) {
				var iconSize = iconScale.ToSize();
				var icon = new Icon(skin.GetIcon(SkinIcon.Command), iconSize);
				var image = new Bitmap(iconSize.Width, iconSize.Height);
				using(var g = Graphics.FromImage(image)) {
					g.DrawIcon(icon, new Rectangle(Point.Empty, iconSize));
				}
				tempCommandIconMap[iconScale] = icon;
			}
			commandIconMap = tempCommandIconMap;
		}

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
			foreach(var icon in _iconMap.Values) {
				icon.ToDispose();
			}

			base.Dispose(disposing);
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
			/*
			result.IconPath = IconPath;
			result.IconIndex = IconIndex;
			 */
			result.IconItem = (IconItem)IconItem.Clone();
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
		/// <param name = "iconScale">アイコンサイズ</param>
		/// <param name="iconIndex">アイコンインデックス</param>
		/// <returns>アイコン</returns>
		public Icon GetIcon(IconScale iconScale, int iconIndex, ApplicationSetting applicationSetting, ILogger logger)
		{
			var hasIcon = this._iconMap.ContainsKey(iconScale);
			if(!hasIcon) {
				string useIconPath = null;

				if(LauncherType == LauncherType.Embedded) {
					Debug.Assert(applicationSetting != null);
					var applicationItem = applicationSetting.GetApplicationItem(this);
					useIconPath = applicationItem.FilePath;
					hasIcon = true;
				} else {
					if(!string.IsNullOrWhiteSpace(IconItem.Path)) {
						var expandIconPath = Environment.ExpandEnvironmentVariables(IconItem.Path);
						//hasIcon = File.Exists(expandIconPath) || Directory.Exists(expandIconPath);
						hasIcon = FileUtility.Exists(expandIconPath);
						useIconPath = expandIconPath;
					}
					if(!hasIcon) {
						if(new[] { LauncherType.File, LauncherType.Directory }.Any(lt => lt == LauncherType)) {
							if(!string.IsNullOrWhiteSpace(Command)) {
								var expandPath = Environment.ExpandEnvironmentVariables(Command);
								//hasIcon = File.Exists(expandPath) || Directory.Exists(expandPath);
								hasIcon = FileUtility.Exists(expandPath);
								useIconPath = expandPath;
							}
						}
					}
				}
				if(hasIcon) {
					Debug.Assert(useIconPath != null);
					/*
					var waitCount = 0;
					while(waitCount <= Literal.loadIconRetryCount) {
						var icon = IconUtility.Load(useIconPath, iconScale, iconIndex);
							if(icon != null) {
								this._iconMap[iconScale] = icon;
								break;
							} else {
								logger.PutsDebug(useIconPath, () => string.Format("LauncherItem: wait {0}ms, count: {1}", Literal.loadIconRetryTime.TotalMilliseconds, waitCount));
								Thread.Sleep(Literal.loadIconRetryTime);
								waitCount++;
							}
					}
					*/
					this._iconMap[iconScale] = AppUtility.LoadIcon(new IconPath(useIconPath, iconIndex), iconScale, Literal.loadIconRetryTime, Literal.loadIconRetryCount, logger);
				}
			}

			if(hasIcon) {
				return this._iconMap[iconScale];
			} else {
				if(LauncherType == LauncherType.URI || LauncherType == LauncherType.Command) {
					return commandIconMap[iconScale];
				} else {
					return notfoundIconMap[iconScale];
				}
			}
		}

		public Icon GetEmbeddedIcon(IconScale iconScale, ApplicationItem applicationItem)
		{
			var icon = IconUtility.Load(applicationItem.FilePath, iconScale, 0);
			return icon;
		}

		public void ClearIcon()
		{
			this._iconMap.Clear();
		}


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
