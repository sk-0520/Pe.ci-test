select
	COUNT(1) = 1
from
	NoteLayouts
where
	NoteLayouts.NoteId = @NoteId
	and
	NoteLayouts.LayoutKind = @LayoutKind;

