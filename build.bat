cd /d %~dp0
echo off

set OUTPUT=output\Release
set PEPATH=%OUTPUT%\x86\PeMain.exe
set ZIP=%OUTPUT%\zip.vbs
set GV=%OUTPUT%\get-ver.vbs

set DOTNETVER=v4.0.30319

if "%PROCESSOR_ARCHITECTURE%" NEQ "x86" (
	set MB=%windir%\microsoft.net\framework64\%DOTNETVER%\msbuild
) else (
	set MB=%windir%\microsoft.net\framework\%DOTNETVER%\msbuild
)

echo build x86
"%MB%" Pe\Pe.sln /p:Configuration=Release;Platform=x86 /t:Rebuild /m
echo build x64
"%MB%" Pe\Pe.sln /p:Configuration=Release;Platform=x64 /t:Rebuild /m

echo create temp script

echo. 'GET VERSION ------------ >  %GV%
echo. Dim exePath, exeVer >> %GV%
echo. exePath = Wscript.Arguments(0) >> %GV%
echo. With CreateObject("Scripting.FileSystemObject") >> %GV%
echo      exePath = .GetAbsolutePathName(exePath) >> %GV%
echo.     exeVer  = .GetFileVersion(exePath) >> %GV%
echo. End With >> %GV%
echo. exeVer = Left(exeVer, InStrRev(exeVer, ".") - 1) >> %GV%
echo. exeVer = Replace(exeVer, ".", "-") >> %GV%
echo. WScript.Echo exeVer >> %GV%

echo. 'MAKE ZIP ARCHIVE ------------ >  %ZIP%
echo. Dim inputDir, outputFile >> %ZIP%
echo. inputDir = Wscript.Arguments(0) >> %ZIP%
echo. outputFile = Wscript.Arguments(1) >> %ZIP%
echo.  >> %ZIP%
echo. With CreateObject("Scripting.FileSystemObject") >> %ZIP%
echo.     outputFile = .GetAbsolutePathName(outputFile) >> %ZIP%
echo.     inputDir   = .GetAbsolutePathName(inputDir) >> %ZIP%
echo.     With .CreateTextFile(outputFile, True) >> %ZIP%
echo.         .Write Chr(80) ^& Chr(75) ^& Chr(5) ^& Chr(6) ^& String(18, chr(0)) >> %ZIP%
echo.     End With >> %ZIP%
echo. End With >> %ZIP%
echo.  >> %ZIP%
echo. With CreateObject("Shell.Application") >> %ZIP%
echo.     .NameSpace(outputFile).CopyHere .NameSpace(inputDir).Items >> %ZIP%
echo.     Do Until .NameSpace(outputFile).Items.Count = .NameSpace(inputDir).Items.Count >> %ZIP%
echo.         WScript.Sleep 1000 >> %ZIP%
echo.     Loop >> %ZIP%
echo. End With >> %ZIP%

for /F "usebackq" %%s in (`cscript %GV% %PEPATH%`) do set EXEVER=%%s

echo compression
cscript %ZIP% %OUTPUT%\x86 %OUTPUT%\Pe_%EXEVER%_x86.zip
cscript %ZIP% %OUTPUT%\x64 %OUTPUT%\Pe_%EXEVER%_x64.zip

