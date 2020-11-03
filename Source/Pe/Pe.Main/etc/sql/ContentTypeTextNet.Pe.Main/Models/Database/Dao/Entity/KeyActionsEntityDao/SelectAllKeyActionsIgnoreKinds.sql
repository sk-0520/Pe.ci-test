
select
	KeyActions.KeyActionId,
	KeyActions.KeyActionKind,
	KeyActions.KeyActionContent,
	KeyActions.Comment
from
	KeyActions
where
	KeyActions.KeyActionKind not in @IgnoreKinds
order by
	case KeyActions.KeyActionKind
		when 'replace' then
			1
		when 'disable' then
			2
		when 'command' then
			3
		when 'launcher-item' then
			4
		when 'launcher-toolbar' then
			5
		when 'note' then
			6
		else
			100
	end

