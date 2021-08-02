#include "console.h"
#include "logging.h"

CONSOLE_RESOURCE begin_console()
{
    bool attached = false;
    //HANDLE sdt_output_handle = GetStdHandle(STD_OUTPUT_HANDLE);
    //logger_format_debug(_T("sdt_output_handle: %p"), sdt_output_handle);
    if (AttachConsole((DWORD)-1)) {
        attached = true;
    }
    if (!attached) {
        AllocConsole();
    }

    CONSOLE_RESOURCE console_resource = {
        .input = GetStdHandle(STD_INPUT_HANDLE),
        .output = GetStdHandle(STD_OUTPUT_HANDLE),
        .error = GetStdHandle(STD_ERROR_HANDLE),
        .library = {
            .attached = attached,
        }
    };

    DWORD modemode;
    GetConsoleMode(console_resource.output, &modemode);


    return console_resource;
}

void end_console(CONSOLE_RESOURCE* console_resource)
{
    if (!console_resource->library.attached) {
        FreeConsole();
    }
}

size_t output_console_text(const CONSOLE_RESOURCE* console_resource, const TEXT* text, bool newline)
{
    DWORD write_length;
    WriteConsole(console_resource->output, text->value, (DWORD)text->length, &write_length, NULL);

    if (newline) {
        DWORD newline_length;
        TEXT newlinet = wrap_text(NEWLINET);
        WriteConsole(console_resource->output, newlinet.value, (DWORD)newlinet.length, &newline_length, NULL);
        write_length += newline_length;
    }

    return (size_t)write_length;
}
