cd /d %~dp0

dotnet build Source/Pe/Pe.sln --verbosity normal --configuration Release --runtime win-x64

pause
