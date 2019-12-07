--// AppExecuteSetting
insert into
	AppExecuteSetting
	(
		Accepted,
		FirstVersion,
		FirstTimestamp,
		LastVersion,
		LastTimestamp,
		ExecuteCount,
		UserId,
		SendUsageStatistics,

		CreatedTimestamp,
		CreatedAccount,
		CreatedProgramName,
		CreatedProgramVersion,
		UpdatedTimestamp,
		UpdatedAccount,
		UpdatedProgramName,
		UpdatedProgramVersion,
		UpdatedCount
	)
	values
	(
/* Accepted              */ false,
/* FirstVersion          */ '0.84.0',
/* FirstTimestamp        */ CURRENT_TIMESTAMP,
/* LastVersion           */ '0.84.0',
/* LastTimestamp         */ CURRENT_TIMESTAMP,
/* ExecuteCount          */ 1,
/* UserId                */ '',
/* SendUsageStatistics   */ false,

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
;












