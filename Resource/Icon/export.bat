cd /d %~dp0

call convert-png.bat App
call convert-png.bat App-beta
call convert-png.bat App-debug

call pack-icon App
call pack-icon App-beta
call pack-icon App-debug
