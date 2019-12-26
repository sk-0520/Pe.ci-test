select
	COUNT(*) = 1
from
	sqlite_master
where
	sqlite_master.type = 'table'
	and
	sqlite_master.name = 'AppExecuteSetting'
;
