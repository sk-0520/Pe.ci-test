﻿namespace ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings
{
	using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.IF;

	public class NoteSettingModel: SettingModelBase, IColorPair
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

		#region IColorPair

		[DataMember]
		public Color ForeColor { get; set; }
		[DataMember]
		public Color BackColor { get; set; }

		#endregion

	}
}
