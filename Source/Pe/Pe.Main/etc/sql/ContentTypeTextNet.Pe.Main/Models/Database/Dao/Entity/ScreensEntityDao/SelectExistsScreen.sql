
select
	COUNT(1) = 1
from
	Screens
where
	Screens.ScreenName = @ScreenName
