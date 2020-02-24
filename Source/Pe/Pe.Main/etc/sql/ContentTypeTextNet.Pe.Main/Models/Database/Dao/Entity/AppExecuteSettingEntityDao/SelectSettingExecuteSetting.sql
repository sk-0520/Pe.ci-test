select
	AppExecuteSetting.UserId,
	AppExecuteSetting.IsEnabledTelemetry
from
	AppExecuteSetting
where
	AppExecuteSetting.Generation = (
		select
			MAX(AppExecuteSetting.Generation)
		from
			AppExecuteSetting
	)
