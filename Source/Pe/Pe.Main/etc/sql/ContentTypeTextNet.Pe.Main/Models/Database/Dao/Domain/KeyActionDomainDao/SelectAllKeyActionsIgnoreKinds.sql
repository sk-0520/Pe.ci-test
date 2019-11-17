select
	KeyActions.KeyActionId,
	KeyActions.KeyActionKind,
	KeyActions.KeyActionContent,
	KeyActions.KeyActionOption,
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
where
		KeyActions.KeyActionKind not in @IgnoreKinds
order by
	KeyActions.KeyActionId,
	KeyMappings.Sequence
