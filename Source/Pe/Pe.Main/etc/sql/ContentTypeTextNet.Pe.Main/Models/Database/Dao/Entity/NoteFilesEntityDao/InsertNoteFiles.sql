insert into
	NoteFiles
	(
		NoteId,
		NoteFileId,
		FileKind,
		FilePath,
		Sequence,

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
/* NoteId                */ @NoteId,
/* NoteFileId            */ @NoteFileId,
/* FileKind              */ @FileKind,
/* FilePath              */ @FilePath,
/* Sequence              */ @Sequence,
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


