insert into
	Plugins
	(
		PluginId,
		Name,
		State,

		LastUseTimestamp,
		LastUsePluginVersion,
		LastUseAppVersion,
		ExecuteCount,


		[CreatedTimestamp],
		[CreatedAccount],
		[CreatedProgramName],
		[CreatedProgramVersion],
		[UpdatedTimestamp],
		[UpdatedAccount],
		[UpdatedProgramName],
		[UpdatedProgramVersion],
		[UpdatedCount]

	)
	values
	(
/* PluginId              */ @PluginId,
/* Name                  */ @Name,
/* State                 */ @State,

/* LastUseTimestamp      */ '0000-01-01T00:00:00.000Z',
/* LastUsePluginVersion  */ '0.0.0',
/* LastUseAppVersion     */ '0.0.0',
/* ExecuteCount          */ 0,

/*                       */
/* CreatedTimestamp      */ @CreatedTimestamp,
/* CreatedAccount        */ @CreatedAccount,
/* CreatedProgramName    */ @CreatedProgramName,
/* CreatedProgramVersion */ @CreatedProgramVersion,
/* UpdatedTimestamp      */ @UpdatedTimestamp,
/* UpdatedAccount        */ @UpdatedAccount,
/* UpdatedProgramName    */ @UpdatedProgramName,
/* UpdatedProgramVersion */ @UpdatedProgramVersion,
/* UpdatedCount          */ 0
	)
