select
	LauncherToolbars.LauncherToolbarId,
	LauncherToolbars.ScreenName,
	IFNULL(Screens.ScreenX, -1) as ScreenX,
	IFNULL(Screens.ScreenY, -1) as ScreenY,
	IFNULL(Screens.ScreenWidth, 0) as ScreenWidth,
	IFNULL(Screens.ScreenHeight, 0) as ScreenHeight
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
