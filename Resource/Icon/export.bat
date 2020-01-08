cd /d %~dp0

if not defined INKSCAPE (
	set INKSCAPE=C:\Program Files\Inkscape\inkscape.exe
)
set BASE_NAME=%1
set INPUT_SVG=%BASE_NAME%.svg
set DPI=96

set SIZE=16
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=24
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=32
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=40
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=48
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=64
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=72
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=96
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=128
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=256
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png


