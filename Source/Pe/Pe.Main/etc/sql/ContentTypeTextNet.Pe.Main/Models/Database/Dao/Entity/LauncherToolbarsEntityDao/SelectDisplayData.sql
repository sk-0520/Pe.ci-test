select
	LauncherToolbars.LauncherToolbarId,
	LauncherToolbars.LauncherGroupId,
	LauncherToolbars.PositionKind,
	LauncherToolbars.Direction,
	LauncherToolbars.IconBox,
	LauncherToolbars.FontId,
	LauncherToolbars.DisplayDelayTime,
	LauncherToolbars.AutoHideTime,
	LauncherToolbars.TextWidth,
	LauncherToolbars.IsVisible,
	LauncherToolbars.IsTopmost,
	LauncherToolbars.IsAutoHide,
	LauncherToolbars.IsIconOnly

	/*
	LauncherToolbars.CreatedTimestamp, LauncherToolbars.CreatedAccount, LauncherToolbars.CreatedProgramName, LauncherToolbars.CreatedProgramVersion,
	LauncherToolbars.UpdatedTimestamp, LauncherToolbars.UpdatedAccount, LauncherToolbars.UpdatedProgramName, LauncherToolbars.UpdatedProgramVersion, LauncherToolbars.UpdatedCount
	*/
from
	LauncherToolbars
where
	LauncherToolbars.LauncherToolbarId = @LauncherToolbarId
