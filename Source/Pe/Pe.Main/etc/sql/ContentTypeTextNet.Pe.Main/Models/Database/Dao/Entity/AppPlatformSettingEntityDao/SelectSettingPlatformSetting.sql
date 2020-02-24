select
	AppPlatformSetting.SuppressSystemIdle,
	AppPlatformSetting.SupportExplorer
from
	AppPlatformSetting
where
	AppPlatformSetting.Generation = (
		select
			MAX(AppPlatformSetting.Generation)
		from
			AppPlatformSetting
	)
