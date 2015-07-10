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
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.IF;

	[Serializable]
	public class ToolbarItemModel: ModelBase, IVisible, ITopMost, ITId<string>
	{
		public ToolbarItemModel()
			: base()
		{
			FloatToolbar = new FloatToolbarItemModel();
			IconScale = IconScale.Normal;
			HideWaitTime = TimeSpan.FromSeconds(3);
			HideAnimateTime = TimeSpan.FromMilliseconds(250);
		}

		#region property

		/// <summary>
		/// ツールバー位置。
		/// </summary>
		[DataMember]
		public DockType DockType { get; set; }

		#region IVisible

		/// <summary>
		/// 表示。
		/// </summary>
		[DataMember]
		public bool Visible { get; set; }

		#endregion

		#region ITopMost

		/// <summary>
		/// 最前面表示。
		/// </summary>
		[DataMember]
		public bool TopMost { get; set; }

		#endregion

		#region IId

		/// <summary>
		/// ツールバーの所属ディスプレイ名。
		/// </summary>
		[DataMember]
		public string Id { get; set; }

		public bool IsSafeId(string s)
		{
			return !string.IsNullOrEmpty(s);
		}

		public string ToSafeId(string s)
		{
			if(string.IsNullOrEmpty(s)) {
				return "id";
			}

			return s;
		}

		#endregion 

		/// <summary>
		/// フロート状態。
		/// </summary>
		[DataMember]
		public FloatToolbarItemModel FloatToolbar { get; set; }
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
		public TimeSpan HideWaitTime { get; set; }
		/// <summary>
		/// 自動的に隠す際のアニメーション時間。
		/// </summary>
		[DataMember]
		public TimeSpan HideAnimateTime { get; set; }
		/// <summary>
		/// 初期値として使用するグループID。
		/// </summary>
		[DataMember]
		public Guid DefaultGroupId { get; set; }

		/// <summary>
		/// テキスト表示を行うか。
		/// </summary>
		[DataMember]
		public bool TextVisible { get; set; }
		/// <summary>
		/// テキストの表示幅。
		/// </summary>
		[DataMember]
		public double TextWidth { get; set; }

		#endregion
	}
}
