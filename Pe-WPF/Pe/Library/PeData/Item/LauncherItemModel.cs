namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.IF;

	[Serializable]
	public class LauncherItemModel: GuidModelBase, IName, IDeepClone
	{
		public LauncherItemModel()
			: base()
		{
			Icon = new IconItemModel();
			History = new LauncherHistoryItemModel();
			Tag = new TagItemModel();
			StdStream = new LauncherStdStreamItemModel();
			EnvironmentVariables = new EnvironmentVariablesItemModel();
		}

		#region IName

		/// <summary>
		/// アイテム名称。
		/// </summary>
		[DataMember, XmlAttribute]
		public string Name { get; set; }

		#endregion

		/// <summary>
		/// ランチャー種別。
		/// </summary>
		[DataMember]
		public LauncherKind LauncherKind { get; set; }
		/// <summary>
		/// 実行時に使用される値。
		/// </summary>
		[DataMember]
		public string Command { get; set; }
		/// <summary>
		/// 実行時に渡されるオプション。
		/// </summary>
		[DataMember]
		public string Option { get; set; }
		/// <summary>
		/// 実行時の作業ディレクトリ。
		/// </summary>
		[DataMember]
		public string WorkDirectoryPath { get; set; }
		/// <summary>
		/// 表示アイコンパス。
		/// </summary>
		[DataMember]
		public IconItemModel Icon { get; set; }

		/// <summary>
		/// 実行履歴
		/// </summary>
		[DataMember]
		public LauncherHistoryItemModel History { get; set; }

		/// <summary>
		/// コメント
		/// </summary>
		[DataMember]
		public string Comment { get; set; }
		/// <summary>
		/// タグ
		/// </summary>
		[DataMember]
		public TagItemModel Tag { get; set; }
		/// <summary>
		/// 標準入出力。
		/// </summary>
		[DataMember]
		public LauncherStdStreamItemModel StdStream { get; set; }
		/// <summary>
		/// 管理者として実行。
		/// </summary>
		[DataMember]
		public bool Administrator { get; set; }
		/// <summary>
		/// 環境変数。
		/// </summary>
		[DataMember]
		public EnvironmentVariablesItemModel EnvironmentVariables { get; set; }

		#region IDeepClone

		public virtual void DeepCloneTo(IDeepClone target)
		{
			var obj = (LauncherItemModel)target;

			obj.Id = Id;
			obj.Name = Name;
			obj.LauncherKind = LauncherKind;
			obj.Command = Command;
			obj.WorkDirectoryPath = WorkDirectoryPath;
			obj.Option = Option;
			obj.Comment = Comment;
			obj.Administrator = Administrator;

			Icon.DeepCloneTo(obj.Icon);
			History.DeepCloneTo(obj.History);
			Tag.DeepCloneTo(obj.Tag);
			StdStream.DeepCloneTo(obj.StdStream);
			EnvironmentVariables.DeepCloneTo(obj.EnvironmentVariables);
		}

		public IDeepClone DeepClone()
		{
			var result = new LauncherItemModel();

			DeepCloneTo(result);

			return result;
		}

		#endregion
	}
}
