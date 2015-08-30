select
    coalesce(max({id_column_name}), 0) MAX_ID,
    coalesce(min({id_column_name}), 0) MIN_ID
from
    {table_name}
