@echo off
@setlocal enabledelayedexpansion

set INDENT=
set INDENT=      %INDENT%

pushd ..\..\Source\Pe

set PLATFORMS_0 = x86
set PLATFORMS_1 = x64

echo %INDENT%#region artifact-single.bat
for /l %%I in (0,1) do (
	set INDEX=%%I

	echo "INDEX: !INDEX!"

	for /F %%D in ('dir Pe.Plugins.Reference.* /A D /B') do (
		set DIR_NAME=%%D
		set PLATFORM=!PLATFORMS_%INDEX%!

		echo "PLATFORM: !PLATFORM!"

		@REM echo %INDENT%- name: ^<Artifact^> Plugin - archive - !DIR_NAME!
		@REM echo %INDENT%  uses: actions/upload-artifact@v3
		@REM echo %INDENT%  with:
		@REM echo %INDENT%    name: !DIR_NAME!-!PLATFORM!.${{ env.DEFAULT_ARCHIVE }}
		@REM echo %INDENT%    path: Output\!DIR_NAME!_*.${{ env.DEFAULT_ARCHIVE }}

		@REM echo.

		@REM echo %INDENT%- name: ^<Artifact^> Plugin - update info - !DIR_NAME!
		@REM echo %INDENT%  uses: actions/upload-artifact@v3
		@REM echo %INDENT%  with:
		@REM echo %INDENT%    name: !DIR_NAME!-!PLATFORM!.json
		@REM echo %INDENT%    path: Output\update-!DIR_NAME!.json

		@REM echo.

		@REM echo %INDENT%- name: ^<Artifact^> Plugin - html - !DIR_NAME!
		@REM echo %INDENT%  uses: actions/upload-artifact@v3
		@REM echo %INDENT%  with:
		@REM echo %INDENT%    name: !DIR_NAME!-!PLATFORM!.html
		@REM echo %INDENT%    path: Output\!DIR_NAME!.html

		@REM echo.
	)
)
echo %INDENT%#endregion

popd
