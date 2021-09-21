select
	count(*) <> 0
from
	Plugins
where
	Plugins.State = @State
