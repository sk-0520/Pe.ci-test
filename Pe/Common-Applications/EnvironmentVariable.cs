using System;
using System.Collections.Generic;

namespace ContentTypeTextNet.Pe.Applications
{
	public static class EnvironmentVariableLiteral
	{
		public const string systemExecuteFilePath = "PE_SYS_APP_EXE";
		public const string systemDirectoryPath = "PE_SYS_APP_DIR";
		public const string systemSettingDirectoryPath = "PE_SYS_SETTING_DIR";

		public const string applicationSettingBaseDirectoryPath = "PE_APP_SETTING_BASE_DIR";
		public const string applicationSettingDirectoryPath = "PE_APP_SETTING_DIR";
	}

	public class EnvironmentVariableDictionary: Dictionary<string, string>, IReadOnlyDictionary<string, string>
	{
		public EnvironmentVariableDictionary()
		{
		}

		public EnvironmentVariableDictionary(IDictionary<string, string> app)
		{
			foreach(var pair in app) {
				Add(pair.Key, pair.Value);
			}
		}
	}
}
