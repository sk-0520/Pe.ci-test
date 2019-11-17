update
	NoteLayouts
set
	X = @X,
	Y = @Y,
	Width = @Width,
	Height = @Height,

	UpdatedTimestamp      = @UpdatedTimestamp,
	UpdatedAccount        = @UpdatedAccount,
	UpdatedProgramName    = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount          = UpdatedCount + 1
where
	NoteId = @NoteId
	and
	LayoutKind = @LayoutKind
