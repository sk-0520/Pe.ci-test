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

		/// <summary>
		/// ツールバーの所属ディスプレイ名。
		/// </summary>
		public string Id { get; set; }
		/// <summary>
		/// ツールバー位置。
		/// </summary>
		public ToobarPosition ToobarPosition { get; set; }
		/// <summary>
		/// 表示。
		/// </summary>
		public bool Visible { get; set; }
		/// <summary>
		/// 最前面表示。
		/// </summary>
		public bool TopMost { get; set; }
		/// <summary>
		/// フロート状態。
		/// </summary>
		public FloatToolbarAreaItemModel FloatToolbarArea { get; set; }
		/// <summary>
		/// アイコンサイズ。
		/// </summary>
		public IconScale IconScale { get; set; }
		/// <summary>
		/// 自動的に隠すか。
		/// </summary>
		public bool AutoHide { get; set; }
		/// <summary>
		/// 自動的に隠すまでの時間。
		/// </summary>
		public TimeSpan HiddenWaitTime { get; set; }
		/// <summary>
		/// 自動的に隠す際のアニメーション時間。
		/// </summary>
		public TimeSpan HiddenAnimateTime { get; set; }
		/// <summary>
		/// 初期値として使用するグループID。
		/// </summary>
		public string DefaultGroupId { get; set; }
	}
}
