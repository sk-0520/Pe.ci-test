insert into
	InstallPlugins
	(
		PluginId,
		PluginName,
		PluginVersion,
		PluginInstallMode,
		ExtractedDirectoryPath,
		PluginDirectoryPath,

		[CreatedTimestamp],
		[CreatedAccount],
		[CreatedProgramName],
		[CreatedProgramVersion]
	)
	values
	(
/* PluginId                 */ @PluginId,
/* PluginName               */ @PluginName,
/* PluginVersion            */ @PluginVersion,
/* PluginInstallMode        */ @PluginInstallMode,
/* ExtractedDirectoryPath   */ @ExtractedDirectoryPath,
/* PluginDirectoryPath      */ @PluginDirectoryPath,
/*                          */
/* CreatedTimestamp         */ @CreatedTimestamp,
/* CreatedAccount           */ @CreatedAccount,
/* CreatedProgramName       */ @CreatedProgramName,
/* CreatedProgramVersion    */ @CreatedProgramVersion
	)
