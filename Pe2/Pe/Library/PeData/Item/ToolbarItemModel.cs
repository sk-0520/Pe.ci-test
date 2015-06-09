namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Define;

	[Serializable]
	public class ToolbarItemModel: ItemModelBase, ITId<string>
	{
		public ToolbarItemModel()
			: base()
		{
			FloatToolbarArea = new FloatToolbarAreaItemModel();
			IconScale = IconScale.Normal;
			HiddenWaitTime = TimeSpan.FromSeconds(3);
			HiddenAnimateTime = TimeSpan.FromMilliseconds(250);
		}

		#region IId

		/// <summary>
		/// ツールバーの所属ディスプレイ名。
		/// </summary>
		[DataMember]
		public string Id { get; set; }

		#endregion 

		/// <summary>
		/// ツールバー位置。
		/// </summary>
		[DataMember]
		public ToobarPosition ToobarPosition { get; set; }
		/// <summary>
		/// 表示。
		/// </summary>
		[DataMember]
		public bool Visible { get; set; }
		/// <summary>
		/// 最前面表示。
		/// </summary>
		[DataMember]
		public bool TopMost { get; set; }
		/// <summary>
		/// フロート状態。
		/// </summary>
		[DataMember]
		public FloatToolbarAreaItemModel FloatToolbarArea { get; set; }
		/// <summary>
		/// アイコンサイズ。
		/// </summary>
		[DataMember]
		public IconScale IconScale { get; set; }
		/// <summary>
		/// 自動的に隠すか。
		/// </summary>
		[DataMember]
		public bool AutoHide { get; set; }
		/// <summary>
		/// 自動的に隠すまでの時間。
		/// </summary>
		[DataMember]
		public TimeSpan HiddenWaitTime { get; set; }
		/// <summary>
		/// 自動的に隠す際のアニメーション時間。
		/// </summary>
		[DataMember]
		public TimeSpan HiddenAnimateTime { get; set; }
		/// <summary>
		/// 初期値として使用するグループID。
		/// </summary>
		[DataMember]
		public string DefaultGroupId { get; set; }
	}
}
