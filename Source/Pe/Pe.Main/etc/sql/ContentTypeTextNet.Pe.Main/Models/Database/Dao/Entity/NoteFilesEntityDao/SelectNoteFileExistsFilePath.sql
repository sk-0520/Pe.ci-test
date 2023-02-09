select
	NoteFiles.NoteFileId
from
	NoteFiles
where
	NoteFiles.NoteId = @NoteId
	and
	-- ほんまはパス文字列とか最後のディレクトリセパレータを補正する必要あり
	NoteFiles.FilePath = @FilePath
