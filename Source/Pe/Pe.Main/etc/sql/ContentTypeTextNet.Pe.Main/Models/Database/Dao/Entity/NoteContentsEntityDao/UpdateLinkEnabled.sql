update
	NoteContents
set
	IsLink                = @IsLink,
	Address               = @Address,
	Encoding              = @Encoding,
	DelayTime             = @DelayTime,
	BufferSize            = @BufferSize,
	RefreshTime           = @RefreshTime,
	IsEnabledRefresh      = @IsEnabledRefresh,

	UpdatedTimestamp      = @UpdatedTimestamp,
	UpdatedAccount        = @UpdatedAccount,
	UpdatedProgramName    = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount          = UpdatedCount + 1
where
	NoteId = @NoteId
