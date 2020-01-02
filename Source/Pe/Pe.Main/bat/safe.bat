cd /d %~dp0

echo off

set DATA_DIR=safe-data

echo == セーフモード起動 ==
echo.
echo 各種データは %DATA_DIR% ディレクトリに保存されます。
echo 実行後に不要であれば削除してください。
echo.
echo 実行しています...
..\Pe.exe --log .\%DATA_DIR%\logs --force-log --user-dir .\%DATA_DIR%\user --machine-dir .\%DATA_DIR%\machine --temp-dir .\%DATA_DIR%\temp

echo.
echo キー入力でプロンプトを閉じます。
pause > nul

