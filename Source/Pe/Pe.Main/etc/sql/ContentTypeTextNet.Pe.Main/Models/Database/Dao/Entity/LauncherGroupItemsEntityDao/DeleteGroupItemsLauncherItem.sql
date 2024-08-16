delete from
	LauncherGroupItems
where
	LauncherGroupItems.RowId = (
		select
			AliasIndexedLauncherGroupItems.RowId
		from
			(
				-- グループID + ランチャーアイテムIDの並び順から行番号(0基点)を取得する
				select -- noqa: ST06
					ROW_NUMBER() over (
						order by
							LauncherGroupItems.Sequence asc
					) - 1 as ItemIndex, -- 1基点なのでずらす
					LauncherGroupItems.RowId,
					LauncherGroupItems.*
				from
					LauncherGroupItems
				where
					LauncherGroupItems.LauncherGroupId = @LauncherGroupId
					and
					LauncherGroupItems.LauncherItemId = @LauncherItemId
			) as AliasIndexedLauncherGroupItems
		where
			AliasIndexedLauncherGroupItems.ItemIndex = @ItemIndex
	)
