delete from
	LauncherGroupItems
where
	LauncherGroupItems.RowId = (
		select
			Indexed_LauncherGroupItems.RowId
		from
			(
				-- グループID + ランチャーアイテムIDの並び順から行番号(0基点)を取得する
				select
					ROW_NUMBER() over(
						order by
							LauncherGroupItems.Sequence
					) - 1 as ItemIndex, -- 1基点なのでずらす
					LauncherGroupItems.RowId,
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
