select
	LauncherToolbars.LauncherToolbarId,
	LauncherToolbars.ScreenName,
	Screens.ScreenX,
	Screens.ScreenY,
	Screens.ScreenWidth,
	Screens.ScreenHeight
from
	LauncherToolbars
	inner join
		Screens
		on
		(
			LauncherToolbars.ScreenName = Screens.ScreenName
		)
