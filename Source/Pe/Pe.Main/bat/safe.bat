cd /d %~dp0

echo off

echo "セーフモード起動"
..\Pe.exe --log .\data\logs --force-log --user-dir .\data\user --machine-dir .\data\machine --temp-dir .\data\temp

pause
