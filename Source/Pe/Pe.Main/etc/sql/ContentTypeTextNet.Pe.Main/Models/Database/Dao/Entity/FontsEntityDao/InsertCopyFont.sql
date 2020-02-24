insert into
	Fonts
	(
		[FontId],
		[FamilyName],
		[Height],
		[IsBold],
		[IsItalic],
		[IsUnderline],
		[IsStrikeThrough],

		[CreatedTimestamp],
		[CreatedAccount],
		[CreatedProgramName],
		[CreatedProgramVersion],
		[UpdatedTimestamp],
		[UpdatedAccount],
		[UpdatedProgramName],
		[UpdatedProgramVersion],
		[UpdatedCount]
	)
	select
/* FontId                   */ @DstFontId,
/* FamilyName               */ Fonts.FamilyName,
/* Height                   */ Fonts.Height,
/* IsBold                   */ Fonts.IsBold,
/* IsItalic                 */ Fonts.IsItalic,
/* IsUnderline              */ Fonts.IsUnderline,
/* IsStrikeThrough          */ Fonts.IsStrikeThrough,
/*                          */
/* CreatedTimestamp         */ @CreatedTimestamp,
/* CreatedAccount           */ @CreatedAccount,
/* CreatedProgramName       */ @CreatedProgramName,
/* CreatedProgramVersion    */ @CreatedProgramVersion,
/* UpdatedTimestamp         */ @UpdatedTimestamp,
/* UpdatedAccount           */ @UpdatedAccount,
/* UpdatedProgramName       */ @UpdatedProgramName,
/* UpdatedProgramVersion    */ @UpdatedProgramVersion,
/* UpdatedCount             */ 0
	from
		Fonts
	where
		Fonts.FontId = @SrcFontId
