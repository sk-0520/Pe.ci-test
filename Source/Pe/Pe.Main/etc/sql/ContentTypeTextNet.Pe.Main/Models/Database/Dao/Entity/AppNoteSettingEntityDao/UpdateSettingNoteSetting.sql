update
	AppNoteSetting
set
	FontId                = @FontId,
	TitleKind             = @TitleKind,
	LayoutKind            = @LayoutKind,
	Foreground            = @ForegroundColor,
	Background            = @BackgroundColor,
	IsTopmost             = @IsTopmost,

	UpdatedTimestamp      = @UpdatedTimestamp,
	UpdatedAccount        = @UpdatedAccount,
	UpdatedProgramName    = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount          = UpdatedCount + 1
