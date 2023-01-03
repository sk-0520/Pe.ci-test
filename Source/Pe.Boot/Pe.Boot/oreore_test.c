#include "oreore_test.h"
#include "../Pe.Library/debug.h"
#include "../Pe.Library/fsio.z.textfile.h"

void oreore_test(HINSTANCE hInstance, const COMMAND_LINE_OPTION* command_line_option)
{
    TEXT input = wrap_text(_T("D:\\sk\\Documents\\programming\\Pe\\Pe\\Source\\Pe\\Pe.Main\\App.xaml.cs.utf16"));
    FILE_READER file_reader = new_file_reader(&input, FILE_ENCODING_UTF16LE, DEFAULT_MEMORY_ARENA);
    TEXT text = read_content_file_reader(&file_reader);
    output_debug(text.value);
    release_file_reader(&file_reader);
}

