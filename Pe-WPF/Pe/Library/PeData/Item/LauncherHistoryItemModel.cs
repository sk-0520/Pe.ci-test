namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	[Serializable]
	public class LauncherHistoryItemModel: HistoryItemModel, IDeepClone
	{
		public LauncherHistoryItemModel()
			: base()
		{
			WorkDirectoryPaths = new ObservableCollection<string>();
			Options = new ObservableCollection<string>();
		}

		/// <summary>
		/// 実行回数。
		/// </summary>
		[DataMember]
		public uint ExecuteCount { get; set; }

		/// <summary>
		/// 作業ディレクトリ。
		/// </summary>
		[DataMember, XmlArray("WorkDirectoryPaths"), XmlArrayItem("Item")]
		ObservableCollection<string> WorkDirectoryPaths { get; set; }

		/// <summary>
		/// オプション。
		/// </summary>
		[DataMember, XmlArray("Options"), XmlArrayItem("Item")]
		ObservableCollection<string> Options { get; set; }

		#region IDeepClone

		public override IDeepClone DeepClone()
		{
			var result = new LauncherHistoryItemModel() {
				ExecuteCount = this.ExecuteCount,
				CreateTimestamp = this.CreateTimestamp,
				UpdateTimestamp = this.UpdateTimestamp,
				UpdateCount = this.UpdateCount,
			};

			result.WorkDirectoryPaths = new ObservableCollection<string>(WorkDirectoryPaths);
			result.Options = new ObservableCollection<string>(Options);

			return result;
		}

		#endregion
	}
}
