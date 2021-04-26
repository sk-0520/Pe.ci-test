select
	KeyActions.KeyActionId,
	KeyMappings.Sequence,
	KeyMappings.Key,
	KeyMappings.Shift,
	KeyMappings.Control,
	KeyMappings.Alt,
	KeyMappings.Super
from
	KeyActions
	inner join
		KeyMappings
		on
		(
			KeyMappings.KeyActionId = KeyActions.KeyActionId
		)
	inner join
		KeyOptions
		on
		(
			KeyOptions.KeyActionId = KeyActions.KeyActionId
		)
where
	KeyActions.KeyActionKind = @KeyActionKind
	and
	KeyActions.KeyActionContent in @KeyActionContents
	and
	KeyOptions.KeyOptionName = 'LauncherItemId'
	and
	KeyOptions.KeyOptionValue = @LauncherItemId
order by
	KeyActions.UsageCount desc,
	KeyMappings.Sequence asc
