select
	COUNT(*) = 1
from
	sqlite_master -- noqa: CP02
where
	sqlite_master.type = 'table' -- noqa: CP02
	and
	sqlite_master.name = 'AppExecuteSetting' -- noqa: CP02
