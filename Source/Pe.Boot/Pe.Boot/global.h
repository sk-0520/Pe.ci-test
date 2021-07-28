#include <stdint.h>

#ifdef GLOBAL
#   define GLOBAL extern
#else
#   define GLOBAL
#endif

GLOBAL const uint8_t library__unicode_utf8_bom[] = { 0xef, 0xbb, 0xbf };
GLOBAL const uint8_t library__unicode_utf16le_bom[] = { 0xff, 0xef };
