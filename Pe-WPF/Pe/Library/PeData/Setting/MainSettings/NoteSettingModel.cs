namespace ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	public class NoteSettingModel: SettingModelBase
	{
		public NoteSettingModel()
			: base()
		{
			CreateHotKey = new HotKeyModel();
			HideHotKey = new HotKeyModel();
			CompactHotKey = new HotKeyModel();
			ShowFrontHotKey = new HotKeyModel();
		}

		#region property

		/// <summary>
		/// 新規作成時のホットキー
		/// </summary>
		[DataMember]
		public HotKeyModel CreateHotKey { get; set; }
		[DataMember]
		public HotKeyModel HideHotKey { get; set; }
		[DataMember]
		public HotKeyModel CompactHotKey { get; set; }
		[DataMember]
		public HotKeyModel ShowFrontHotKey { get; set; }

		#endregion
	}
}
