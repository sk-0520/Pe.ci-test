insert into
	KeyOptions
	(
		KeyActionId,
		KeyOptionName,
		KeyOptionValue,

		CreatedTimestamp,
		CreatedAccount,
		CreatedProgramName,
		CreatedProgramVersion
	)
	values
	(
/* KeyActionId          */ @KeyActionId,
/* KeyOptionName        */ @KeyOptionName,
/* KeyOptionValue       */ @KeyOptionValue,

/* CreatedTimestamp     */ @CreatedTimestamp,
/* CreatedAccount       */ @CreatedAccount,
/* CreatedProgramName   */ @CreatedProgramName,
/*CreatedProgramVersion */ @CreatedProgramVersion
	)
