﻿namespace ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings
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
	public class TemplateSettingModel: SettingModelBase, IWindowStatus, IDeepClone
	{
		public TemplateSettingModel()
			: base()
		{
			ToggleHotKey = new HotKeyModel();
			Font = new FontModel();
		}

		#region property

		/// <summary>
		/// 表示非表示切り替え。
		/// </summary>
		[DataMember]
		public HotKeyModel ToggleHotKey { get; set; }

		/// <summary>
		/// リスト部の幅。
		/// </summary>
		[DataMember]
		public double ItemsListWidth { get; set; }

		/// <summary>
		/// 置き換えリスト部の幅。
		/// </summary>
		[DataMember]
		public double ReplaceListWidth { get; set; }

		[DataMember]
		public FontModel Font { get; set; }

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
			var obj = (TemplateSettingModel)target;

			ToggleHotKey.DeepCloneTo(obj.ToggleHotKey);
			obj.ItemsListWidth = ItemsListWidth;
			obj.ReplaceListWidth = ReplaceListWidth;
			obj.WindowTop = WindowTop;
			obj.WindowLeft = WindowLeft;
			obj.WindowWidth = WindowWidth;
			obj.WindowHeight = WindowHeight;
			obj.WindowState = WindowState;
			obj.IsTopmost = IsTopmost;
			obj.IsVisible = IsVisible;
			Font.DeepCloneTo(obj.Font);
		}

		public IDeepClone DeepClone()
		{
			var result = new TemplateSettingModel();

			DeepCloneTo(result);

			return result;
		}

		#endregion
	}
}
