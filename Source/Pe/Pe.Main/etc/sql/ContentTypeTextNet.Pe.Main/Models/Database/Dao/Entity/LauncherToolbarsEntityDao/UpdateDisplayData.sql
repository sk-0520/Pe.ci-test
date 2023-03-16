update
	LauncherToolbars
set
	LauncherGroupId       = @LauncherGroupId,
	PositionKind          = @PositionKind,
	Direction             = @Direction,
	IconBox               = @IconBox,
	FontId                = @FontId,
	DisplayDelayTime      = @DisplayDelayTime,
	AutoHideTime          = @AutoHideTime,
	TextWidth             = @TextWidth,
	IsVisible             = @IsVisible,
	IsTopmost             = @IsTopmost,
	IsAutoHide            = @IsAutoHide,
	IsIconOnly            = @IsIconOnly,

	UpdatedTimestamp      = @UpdatedTimestamp,
	UpdatedAccount        = @UpdatedAccount,
	UpdatedProgramName    = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount          = UpdatedCount + 1
where
	LauncherToolbarId = @LauncherToolbarId
