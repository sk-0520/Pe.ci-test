cd /d %~dp0

set OUTPUT=output\Release
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

echo compression

dir %OUTPUT%\x86 /s /b /a-d >%OUTPUT%\files.x86
makecab /d "CabinetName1=Pe_x86.cab" /f %OUTPUT%\files.x86

dir %OUTPUT%\x64 /s /b /a-d >%OUTPUT%\files.x64
makecab /d "CabinetName1=Pe_x64.cab" /f %OUTPUT%\files.x64

move /Y disk1\* %OUTPUT%
move /Y setup.* %OUTPUT%
rmdir disk1 /q

