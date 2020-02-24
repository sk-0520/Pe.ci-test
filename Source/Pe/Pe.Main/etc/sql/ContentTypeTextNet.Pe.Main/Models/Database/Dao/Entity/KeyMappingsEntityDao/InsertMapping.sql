insert into
	KeyMappings
	(
		KeyActionId,
		Sequence,
		Key,
		Shift,
		Control,
		Alt,
		Super,

		CreatedTimestamp,
		CreatedAccount,
		CreatedProgramName,
		CreatedProgramVersion
	)
	values
	(
/* KeyActionId          */ @KeyActionId,
/* Sequence             */ @Sequence,
/* Key                  */ @Key,
/* Shift                */ @Shift,
/* Control              */ @Control,
/* Alt                  */ @Alt,
/* Super                */ @Super,

/* CreatedTimestamp     */ @CreatedTimestamp,
/* CreatedAccount       */ @CreatedAccount,
/* CreatedProgramName   */ @CreatedProgramName,
/*CreatedProgramVersion */ @CreatedProgramVersion
	)
