namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// ILoggerで使用する設定。
	/// </summary>
	[DataContract, Serializable]
	public class LoggerConfigModel : ModelBase
	{
		/// <summary>
		/// デバッグ情報をログ対象とするか。
		/// </summary>
		[DataMember]
		public bool EnabledDebug { get; set; }
		/// <summary>
		/// トレース情報をログ対象とするか。
		/// </summary>
		[DataMember]
		public bool EnabledTrace { get; set; }
		/// <summary>
		/// 操作情報をログ対象とするか。
		/// </summary>
		[DataMember]
		public bool EnabledInformation { get; set; }
		/// <summary>
		/// 注意をログ対象とするか。
		/// </summary>
		[DataMember]
		public bool EnabledWarning { get; set; }
		/// <summary>
		/// エラーをログ対象とするか。
		/// </summary>
		[DataMember]
		public bool EnabledError { get; set; }
		/// <summary>
		/// 異常をログ対象とするか。
		/// </summary>
		[DataMember]
		public bool EnabledFatal { get; set; }

		/// <summary>
		/// すべて。
		/// </summary>
		public bool EnabledAll
		{
			get 
			{
				return EnabledDebug
					&& EnabledTrace
					&& EnabledInformation
					&& EnabledWarning
					&& EnabledError
					&& EnabledFatal
				;
			}
			set
			{
				EnabledDebug 
					= EnabledTrace
					= EnabledInformation
					= EnabledWarning
					= EnabledError
					= EnabledFatal
					= value
				;
			}
		}
	}
}
