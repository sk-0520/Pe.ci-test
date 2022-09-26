@echo off
@setlocal enabledelayedexpansion

pushd ..\..\Source\Pe

echo #region artifact.bat
for /F %%D in ('dir Pe.Plugins.* /A D /B') do (
	set DIR_NAME=%%D

	echo - name: ^<Artifact^> archive: !DIR_NAME!
	echo   uses: actions/upload-artifact@v3
	echo   with:
	echo     name: !DIR_NAME!-${{ matrix.PLATFORM }}.${{ env.DEFAULT_ARCHIVE }}
	echo     path: Output\!DIR_NAME!_*.${{ env.DEFAULT_ARCHIVE }}

	echo.

	echo - name: ^<Artifact^> update: !DIR_NAME!
	echo   uses: actions/upload-artifact@v3
	echo   with:
	echo     name: !DIR_NAME!-${{ matrix.PLATFORM }}.json
	echo     path: Output\update-!DIR_NAME!.json

	echo.
	echo.
)
echo #endregion

popd
