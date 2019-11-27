select
	KeyActions.KeyActionId,
	KeyActions.KeyActionKind,
	KeyActions.KeyActionContent,
	KeyActions.Comment
from
	KeyActions
where
	KeyActions.KeyActionKind = @KeyActionKind
