insert into
	InstallPlugins
	(
		PluginId,
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
/* ExtractedDirectoryPath   */ @ExtractedDirectoryPath,
/* PluginDirectoryPath      */ @PluginDirectoryPath,
/*                          */
/* CreatedTimestamp         */ @CreatedTimestamp,
/* CreatedAccount           */ @CreatedAccount,
/* CreatedProgramName       */ @CreatedProgramName,
/* CreatedProgramVersion    */ @CreatedProgramVersion
	)
