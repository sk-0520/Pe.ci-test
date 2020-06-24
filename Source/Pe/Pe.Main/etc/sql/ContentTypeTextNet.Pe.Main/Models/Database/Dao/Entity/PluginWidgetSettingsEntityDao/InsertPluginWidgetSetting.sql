insert into
	PluginWidgetSettings
	(
		PluginId,
		X,
		Y,
		Width,
		Height,
		IsVisible,
		IsTopmost,

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
/* Y                     */ @X,
/* X                     */ @Y,
/* Width                 */ @Width,
/* Height                */ @Height,
/* IsVisible             */ @IsVisible,
/* IsTopmost             */ @IsTopmost,

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
