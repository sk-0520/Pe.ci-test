cd /d %~dp0

set DOTNETVER=v4.0.30319
if "%PROCESSOR_ARCHITECTURE%" NEQ "x86" (
	set MB=%windir%\microsoft.net\framework64\%DOTNETVER%\msbuild
) else (
	set MB=%windir%\microsoft.net\framework\%DOTNETVER%\msbuild
)

echo "x86"
"%MB%" Pe\Pe.sln /p:Configuration=Release;Platform=x86 /t:Rebuild /m
echo "x64"
"%MB%" Pe\Pe.sln /p:Configuration=Release;Platform=x64 /t:Rebuild /m

