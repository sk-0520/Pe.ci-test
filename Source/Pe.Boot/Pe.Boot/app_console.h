#pragma once
#include "command_line.h"
#include "app_common.h"


typedef enum tag_CONSOLE_KIND
{
    CONSOLE_KIND_UNKNOWN,
    CONSOLE_KIND_PROMPT,
} CONSOLE_KIND;

typedef struct tag_CONSOLE_RESOURCE
{
    HANDLE input;
    HANDLE output;
    HANDLE error;
} CONSOLE_RESOURCE;

CONSOLE_RESOURCE begin_console();
void end_console(CONSOLE_RESOURCE* console_resource);

CONSOLE_KIND get_console_kind(const COMMAND_LINE_OPTION* command_line_option);

size_t output_console_text(const CONSOLE_RESOURCE* console_resource, const TEXT* text, bool newline);

EXIT_CODE console_execute(HINSTANCE hInstance, const COMMAND_LINE_OPTION* command_line_option);
