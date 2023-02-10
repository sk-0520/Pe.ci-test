select
	NoteFiles.NoteId,
	NoteFiles.NoteFileId,
	NoteFiles.FileKind,
	NoteFiles.FilePath,
	NoteFiles.Sequence,
	
	NoteFiles.CreatedTimestamp,
	NoteFiles.CreatedAccount,
	NoteFiles.CreatedProgramName,
	NoteFiles.CreatedProgramVersion,
	NoteFiles.UpdatedTimestamp,
	NoteFiles.UpdatedAccount,
	NoteFiles.UpdatedProgramName,
	NoteFiles.UpdatedProgramVersion,
	NoteFiles.UpdatedCount
from
	NoteFiles
where
	NoteFiles.NoteId = @NoteId
order by
	NoteFiles.Sequence,
	NoteFiles.FilePath
