insert into
	Screens
	(
		[ScreenName],
		[ScreenX],
		[ScreenY],
		[ScreenWidth],
		[ScreenHeight],

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
/* ScreenName               */ @ScreenName,
/* ScreenX                  */ @ScreenX,
/* ScreenY                  */ @ScreenY,
/* ScreenWidth              */@ScreenWidth,
/* ScreenHeight             */ @ScreenHeight,
/*                          */
/* CreatedTimestamp         */ @CreatedTimestamp,
/* CreatedAccount           */ @CreatedAccount,
/* CreatedProgramName       */ @CreatedProgramName,
/* CreatedProgramVersion    */ @CreatedProgramVersion,
/* UpdatedTimestamp         */ @UpdatedTimestamp,
/* UpdatedAccount           */ @UpdatedAccount,
/* UpdatedProgramName       */ @UpdatedProgramName,
/* UpdatedProgramVersion    */ @UpdatedProgramVersion,
/* UpdatedCount             */ 0
	)
