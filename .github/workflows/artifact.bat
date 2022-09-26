@echo off

pushd ..\..\Source\Pe

for /F %%D in ('dir Pe.Plugins.* /A D /B') do (  
	set DIR_NAME=%STR% %%D
	
	echo - name: ^<Artifact^> %DIR_NAME%
	echo   uses: actions/upload-artifact@v3
	echo   with:
	echo     name: %DIR_NAME%-${{ matrix.PLATFORM }}
	echo     path: Output\%DIR_NAME%_*.${{ env.DEFAULT_ARCHIVE }}
)


popd
