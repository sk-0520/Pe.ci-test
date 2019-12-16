update
	Fonts
set
	FamilyName      = @FamilyName,
	Height          = @Height,
	IsBold          = @IsBold,
	IsItalic        = @IsItalic,
	IsUnderline     = @IsUnderline,
	IsStrikeThrough = @IsStrikeThrough,

	UpdatedTimestamp      = @UpdatedTimestamp,
	UpdatedAccount        = @UpdatedAccount,
	UpdatedProgramName    = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount          = UpdatedCount + 1
where
	FontId = @FontId
