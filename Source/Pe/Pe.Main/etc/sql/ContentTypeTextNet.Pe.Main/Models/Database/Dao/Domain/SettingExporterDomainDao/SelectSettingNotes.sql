select
	Notes.NoteId,
	Notes.Title,
	Notes.ScreenName,
	Notes.ForegroundColor,
	Notes.BackgroundColor,
	Notes.IsLocked,
	Notes.IsTopmost,
	Notes.IsCompact,
	Notes.ContentKind,
	Notes.HiddenMode,
	Notes.CaptionPosition,
	NoteContents.Encoding,
	NoteContents.Content
from
	Notes
	inner join
		NoteContents
		on
		(
			Notes.NoteId = NoteContents.NoteId
			and
			Notes.ContentKind = NoteContents.ContentKind
		)
order by
	Notes.Title,
	Notes.NoteId
