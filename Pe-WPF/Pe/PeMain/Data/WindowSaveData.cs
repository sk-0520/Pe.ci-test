namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	public class WindowSaveData: ItemModelBase
	{
		public WindowSaveData()
		{
			TimerItems = new FixeSizedList<WindowItemCollectionModel>();
			SystemItems = new FixeSizedList<WindowItemCollectionModel>();
		}

		#region property

		public WindowItemCollectionModel TemporaryItem { get; set; }

		/// <summary>
		/// タイマー保存。
		/// </summary>
		public FixeSizedList<WindowItemCollectionModel> TimerItems { get; private set; }
		/// <summary>
		/// 環境変更時の保存。
		/// </summary>
		public FixeSizedList<WindowItemCollectionModel> SystemItems { get; private set; }

		#endregion
	}
}
