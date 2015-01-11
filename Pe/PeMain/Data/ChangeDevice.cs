using System.Windows.Forms;
using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;

namespace ContentTypeTextNet.Pe.PeMain.Data
{
	/// <summary>
	/// デバイス情報変更。
	/// </summary>
	public class ChangeDevice
	{
		/// <summary>
		/// メッセージデータ。
		/// </summary>
		private readonly Message _message;
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public ChangeDevice(Message message)
		{
			this._message = message;
			
			DBT = (DBT)this._message.WParam.ToInt32();
		}
		
		/// <summary>
		/// DBT! DBT!
		/// </summary>
		public DBT DBT { get; private set; }
		
		
	}
}
