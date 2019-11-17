update
	NoteLayouts
set
	X = case @isEnabledLocation
		when 1 then @X
		else X
		end,
	Y = case @isEnabledLocation
		when 1 then @Y
		else Y
	end,
	Width = case @isEnabledSize
		when 1 then @Width
		else Width
	end,
	Height = case @isEnabledSize
		when 1 then @Height
		else Height
	end,

	UpdatedTimestamp      = @UpdatedTimestamp,
	UpdatedAccount        = @UpdatedAccount,
	UpdatedProgramName    = @UpdatedProgramName,
	UpdatedProgramVersion = @UpdatedProgramVersion,
	UpdatedCount          = UpdatedCount + 1
where
	NoteId = @NoteId
	and
	LayoutKind = @LayoutKind
