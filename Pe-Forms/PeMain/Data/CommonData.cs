namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using ContentTypeTextNet.Pe.Library.Skin;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic;

	/// <summary>
	/// 持ち運び用共通データ。
	/// </summary>
	public sealed class CommonData: IDisposable
	{
		/// <summary>
		/// 主設定。
		/// </summary>
		public MainSetting MainSetting { get; set; }
		/// <summary>
		/// 言語設定。
		/// </summary>
		public Language Language { get; set; }
		/// <summary>
		/// スキン。
		/// </summary>
		public ISkin Skin { get; set; }
		/// <summary>
		/// ロガー。
		/// </summary>
		public ILogger Logger { get; set; }
		/// <summary>
		/// メッセージ送信。
		/// </summary>
		public IRootSender RootSender { get; set; }
		/// <summary>
		/// データベースコネクション。
		/// 
		/// 入れるべきじゃない気がする。
		/// </summary>
		public AppDBManager Database { get; set; }

		public ApplicationSetting ApplicationSetting { get; set; }
		
		public void Dispose()
		{
			MainSetting.ToDispose();
			ApplicationSetting.ToDispose();
			Database.ToDispose();
		}
	}
}
