update
	NoteFiles as AliasNoteFilesTarget
set
	[Sequence] = (
		select
			COUNT(AliasNoteFilesCurrent.Sequence) + 1
		from
			NoteFiles as AliasNoteFilesCurrent
		where
			AliasNoteFilesTarget.NoteId = AliasNoteFilesCurrent.NoteId
			and
			AliasNoteFilesCurrent.[Sequence] < AliasNoteFilesTarget.[Sequence]
	),

	UpdatedTimestamp      = @UpdatedTimestamp,
	UpdatedAccount        = @UpdatedAccount,
	UpdatedProgramName    = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount          = UpdatedCount + 1
where
	NoteId = @NoteId

