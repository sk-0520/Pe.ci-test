select
	KeyOptions.KeyActionId,
	KeyOptions.KeyOptionName,
	KeyOptions.KeyOptionValue
from
	KeyOptions
where
	KeyOptions.KeyActionId = @KeyActionId
