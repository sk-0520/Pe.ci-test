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
	values
	(
/* FontId                   */ @FontId,
/* FamilyName               */ @FamilyName,
/* Height                   */ @Height,
/* IsBold                   */ @IsBold,
/* IsItalic                 */ @IsItalic,
/* IsUnderline              */ @IsUnderline,
/* IsStrikeThrough          */ @IsStrikeThrough,
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
	)
