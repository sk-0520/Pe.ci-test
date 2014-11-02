cd /d %~dp0\..\

rem アップデートによる過去の残骸削除

del PeUpdater.exe
del PeUpdater.exe.config
del PeUpdater.update-old
del bin\PeUpdater.update-old

del doc\changelog.xsl

pause
