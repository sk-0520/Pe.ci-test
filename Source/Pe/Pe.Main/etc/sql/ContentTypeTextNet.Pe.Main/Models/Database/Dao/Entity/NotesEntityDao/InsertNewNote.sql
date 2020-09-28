
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
		HiddenMode,

		Title,
		LayoutKind,
		FontId,
		ForegroundColor,
		BackgroundColor,
		IsTopmost,
		CaptionPosition,

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
	select
/* NoteId                */ @NoteId,
/* ScreenName            */ @ScreenName,
/* IsVisible             */ @IsVisible,
/* IsLocked              */ @IsLocked,
/* IsCompact             */ @IsCompact,
/* TextWrap              */ @TextWrap,
/* ContentKind           */ @ContentKind,
/* HiddenMode            */ @HiddenMode,

/* Title                 */
		case AppNoteSetting.TitleKind
			when 'timestamp' then
				strftime('%Y/%m/%d %H:%M:%S', CURRENT_TIMESTAMP, 'localtime')
			else
				(select count(NoteId) + 1 from Notes)
		end,
/* LayoutKind            */ AppNoteSetting.LayoutKind,
/* FontId                */ AppNoteSetting.FontId,
/* ForegroundColor       */ AppNoteSetting.ForegroundColor,
/* BackgroundColor       */ AppNoteSetting.BackgroundColor,
/* IsTopmost             */ AppNoteSetting.IsTopmost,
/* CaptionPosition       */ AppNoteSetting.CaptionPosition,
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
	from
		AppNoteSetting
