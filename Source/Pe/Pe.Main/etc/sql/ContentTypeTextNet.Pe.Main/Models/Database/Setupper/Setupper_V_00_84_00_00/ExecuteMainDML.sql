--// Fonts: Note
insert into
	Fonts
	(
		FontId,
		FamilyName,
		Height,
		IsBold,
		IsItalic,
		IsUnderline,
		IsStrikeThrough,

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
/* FontId                */ @NoteDefaultFontId,
/* FamilyName            */ @NoteDefaultFamilyName,
/* Height                */ @NoteDefaultHeight,
/* IsBold                */ @NoteDefaultIsBold,
/* IsItalic              */ @NoteDefaultIsItalic,
/* IsUnderline           */ @NoteDefaultIsUnderline,
/* IsStrikeThrough       */ @NoteDefaultIsStrikeThrough,

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

--// Fonts: Command
insert into
	Fonts
	(
		FontId,
		FamilyName,
		Height,
		IsBold,
		IsItalic,
		IsUnderline,
		IsStrikeThrough,

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
/* FontId                */ @CommandFontId,
/* FamilyName            */ @CommandFamilyName,
/* Height                */ @CommandHeight,
/* IsBold                */ @CommandIsBold,
/* IsItalic              */ @CommandIsItalic,
/* IsUnderline           */ @CommandIsUnderline,
/* IsStrikeThrough       */ @CommandIsStrikeThrough,

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

--// Fonts: Stdio
insert into
	Fonts
	(
		FontId,
		FamilyName,
		Height,
		IsBold,
		IsItalic,
		IsUnderline,
		IsStrikeThrough,

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
/* FontId                */ @StdioFontId,
/* FamilyName            */ @StdioFamilyName,
/* Height                */ @StdioHeight,
/* IsBold                */ @StdioIsBold,
/* IsItalic              */ @StdioIsItalic,
/* IsUnderline           */ @StdioIsUnderline,
/* IsStrikeThrough       */ @StdioIsStrikeThrough,

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


--// Fonts: LauncherToolbar
insert into
	Fonts
	(
		FontId,
		FamilyName,
		Height,
		IsBold,
		IsItalic,
		IsUnderline,
		IsStrikeThrough,

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
/* FontId                */ @LauncherToolbarFontId,
/* FamilyName            */ @LauncherToolbarFamilyName,
/* Height                */ @LauncherToolbarHeight,
/* IsBold                */ @LauncherToolbarIsBold,
/* IsItalic              */ @LauncherToolbarIsItalic,
/* IsUnderline           */ @LauncherToolbarIsUnderline,
/* IsStrikeThrough       */ @LauncherToolbarIsStrikeThrough,

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
/* IgnoreVersion         */ '0.0.0',

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
/* FontId                */ @CommandFontId,
/* IconBox               */ 'small',
/* HideWaitTime          */ '0.00:00:03.0',
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
/*  FontId               */ @NoteDefaultFontId,
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
/*  FontId               */ @StdioFontId,
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

--// AppLauncherToolbarSetting
insert into
	AppLauncherToolbarSetting
	(
		[PositionKind],
		[Direction],
		[IconBox],
		[FontId],
		[AutoHideTimeout],
		[TextWidth],
		[IsVisible],
		[IsTopmost],
		[IsAutoHide],
		[IsIconOnly],

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
/* PositionKind             */ 'right',
/* Direction                */ 'left-top',
/* IconBox                  */ 'normal',
/* FontId                   */ @LauncherToolbarFontId,
/* AutoHideTimeout          */ '0.00:00:03.0',
/* TextWidth                */ 80,
/* IsVisible                */ true,
/* IsTopmost                */ true,
/* IsAutoHide               */ false,
/* IsIconOnly               */ true,

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

