select
	AppExecuteSetting.FirstVersion,
	AppExecuteSetting.FirstTimestamp
from
	AppExecuteSetting
where
	AppExecuteSetting.Generation = (
		select
			MAX(AppExecuteSetting.Generation)
		from
			AppExecuteSetting
	)
