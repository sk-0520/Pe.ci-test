insert into
	LauncherGroups
	(
		[LauncherGroupId],
		[Name],
		[Kind],
		[ImageName],
		[ImageColor],
		[Sort],

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
/* LauncherGroupId          */ (
	select
		printf(
			'%s-%s-%s-%s-%s',
				lower(hex(randomblob(4))),
				lower(hex(randomblob(2))),
				lower(hex(randomblob(2))),
				lower(hex(randomblob(2))),
				lower(hex(randomblob(6)))
		)
	),
/* Name                     */ @Name,
/* Kind                     */ 'normal',
/* ImageName                */ 'directory',
/* ImageColor               */ '#ff000000',
/* Sort                     */ 10,
/*                          */
/* CreatedTimestamp         */ CURRENT_TIMESTAMP,
/* CreatedAccount           */ @CreatedAccount,
/* CreatedProgramName       */ @CreatedProgramName,
/* CreatedProgramVersion    */ @CreatedProgramVersion,
/* UpdatedTimestamp         */ CURRENT_TIMESTAMP,
/* UpdatedAccount           */ @UpdatedAccount,
/* UpdatedProgramName       */ @UpdatedProgramName,
/* UpdatedProgramVersion    */ @UpdatedProgramVersion,
/* UpdatedCount             */ 0
	)
