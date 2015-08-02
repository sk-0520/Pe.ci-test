namespace ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.Attribute;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	[Serializable]
	public class ClipboardSettingModel : SettingModelBase, IWindowStatus, IDeepClone
	{
		public ClipboardSettingModel()
			: base()
		{
			ToggleHotKey = new HotKeyModel();
		}

		#region property

		/// <summary>
		/// クリップボード監視の変更を検知するか。
		/// </summary>
		[DataMember]
		public bool Enabled { get; set; }

		/// <summary>
		/// アプリケーション内でのコピー操作も監視対象とするか。
		/// </summary>
		[DataMember]
		public bool EnabledApplicationCopy { get; set; }

		/// <summary>
		/// 表示非表示切り替え。
		/// </summary>
		[DataMember]
		public HotKeyModel ToggleHotKey { get; set; }

		/// <summary>
		/// 取り込み対象。
		/// </summary>
		[DataMember]
		public ClipboardType EnabledClipboardTypes { get; set; }

		/// <summary>
		/// 履歴数。
		/// </summary>
		[DataMember]
		public int SaveCount { get; set; }

		/// <summary>
		/// 待機時間。
		/// </summary>
		[DataMember]
		public TimeSpan WaitTime { get; set; }

		/// <summary>
		/// 重複判定。
		/// </summary>
		[DataMember]
		public int DuplicationCount { get; set; }

		/// <summary>
		/// 転送にクリップボードを使用する。
		/// </summary>
		[DataMember]
		public bool UsingClipboard { get; set; }

		/// <summary>
		/// リスト部の幅。
		/// </summary>
		[DataMember]
		public double ItemsListWidth { get; set; }

		#endregion

		#region IWindowStatus

		[DataMember]
		[PixelKind(Px.Logical)]
		public double WindowTop { get; set; }
		[DataMember]
		[PixelKind(Px.Logical)]
		public double WindowLeft { get; set; }
		[DataMember]
		[PixelKind(Px.Logical)]
		public double WindowWidth { get; set; }
		[DataMember]
		[PixelKind(Px.Logical)]
		public double WindowHeight { get; set; }
		[DataMember]
		[PixelKind(Px.Logical)]
		public WindowState WindowState { get; set; }

		#region ITopMost

		[DataMember]
		public bool IsTopmost { get; set; }

		#endregion

		#region IVisible

		[DataMember]
		public bool IsVisible { get; set; }

		#endregion

		#endregion

		#region IDeepClone

		public void DeepCloneTo(IDeepClone target)
		{
			var obj = (ClipboardSettingModel)target;

			obj.Enabled = Enabled;
			obj.EnabledApplicationCopy = EnabledApplicationCopy;
			ToggleHotKey.DeepCloneTo(obj.ToggleHotKey);
			obj.EnabledClipboardTypes = EnabledClipboardTypes;
			obj.SaveCount = SaveCount;
			obj.WaitTime = WaitTime;
			obj.DuplicationCount = DuplicationCount;
			obj.UsingClipboard = UsingClipboard;
			obj.ItemsListWidth = ItemsListWidth;
			obj.WindowTop = WindowTop;
			obj.WindowLeft = WindowLeft;
			obj.WindowWidth = WindowWidth;
			obj.WindowHeight = WindowHeight;
			obj.WindowState = WindowState;
			obj.IsTopmost = IsTopmost;
			obj.IsVisible = IsVisible;
		}

		public IDeepClone DeepClone()
		{
			var result = new ClipboardSettingModel();

			DeepCloneTo(result);

			return result;
		}

		#endregion
	}
}
