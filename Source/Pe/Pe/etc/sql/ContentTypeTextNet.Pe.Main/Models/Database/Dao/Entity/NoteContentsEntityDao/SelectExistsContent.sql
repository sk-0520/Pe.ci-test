select
	count(*) = 1
from
	NoteContents
where
	NoteContents.NoteId = @NoteId

