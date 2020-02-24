select
	Fonts.FontId,
	Fonts.FamilyName,
	Fonts.Height,
	Fonts.IsBold,
	Fonts.IsItalic,
	Fonts.IsUnderline,
	Fonts.IsStrikeThrough
from
	Fonts
where
	Fonts.FontId = @FontId
