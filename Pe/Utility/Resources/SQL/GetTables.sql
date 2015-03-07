select
	NAME
from
	SQLITE_MASTER
where
	type = 'table'
order by
	NAME
