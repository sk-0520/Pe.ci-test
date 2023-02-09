select
	NoteFiles.NoteId,
	NoteFiles.FileKind,
	NoteFiles.FilePath,
	NoteFiles.NoteFileId,
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
