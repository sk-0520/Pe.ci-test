@echo off
@setlocal enabledelayedexpansion

set INDENT=
set INDENT=      %INDENT%

pushd ..\..\Source\Pe

set PLATFORMS[0] = PLATFORMS[%I]
set PLATFORMS[1] = x64

echo %INDENT%#region artifact-single.bat
for /l %%I in (0,1) do (
	for /F %%D in ('dir Pe.Plugins.Reference.* /A D /B') do (
		set DIR_NAME=%%D

		echo %INDENT%- name: ^<Artifact^> Plugin - archive - !DIR_NAME!
		echo %INDENT%  uses: actions/upload-artifact@v3
		echo %INDENT%  with:
		echo %INDENT%    name: !DIR_NAME!-PLATFORMS[%%I].${{ env.DEFAULT_ARCHIVE }}
		echo %INDENT%    path: Output\!DIR_NAME!_*.${{ env.DEFAULT_ARCHIVE }}

		echo.

		echo %INDENT%- name: ^<Artifact^> Plugin - update info - !DIR_NAME!
		echo %INDENT%  uses: actions/upload-artifact@v3
		echo %INDENT%  with:
		echo %INDENT%    name: !DIR_NAME!-PLATFORMS[%%I].json
		echo %INDENT%    path: Output\update-!DIR_NAME!.json

		echo.

		echo %INDENT%- name: ^<Artifact^> Plugin - html - !DIR_NAME!
		echo %INDENT%  uses: actions/upload-artifact@v3
		echo %INDENT%  with:
		echo %INDENT%    name: !DIR_NAME!-PLATFORMS[%I].html
		echo %INDENT%    path: Output\!DIR_NAME!.html

		echo.
	)
)
echo %INDENT%#endregion

popd
