#include "../Pe.Library/path.h"
#include "../Pe.Library/platform.h"
#include "../Pe.Library/logging.h"
#include "execute.h"

void add_visual_cpp_runtime_redist_env_path(const TEXT* root_directory_path)
{
    TEXT dirs[] = {
        wrap_text(_T("bin")),
        wrap_text(_T("lib")),
        wrap_text(_T("Redist.MSVC.CRT")),
#ifdef _WIN64
        wrap_text(_T("x64")),
#else
        wrap_text(_T("x86")),
#endif
    };

    TEXT crt_path = join_path(root_directory_path, dirs, SIZEOF_ARRAY(dirs), DEFAULT_MEMORY_ARENA);
    logger_put_debug(crt_path.value);

    TEXT env_path_key = wrap_text(_T("PATH"));
    TEXT path_src_value = get_environment_variable(&env_path_key, DEFAULT_MEMORY_ARENA);

    TEXT values[] = {
        path_src_value,
        crt_path
    };
    TEXT env_sep = wrap_text(_T(";"));
    TEXT path_new_value = join_text(&env_sep, values, SIZEOF_ARRAY(values), IGNORE_EMPTY_ONLY, DEFAULT_MEMORY_ARENA);
    set_environment_variable(&env_path_key, &path_new_value);

    release_text(&path_new_value);
    release_text(&path_src_value);
    release_text(&crt_path);
}
