@echo off
@setlocal enabledelayedexpansion

pushd ..\..\Source\Pe

echo #region artifact.bat
for /F %%D in ('dir Pe.Plugins.* /A D /B') do (
	set DIR_NAME=%%D

	echo - name: ^<Artifact^> !DIR_NAME!
	echo   uses: actions/upload-artifact@v3
	echo   with:
	echo     name: !DIR_NAME!-${{ matrix.PLATFORM }}
	echo     path: Output\!DIR_NAME!_*.${{ env.DEFAULT_ARCHIVE }}

	echo - name: ^<Artifact^> !DIR_NAME!.json
	echo   uses: actions/upload-artifact@v3
	echo   with:
	echo     name: !DIR_NAME!-${{ matrix.PLATFORM }}.json
	echo     path: Output\!DIR_NAME!.json
)
echo #endregion

popd
