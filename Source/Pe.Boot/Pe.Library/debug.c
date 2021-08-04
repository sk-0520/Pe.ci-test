#include "common.h"
#include "debug.h"
#include "logging.h"

void output_debug(const TCHAR* s)
{
#ifndef NDEBUG
    logger_put_debug(s);
#endif
}
