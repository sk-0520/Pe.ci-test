cd /d %~dp0

powershell -File icon.ps1 -BatchMode -FirstInput 1
powershell -File icon.ps1 -BatchMode -FirstInput 2
powershell -File icon.ps1 -BatchMode -FirstInput 3

pause