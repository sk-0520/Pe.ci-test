@echo off

pushd ..\..\Source\Pe

for /F %%D in ('dir Pe.Plugins.* /A D /B') do (  
	set DIR_NAME=%STR% %%D
	
	echo - name: ^<Artifact^> e.Plugins.Reference.ClassicTheme
	echo   uses: actions/upload-artifact@v3
	echo   with:
	echo     name: Pe.Plugins.Reference.ClassicTheme-${{ matrix.PLATFORM }}
	echo     path: Output\Pe.Plugins.Reference.ClassicTheme_*.${{ env.DEFAULT_ARCHIVE }}
)


popd
