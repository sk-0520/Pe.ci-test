--// AppNotifyLogSetting
insert into
	AppNotifyLogSetting
	(
		IsVisible,
		Position,

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
/* IsVisible             */ true,
/* Position              */ 'right-bottom',

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

--// LauncherItems -> LauncherRedoItems
insert into
	LauncherRedoItems
	(
		LauncherItemId,
		RedoMode,
		WaitTime,
		RetryCount,

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
	select
		LauncherItems.LauncherItemId,
		'none',
		'0.00:00:01.0',
		1,

		@CreatedTimestamp,
		@CreatedAccount,
		@CreatedProgramName,
		@CreatedProgramVersion,
		@UpdatedTimestamp,
		@UpdatedAccount,
		@UpdatedProgramName,
		@UpdatedProgramVersion,
		@UpdatedCount
	from
		LauncherItems
	where
		LauncherItems.Kind = 'file'
;
