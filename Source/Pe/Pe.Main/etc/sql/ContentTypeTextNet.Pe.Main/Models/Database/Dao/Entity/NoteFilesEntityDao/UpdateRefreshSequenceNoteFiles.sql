update
	NoteFiles as alias_NoteFiles_target
set
	[Sequence] = (
		select
			COUNT(alias_NoteFiles_current.Sequence) + 1
		from
			NoteFiles as alias_NoteFiles_current
		where
			alias_NoteFiles_target.NoteId = alias_NoteFiles_current.NoteId
			and
			alias_NoteFiles_current.[Sequence] < alias_NoteFiles_target.[Sequence]
	),

	UpdatedTimestamp      = @UpdatedTimestamp,
	UpdatedAccount        = @UpdatedAccount,
	UpdatedProgramName    = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount          = UpdatedCount + 1
where
	NoteId = @NoteId

