--// insert: AppNoteHiddenSetting
insert into
	[AppNoteHiddenSetting]
	(
		[CreatedTimestamp],
		[CreatedAccount],
		[CreatedProgramName],
		[CreatedProgramVersion],
		[UpdatedTimestamp],
		[UpdatedAccount],
		[UpdatedProgramName],
		[UpdatedProgramVersion],
		[UpdatedCount],
		[HiddenMode],
		[WaitTime]
	)
	values
	(
/* CreatedTimestamp       */ @CreatedTimestamp,
/* CreatedAccount         */ @CreatedAccount,
/* CreatedProgramName     */ @CreatedProgramName,
/* CreatedProgramVersion  */ @CreatedProgramVersion,
/* UpdatedTimestamp       */ @UpdatedTimestamp,
/* UpdatedAccount         */ @UpdatedAccount,
/* UpdatedProgramName     */ @UpdatedProgramName,
/* UpdatedProgramVersion  */ @UpdatedProgramVersion,
/* UpdatedCount           */ 0,
/* HiddenMode             */ 'blind',
/* WaitTime               */ '0.00:00:30.0'
	),
	(
/* CreatedTimestamp       */ @CreatedTimestamp,
/* CreatedAccount         */ @CreatedAccount,
/* CreatedProgramName     */ @CreatedProgramName,
/* CreatedProgramVersion  */ @CreatedProgramVersion,
/* UpdatedTimestamp       */ @UpdatedTimestamp,
/* UpdatedAccount         */ @UpdatedAccount,
/* UpdatedProgramName     */ @UpdatedProgramName,
/* UpdatedProgramVersion  */ @UpdatedProgramVersion,
/* UpdatedCount           */ 0,
/* HiddenMode             */ 'compact',
/* WaitTime               */ '0.00:01:00.0'
	)
;
