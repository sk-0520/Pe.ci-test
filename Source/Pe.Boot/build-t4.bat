cd /d %~dp0
rem 1. *.tt を変更した際にこのバッチを起動してソースコードを生成する
rem 2. t4 の挙動に依存しているが出力拡張子を空で設定し、t4自体は .c.tt, .h.tt として配置する
rem 3. 定義変数を変更する場合は %CUSTOM_SETTING_FILE% を同一ディレクトリに配置して変数を上書きすること

set CUSTOM_SETTING_FILE=@build-t4-setting.bat

set DevEnvDir=C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\

if exist "%CUSTOM_SETTING_FILE%" call "%CUSTOM_SETTING_FILE%"

for /d %%d in (*) do (
	for %%f in (%%d\*.tt) do (
		"%DevEnvDir%TextTransform.exe"  "%%f"
	)
)
