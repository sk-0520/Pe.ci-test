insert into
	LauncherItemIcons
	(
		[LauncherItemId],
		[IconBox],
		[IconScale],
		[Sequence],
		[Image],

		[CreatedTimestamp],
		[CreatedAccount],
		[CreatedProgramName],
		[CreatedProgramVersion]
	)
	values
	(
/* LauncherItemId           */ @LauncherItemId,
/* IconBox                  */ @IconBox,
/* IconScale                */ @IconScale,
/* Sequence                 */ @Sequence,
/* Image                    */ @Image,

/* CreatedTimestamp         */ @CreatedTimestamp,
/* CreatedAccount           */ @CreatedAccount,
/* CreatedProgramName       */ @CreatedProgramName,
/* CreatedProgramVersion    */ @CreatedProgramVersion
	)
