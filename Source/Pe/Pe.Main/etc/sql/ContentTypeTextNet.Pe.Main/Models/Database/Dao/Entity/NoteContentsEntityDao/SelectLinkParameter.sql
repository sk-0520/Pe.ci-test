select
	NoteContents.NoteId,
	NoteContents.IsLink,
	NoteContents.Address,
	NoteContents.Encoding,
	NoteContents.DelayTime,
	NoteContents.BufferSize,
	NoteContents.RefreshTime,
	NoteContents.IsEnabledRefresh
from
	NoteContents
where
	NoteContents.NoteId = @NoteId

