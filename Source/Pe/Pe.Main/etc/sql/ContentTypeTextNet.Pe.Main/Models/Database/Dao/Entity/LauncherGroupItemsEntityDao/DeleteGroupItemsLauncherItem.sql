delete
from
	LauncherGroupItems
where
	LauncherGroupItems.rowid = (
		select
			Indexed_LauncherGroupItems.rowid
		from
			(
				-- グループID + ランチャーアイテムIDの並び順から行番号(0基点)を取得する
				select
					ROW_NUMBER() OVER(
						order by
							LauncherGroupItems.Sequence
					) - 1 as ItemIndex, -- 1基点なのでずらす
					LauncherGroupItems.rowid,
					LauncherGroupItems.*
				from
					LauncherGroupItems
				where
					LauncherGroupItems.LauncherGroupId = @LauncherGroupId
					and
					LauncherGroupItems.LauncherItemId = @LauncherItemId
			) as Indexed_LauncherGroupItems
		where
			Indexed_LauncherGroupItems.ItemIndex = @ItemIndex
	)
