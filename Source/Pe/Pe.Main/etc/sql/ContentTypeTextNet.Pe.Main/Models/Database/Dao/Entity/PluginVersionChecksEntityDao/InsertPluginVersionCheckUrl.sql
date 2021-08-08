insert into
	PluginVersionChecks
	(
		PluginId,
		Sequence,
		VersionCheckUrl,
		CreatedTimestamp,
		CreatedAccount,
		CreatedProgramName,
		CreatedProgramVersion
	)
	values
	(
/* PluginId              */ @PluginId,
/* Sequence              */ @Sequence,
/* VersionCheckUrl       */ @VersionCheckUrl,
/* CreatedTimestamp      */ @CreatedTimestamp,
/* CreatedAccount        */ @CreatedAccount,
/* CreatedProgramName    */ @CreatedProgramName,
/* CreatedProgramVersion */ @CreatedProgramVersion
	)
