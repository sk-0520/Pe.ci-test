insert into
	PluginVersionChecks
	(
		PluginId,
		Sequence,
		CheckUrl,

		CreatedTimestamp,
		CreatedAccount,
		CreatedProgramName,
		CreatedProgramVersion
	)
	values
	(
/* PluginId              */ @PluginId,
/* Sequence              */ @Sequence,
/* CheckUrl              */ @CheckUrl,

/* CreatedTimestamp      */ @CreatedTimestamp,
/* CreatedAccount        */ @CreatedAccount,
/* CreatedProgramName    */ @CreatedProgramName,
/* CreatedProgramVersion */ @CreatedProgramVersion
	)
