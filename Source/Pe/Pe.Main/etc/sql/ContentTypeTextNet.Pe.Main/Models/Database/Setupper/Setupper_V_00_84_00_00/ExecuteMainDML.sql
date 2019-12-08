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


--// AppGeneralSetting
insert into
	AppGeneralSetting
	(
		Language,

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
/* Language              */ '',

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


--// AppUpdateSetting
insert into
	AppUpdateSetting
	(
		CheckReleaseVersion,
		CheckRcVersion,
		IgnoreVersion,

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
/* CheckReleaseVersion   */ false,
/* CheckRcVersion        */ false,
/* IgnoreVersion         */ '0.0.0.0',

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

--// AppWindowSetting
insert into
	AppWindowSetting
	(
		IsEnabled,
		Count,
		Interval,

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
/* IsEnabled             */ true,
/* Count                 */ 10,
/* Interval              */ '0.00:10.00.0',

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


--// AppCommandSetting
insert into
	AppCommandSetting
	(
		FontId,
		IconBox,
		HideWaitTime,
		FindTag,
		FindFile,

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
/* FontId                */ '00000000-0000-0000-0000-000000000000',
/* IconBox               */ 'small',
/* HideWaitTime          */ '0.00:00.05.0',
/* FindTag               */ true,
/* FindFile              */ true,

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


--// AppNoteSetting
insert into
	AppNoteSetting
	(
		FontId,
		TitleKind,
		LayoutKind,
		Foreground,
		Background,
		IsTopmost,

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
/*  FontId               */ '00000000-0000-0000-0000-000000000000',
/*  TitleKind            */ 'timestamp',
/*  LayoutKind           */ 'relative',
/*  Foreground           */ '#ff000000',
/*  Background           */ '#ffffffaa',
/*  IsTopmost            */ false,

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

--// AppStandardInputOutputSetting
insert into
	AppStandardInputOutputSetting
	(
		FontId,
		OutputForeground,
		OutputBackground,
		ErrorForeground,
		ErrorBackground,
		IsTopmost,

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
/*  FontId               */ '00000000-0000-0000-0000-000000000000',
/*  OutputForeground     */ '#ffffffff',
/*  OutputBackground     */ '#ff000000',
/*  ErrorForeground      */ '#ffff0000',
/*  ErrorBackground      */ '#ff000000',
/*  IsTopmost            */ false,

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


