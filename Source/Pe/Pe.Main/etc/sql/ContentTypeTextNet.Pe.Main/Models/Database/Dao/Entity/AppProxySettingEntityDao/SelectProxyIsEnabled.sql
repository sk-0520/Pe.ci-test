select
	AppProxySetting.ProxyIsEnabled
from
	AppProxySetting
where
	AppProxySetting.Generation = (
		select
			MAX(AppProxySetting.Generation)
		from
			AppProxySetting
	)
