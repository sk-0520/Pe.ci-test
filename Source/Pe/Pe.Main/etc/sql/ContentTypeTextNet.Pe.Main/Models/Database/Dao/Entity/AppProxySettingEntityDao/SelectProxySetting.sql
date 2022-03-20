select
	AppProxySetting.ProxyIsEnabled,
	AppProxySetting.ProxyUrl,
	AppProxySetting.CredentialIsEnabled,
	AppProxySetting.CredentialUser,
	AppProxySetting.CredentialPassword
from
	AppProxySetting
where
	AppProxySetting.Generation = (
		select
			MAX(AppProxySetting.Generation)
		from
			AppProxySetting
	)
