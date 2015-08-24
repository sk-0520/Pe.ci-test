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
	public class ToolbarItemModel: ModelBase, IVisible, ITopMost, ITId<string>, IDeepClone
	{
		public ToolbarItemModel()
			: base()
		{
			FloatToolbar = new FloatToolbarItemModel();
			Font = new FontModel();
		}

		#region property

		/// <summary>
		/// ツールバー位置。
		/// </summary>
		[DataMember]
		public DockType DockType { get; set; }

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

		[DataMember]
		public FontModel Font { get; set; }

		/// <summary>
		/// メニューボタンの位置を補正する。
		/// <para>右側にツールバー設置時に左側にメニューボタンを配置する。</para>
		/// </summary>
		[DataMember]
		public bool MenuPositionCorrection { get; set; }

		#endregion

		#region IVisible

		/// <summary>
		/// 表示。
		/// </summary>
		[DataMember]
		public bool IsVisible { get; set; }

		#endregion

		#region ITopMost

		/// <summary>
		/// 最前面表示。
		/// </summary>
		[DataMember]
		public bool IsTopmost { get; set; }

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

		#region IDeepClone

		public void DeepCloneTo(IDeepClone target)
		{
			var obj = (ToolbarItemModel)target;

			obj.DockType = DockType;
			FloatToolbar.DeepCloneTo(obj.FloatToolbar);
			obj.IconScale = IconScale;
			obj.AutoHide = AutoHide;
			obj.HideWaitTime = HideWaitTime;
			obj.HideAnimateTime = HideAnimateTime;
			obj.DefaultGroupId = DefaultGroupId;
			obj.TextVisible = TextVisible;
			obj.TextWidth = TextWidth;
			obj.IsVisible = IsVisible;
			obj.IsTopmost = IsTopmost;
			obj.Id = Id;
			Font.DeepCloneTo(obj.Font);
		}
	
		public IDeepClone DeepClone()
		{
			var result = new ToolbarItemModel();

			DeepCloneTo(result);

			return result;
		}

		#endregion
	}
}
