cd /d %~dp0\..\

for /F "skip=2 tokens=3* delims= " %%a in ('reg query "HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders" /v Desktop') do set DesktopFolder=%%a
for /F "usebackq" %%s in (`echo %DesktopFolder%`) do set DESKTOP=%%s

set BETA=Pe-beta
set BETA_ROOT_DIR=%DESKTOP%\%BETA%
set BETA_DIR=%BETA_ROOT_DIR%\Pe

set RELEASE_DIR=%APPDATA%\Pe

if exist "%BETA_ROOT_DIR%" (
	rem ベータ用ディレクトリがあるのでコピらない
	echo "existis: beta setting"
) else (
	rem 既存データをパクってくる
	mkdir "%BETA_DIR%"  2>NUL
	robocopy /MIR "%RELEASE_DIR%\setting" "%BETA_DIR%\setting"
)

start PeMain.exe /setting-root=%BETA_ROOT_DIR% /log=%BETA_ROOT_DIR%\logs /mutex=Pe_beta

