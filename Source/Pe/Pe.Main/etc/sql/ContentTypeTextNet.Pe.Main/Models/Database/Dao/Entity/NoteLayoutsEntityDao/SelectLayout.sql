select
	NoteLayouts.NoteId,
	NoteLayouts.LayoutKind,
	NoteLayouts.X,
	NoteLayouts.Y,
	NoteLayouts.Width,
	NoteLayouts.Height
from
	NoteLayouts
where
	NoteLayouts.NoteId = @NoteId
	and
	NoteLayouts.LayoutKind = @LayoutKind;

