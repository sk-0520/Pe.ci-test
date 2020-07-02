cd /d %~dp0

set BUILD_TYPE=BETA
set PLATFORM=x64

for /d %%d in ( * ) do (
	dotnet publish %%d\%%~nxd.csproj /m --verbosity normal --configuration Release /p:Platform=x64 --runtime win-x64 --output X:\00_others\10_temp\Pe\bat\beta-data\machine\plugin\modules\%%~nxd
)


pause
