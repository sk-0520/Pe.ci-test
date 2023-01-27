cd /d %~dp0

for /R %%F in (dev-launchSettings.json) do (
	if exist "%%F" (
		copy /Y "%%F" "%%~dF%%~pFlaunchSettings.json"
	)
)
