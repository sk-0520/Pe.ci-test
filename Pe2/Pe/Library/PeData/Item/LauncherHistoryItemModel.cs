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

	[DataContract, Serializable]
	public class LauncherHistoryItemModel: HistoryItemModel, IDeepClone
	{
		public LauncherHistoryItemModel()
			: base()
		{
			WorkDirectoryPaths = new List<string>();
			Options = new List<string>();
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
		List<string> WorkDirectoryPaths { get; set; }

		/// <summary>
		/// オプション。
		/// </summary>
		[DataMember, XmlArray("Options"), XmlArrayItem("Item")]
		List<string> Options { get; set; }

		#region IDeepClone

		public IDeepClone DeepClone()
		{
			var result = new LauncherHistoryItemModel() {
				ExecuteCount = this.ExecuteCount,
				CreateDateTime = this.CreateDateTime,
				UpdateDateTime = this.UpdateDateTime,
				UpdateCount = this.UpdateCount,
			};

			result.WorkDirectoryPaths.AddRange(WorkDirectoryPaths);
			result.Options.AddRange(Options);

			return result;
		}

		#endregion
	}
}
