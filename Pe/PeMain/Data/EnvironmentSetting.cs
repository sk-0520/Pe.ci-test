using System;
using System.Collections.Generic;
using ContentTypeTextNet.Pe.Library.Utility;

namespace ContentTypeTextNet.Pe.PeMain.Data
{
	/// <summary>
	/// ランチャアイテム起動時の環境変数設定。
	/// 
	/// デフォルト環境変数(ON/OFF)に追加・変更を適用してから削除する。
	/// </summary>
	[Serializable]
	public class EnvironmentSetting: Item, ICloneable
	{
		public EnvironmentSetting()
		{
			EditEnvironment = false;
			Update = new List<TPair<string, string>>();
			Remove = new List<string>();
		}
		/// <summary>
		/// 
		/// </summary>
		public bool EditEnvironment { get; set; }
		/// <summary>
		/// 追加・変更対象
		/// </summary>
		public List<TPair<string, string>> Update { get; set; }
		/// <summary>
		/// 削除変数
		/// </summary>
		public List<string> Remove { get; set; }
		
		/// <summary>
		/// コピー。
		/// </summary>
		/// <returns>コピーされたオブジェクト</returns>
		public object Clone()
		{
			var result = new EnvironmentSetting();
			result.EditEnvironment = EditEnvironment;
			result.Update.AddRange(Update);
			result.Remove.AddRange(Remove);
			
			return result;
		}
	}
}
