cd /d %~dp0\..\

rem アップデートによる過去の残骸削除

del PeUpdater.exe
del PeUpdater.exe.config
del PeUpdater.update-old
del bin\PeUpdater.update-old

del bin\PeUpdater.exe
del Interop.IWshRuntimeLibrary.dll
del Library.dll
del PInvoke.dll
del PeSkin.dll
del PeUtility.dll
del PeUtility.dll.config

del doc\changelog.xsl

pause
