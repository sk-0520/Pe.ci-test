select
    count(*) NUM
from
    SQLITE_MASTER
where
    NAME = :table_name
    and
    TYPE = 'table'
