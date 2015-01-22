cd /d %~dp0\..\
echo off

set BUILD=Build
set ERROR=%BUILD%\@error

set OUTPUT=output\Release
set OUTPUTx86=%OUTPUT%\x86
set OUTPUTx64=%OUTPUT%\x64
set VER_TARGET=%OUTPUTx86%\PeMain.exe
set ZIP=%BUILD%\zip.vbs
set GV=%BUILD%\get-ver.vbs

set DOTNETVER=v4.0.30319

del /Q %ERROR%

rmdir /S /Q "%OUTPUTx86%"
rmdir /S /Q "%OUTPUTx64%"

if "%PROCESSOR_ARCHITECTURE%" NEQ "x86" (
	set MB=%windir%\microsoft.net\framework64\%DOTNETVER%\msbuild
) else (
	set MB=%windir%\microsoft.net\framework\%DOTNETVER%\msbuild
)

echo build x86
"%MB%" Pe\Pe.sln /p:DefineConstants=BUILD /p:Configuration=Release;Platform=x86 /t:Rebuild /m
set ERROR_X86=%ERRORLEVEL%

echo build x64
"%MB%" Pe\Pe.sln /p:DefineConstants=BUILD /p:Configuration=Release;Platform=x64 /t:Rebuild /m
set ERROR_X64=%ERRORLEVEL%

if not %ERROR_X86% == 0 echo "build error x86: %ERROR_X86%" >> "%ERROR%"
if not %ERROR_X64% == 0 echo "build error x64: %ERROR_X64%" >> "%ERROR%"

for /F "usebackq" %%s in (`cscript "%GV%" "%VER_TARGET%"`) do set EXEVER=%%s

echo remove
echo remove SQLite.Interop.dll
rmdir /S /Q "%OUTPUTx86%\x64"
rmdir /S /Q "%OUTPUTx86%\x86"
rmdir /S /Q "%OUTPUTx64%\x64"
rmdir /S /Q "%OUTPUTx64%\x86"
rmdir /S /Q "%OUTPUTx86%\lib\x64"
rmdir /S /Q "%OUTPUTx64%\lib\x86"
echo remove XML
del "%OUTPUTx86%\lib\System.Data.SQLite.xml"
del "%OUTPUTx64%\lib\System.Data.SQLite.xml"
del "%OUTPUTx86%\lib\MouseKeyboardActivityMonitor.xml"
del "%OUTPUTx64%\lib\MouseKeyboardActivityMonitor.xml"


echo compression
cscript "%ZIP%" "%OUTPUTx86%" "%OUTPUT%\Pe_%EXEVER%_x86.zip"
cscript "%ZIP%" "%OUTPUTx64%" "%OUTPUT%\Pe_%EXEVER%_x64.zip"

