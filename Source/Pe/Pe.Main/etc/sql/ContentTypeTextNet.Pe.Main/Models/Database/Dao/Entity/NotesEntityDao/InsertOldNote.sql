
insert into
	Notes
	(
		NoteId,
		ScreenName,
		IsVisible,
		IsLocked,
		IsCompact,
		TextWrap,
		ContentKind,

		Title,
		LayoutKind,
		FontId,
		ForegroundColor,
		BackgroundColor,
		IsTopmost,

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
/* NoteId                */ @NoteId,
/* ScreenName            */ @ScreenName,
/* IsVisible             */ @IsVisible,
/* IsLocked              */ @IsLocked,
/* IsCompact             */ @IsCompact,
/* TextWrap              */ @TextWrap,
/* ContentKind           */ @ContentKind,

/* Title                 */ @Title,
/* LayoutKind            */ @LayoutKind,
/* FontId                */ @FontId,
/* ForegroundColor       */ @ForegroundColor,
/* BackgroundColor       */ @BackgroundColor,
/* IsTopmost             */ @IsTopmost,
/*                       */
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
