cd /d %~dp0
if "%PROCESSOR_ARCHITECTURE%" NEQ "x86" (
	set SHELL="C:\Program Files (x86)\Git\bin\sh.exe"
) else (
	set SHELL="C:\Program Files\Git\bin\sh.exe"
)
%SHELL% --login -i

