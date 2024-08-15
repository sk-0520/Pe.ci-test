select
	LauncherToolbars.LauncherToolbarId,
	LauncherToolbars.ScreenName,
	COALESCE(Screens.ScreenX, -1) as ScreenX,
	COALESCE(Screens.ScreenY, -1) as ScreenY,
	COALESCE(Screens.ScreenWidth, 0) as ScreenWidth,
	COALESCE(Screens.ScreenHeight, 0) as ScreenHeight
from
	LauncherToolbars
	left join
		Screens
		on
		(
			LauncherToolbars.ScreenName = Screens.ScreenName
		)
where
	LauncherToolbars.LauncherToolbarId = @LauncherToolbarId
