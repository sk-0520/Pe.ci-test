cd /d %~dp0

if not defined INKSCAPE (
	set INKSCAPE=C:\Program Files\Inkscape\inkscape.exe
)
set BASE_NAME=%1
set INPUT_SVG=%BASE_NAME%.svg
set DPI=96

set SIZE=16
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=18
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=20
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=24
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=28
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=32
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=40
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=48
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=56
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=60
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=64
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=72
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=80
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=84
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=96
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=112
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=128
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=160
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=192
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=224
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=256
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=320
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=384
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=448
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png

set SIZE=512
"%INKSCAPE%" --file=%INPUT_SVG% --export-dpi=%DPI% --export-width=%SIZE% --export-height=%SIZE% --export-png=%BASE_NAME%_%SIZE%.png


