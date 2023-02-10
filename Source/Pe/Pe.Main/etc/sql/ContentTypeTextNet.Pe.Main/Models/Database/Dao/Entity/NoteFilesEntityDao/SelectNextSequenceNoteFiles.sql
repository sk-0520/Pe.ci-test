select
	IFNULL(NoteFile.Sequence, 0) as Sequence
from
	(
		select
			MAX(NoteFiles.Sequence) + 1 as Sequence
		from
			NoteFiles
		where
			NoteFiles.NoteId = @NoteId
	) as NoteFile
