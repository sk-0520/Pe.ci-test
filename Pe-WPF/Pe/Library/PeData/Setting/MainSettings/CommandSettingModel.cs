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
	using ContentTypeTextNet.Pe.Library.PeData.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	[Serializable]
	public class CommandSettingModel : SettingModelBase, IWindowStatus, IDeepClone
	{
		public CommandSettingModel()
			: base()
		{
			ShowHotkey = new HotKeyModel();
		}

		#region property

		/// <summary>
		/// アイコンサイズ。
		/// </summary>
		[DataMember]
		public IconScale IconScale { get; set; }
		/// <summary>
		/// 非表示になるまでの時間。
		/// </summary>
		[DataMember]
		public TimeSpan HideTime { get; set; }

		/// <summary>
		/// 呼び出しホットキー。
		/// </summary>
		[DataMember]
		public HotKeyModel ShowHotkey { get; set; }

		/// <summary>
		/// IDを検索対象にする。
		/// </summary>
		[DataMember]
		public bool FindId { get; set; }
		/// <summary>
		/// タグを検索対象にする。
		/// </summary>
		[DataMember]
		public bool FindTag { get; set; }
		/// <summary>
		/// ファイル検索を有効にする。
		/// </summary>
		[DataMember]
		public bool FindFile { get; set; }

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
		public bool TopMost { get; set; }

		#endregion

		#region IVisible

		[DataMember]
		public bool Visible { get; set; }

		#endregion

		#endregion

		#region IDeepClone

		public void DeepCloneTo(IDeepClone target)
		{
			var obj = (CommandSettingModel)target;

			obj.IconScale = IconScale;
			obj.HideTime = HideTime;
			ShowHotkey.DeepCloneTo(obj.ShowHotkey);
			obj.FindId = FindId;
			obj.FindTag = FindTag;
			obj.FindFile = FindFile;
			obj.WindowTop = WindowTop;
			obj.WindowLeft = WindowLeft;
			obj.WindowWidth = WindowWidth;
			obj.WindowHeight = WindowHeight;
			obj.WindowState = WindowState;
			obj.TopMost = TopMost;
			obj.Visible = Visible;
		}

		public IDeepClone DeepClone()
		{
			var result = new CommandSettingModel();

			DeepCloneTo(result);

			return result;
		}

		#endregion
	}
}
