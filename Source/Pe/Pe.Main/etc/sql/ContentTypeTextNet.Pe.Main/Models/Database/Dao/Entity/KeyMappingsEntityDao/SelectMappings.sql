select
	KeyMappings.Key,
	KeyMappings.Shift,
	KeyMappings.Control,
	KeyMappings.Alt,
	KeyMappings.Super
from
	KeyMappings
where
	KeyMappings.KeyActionId = @KeyActionId
order by
	KeyMappings.Sequence
