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
			KeyActions.KeyActionId = KeyMappings.KeyActionId
		)
	inner join
		KeyOptions
		on
		(
			KeyActions.KeyActionId = KeyOptions.KeyActionId
		)
where
	KeyActions.KeyActionKind = @KeyActionKind
	and
	KeyActions.KeyActionContent in @KeyActionContents -- noqa: PRS
	and
	KeyOptions.KeyOptionName = 'LauncherItemId'
	and
	KeyOptions.KeyOptionValue = @LauncherItemId
order by
	KeyActions.UsageCount desc,
	KeyMappings.Sequence asc
