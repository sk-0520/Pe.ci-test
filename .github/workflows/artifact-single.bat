@echo off
@setlocal enabledelayedexpansion

set INDENT=
set INDENT=      %INDENT%

pushd ..\..\Source\Pe

echo %INDENT%#region artifact-platform.bat
for /F %%D in ('dir Pe.Plugins.Reference.* /A D /B') do (
	set DIR_NAME=%%D

	echo %INDENT%- name: ^<Artifact^> Plugin - archive - !DIR_NAME!
	echo %INDENT%  uses: actions/upload-artifact@v3
	echo %INDENT%  with:
	echo %INDENT%    name: !DIR_NAME!-x86.${{ env.DEFAULT_ARCHIVE }}
	echo %INDENT%    path: Output\!DIR_NAME!_*.${{ env.DEFAULT_ARCHIVE }}

	echo %INDENT%- name: ^<Artifact^> Plugin - archive - !DIR_NAME!
	echo %INDENT%  uses: actions/upload-artifact@v3
	echo %INDENT%  with:
	echo %INDENT%    name: !DIR_NAME!-x64.${{ env.DEFAULT_ARCHIVE }}
	echo %INDENT%    path: Output\!DIR_NAME!_*.${{ env.DEFAULT_ARCHIVE }}
	
	echo.

	echo %INDENT%- name: ^<Artifact^> Plugin - update info - !DIR_NAME!
	echo %INDENT%  uses: actions/upload-artifact@v3
	echo %INDENT%  with:
	echo %INDENT%    name: !DIR_NAME!-x86.json
	echo %INDENT%    path: Output\update-!DIR_NAME!.json

	echo %INDENT%- name: ^<Artifact^> Plugin - update info - !DIR_NAME!
	echo %INDENT%  uses: actions/upload-artifact@v3
	echo %INDENT%  with:
	echo %INDENT%    name: !DIR_NAME!-x64.json
	echo %INDENT%    path: Output\update-!DIR_NAME!.json

	echo.

	echo %INDENT%- name: ^<Artifact^> Plugin - html - !DIR_NAME!
	echo %INDENT%  uses: actions/upload-artifact@v3
	echo %INDENT%  with:
	echo %INDENT%    name: !DIR_NAME!-x86.html
	echo %INDENT%    path: Output\!DIR_NAME!.html

	echo %INDENT%- name: ^<Artifact^> Plugin - html - !DIR_NAME!
	echo %INDENT%  uses: actions/upload-artifact@v3
	echo %INDENT%  with:
	echo %INDENT%    name: !DIR_NAME!-x64.html
	echo %INDENT%    path: Output\!DIR_NAME!.html

	echo.
)
echo %INDENT%#endregion

popd
