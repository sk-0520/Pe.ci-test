cd /d %~dp0

if not defined CONVERT (
	set CONVERT=C:\Applications\tool\ImageMagick\convert.exe
)
set BASE_NAME=%1
set OUTPUT_NAME=%BASE_NAME%.ico

setlocal enabledelayedexpansion

set INPUT_FILES=
for %%i in (%BASE_NAME%_*.png) do (
	echo ファイル名：%%i
	set INPUT_FILES=!INPUT_FILES! %%i
)

%CONVERT% %INPUT_FILES% %BASE_NAME%.ico
