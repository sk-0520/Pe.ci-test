cd /d %~dp0\..\

for /F "skip=2 tokens=3* delims= " %%a in ('reg query "HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders" /v Desktop') do set DesktopFolder=%%a
for /F "usebackq" %%s in (`echo %DesktopFolder%`) do set DESKTOP=%%s

set BETA=Pe-beta
set SETTING_DIR=%DESKTOP%\%BETA%

start PeMain.exe /setting-root=%SETTING_DIR% /mutex=Pe_beta

