using System;
using System.Collections.Generic;

namespace ContentTypeTextNet.Pe.Applications
{
	/// <summary>
	/// 環境変数名。
	/// </summary>
	public static class EVLiteral
	{
		public const string systemExecuteFilePath = "PE_SYS_APP_EXE";
		public const string systemDirectoryPath = "PE_SYS_APP_DIR";
		public const string systemSettingDirectoryPath = "PE_SYS_SETTING_DIR";
		public const string systemLogDirectoryPath = "PE_SYS_LOG_DIR";

		public const string applicationSettingDirectoryPath = "PE_APP_SETTING_DIR";
		public const string applicationLogDirectoryPath = "PE_APP_LOG_DIR";

		public const string communicationEventName = "PE_C_EVENT";
		public const string communicationServerName = "PE_C_SEVER";
		/// <summary>
		/// 環境変数名一覧。
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<string> GetEVNames()
		{
			return new [] {
				systemExecuteFilePath,
				systemDirectoryPath,
				systemSettingDirectoryPath,
				systemLogDirectoryPath,
				// --------------------------------------
				applicationSettingDirectoryPath,
				applicationLogDirectoryPath,
				// --------------------------------------
				communicationEventName,
				communicationServerName,
			};
		}
	}

	/// <summary>
	/// Pe環境変数。
	/// </summary>
	public class EVDictionary: Dictionary<string, string>
	{
		/// <summary>
		/// 呼び出されるアプリケーション側で実行。
		/// </summary>
		public EVDictionary()
		{
			foreach(var name in EVLiteral.GetEVNames()) {
				var value = Environment.GetEnvironmentVariable(name);
				Add(name, value);
			}
		}
	}
}
