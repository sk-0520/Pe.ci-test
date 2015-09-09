namespace ContentTypeTextNet.Pe.PeMain.Data.Temporary
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// 設定データ通知用クラス。
	/// <para>確定時に実ファイル操作などが必要な印として使用する。</para>
	/// </summary>
	public class SettingNotifiyData : NotifiyDataBase
	{
		#region property

		/// <summary>
		/// スタートアップに登録するか。
		/// </summary>
		public bool? StartupRegist { get; set; }

		#endregion
	}
}
