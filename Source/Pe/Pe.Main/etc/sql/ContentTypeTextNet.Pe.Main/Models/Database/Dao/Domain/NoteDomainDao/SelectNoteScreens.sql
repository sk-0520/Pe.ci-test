select
	Notes.NoteId,
	Notes.ScreenName,
	Screens.ScreenX,
	Screens.ScreenY,
	Screens.ScreenWidth,
	Screens.ScreenHeight
from
	Notes
	left join -- 全件ひっかける
		Screens
		on
		(
			Notes.ScreenName = Screens.ScreenName
		)
where
	Notes.NoteId = @NoteId
