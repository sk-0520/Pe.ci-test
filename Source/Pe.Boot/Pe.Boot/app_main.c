#include <assert.h>

#include "app_main.h"
#include "debug.h"
#include "memory.h"
#include "tstring.h"
#include "path.h"
#include "logging.h"
#include "app_boot.h"

int getWaitTime(const TCHAR* s);

int app_main(HINSTANCE hInstance, const COMMAND_LINE_OPTION* command_line_option)
{
    if (command_line_option->count < 1) {
        // そのまま実行
        boot_normal(hInstance);
        return 0;
    }

    // コマンドライン渡して実行
    size_t tuned_args_count = command_line_option->count;
    TCHAR** tuned_args = allocate_clear_memory(tuned_args_count, sizeof(TCHAR*));
    if (!tuned_args) {
        // これもう立ち上げ不能だと思う
        output_debug(_T("メモリ確保できんかったね！"));
        boot_normal(hInstance);
        return 0;
    }

    // 実行待機用
    int wait_time = 0;
    size_t total_length = 0;
    size_t skip_index1 = SIZE_MAX;
    size_t skip_index2 = SIZE_MAX;

    for (size_t i = 0, j = 0; i < command_line_option->count; i++, j++) {
        const TCHAR* work_arg = command_line_option->library.argv[i];
        output_debug(work_arg);
        TCHAR* tuned_arg = tuneArg(work_arg);
        assert(tuned_arg);
        tuned_args[j] = tuned_arg;
        total_length += get_string_length(tuned_args[j]);
        if (!wait_time) {
            TCHAR waits[][16] = {
                _T("--_boot-wait"), _T("-_boot-wait"), _T("/_boot-wait"),
                _T("--wait"), _T("-wait"), _T("/wait"), //TODO: #737 互換用処理
            };
            for (size_t wait_index = 0; wait_index < sizeof(waits) / sizeof(waits[0]); wait_index++) {
                const TCHAR* wait_arg = waits[wait_index];
                const TCHAR* wait = findString(tuned_arg, wait_arg, false);
                if (wait == tuned_arg) {
                    skip_index1 = j;

                    TCHAR* eq = findCharacter(wait, '=');
                    if (eq && eq + 1) {
                        TCHAR* value = eq + 1;
                        wait_time = getWaitTime(value);
                    } else if (i + 1 < command_line_option->count) {
                        wait_time = getWaitTime(command_line_option->library.argv[i + 1]);

                        skip_index2 = (size_t)(j + 1);
                    }
                    break;
                }
            }
        }
    }

    TCHAR* command_arg = allocate_string(total_length + 1);
    if (command_arg) {
        command_arg[0] = 0;
        for (size_t i = 0; i < tuned_args_count; i++) {
            // 大丈夫、はやいよ！
            if ((skip_index1 == i) || (skip_index2 == i)) {
                continue;
            }
            concat_string(command_arg, tuned_args[i]);
            concat_string(command_arg, _T(" "));
        }
    }

    // 起動前停止
    if (0 < wait_time) {
        TCHAR s[1000];
        format_string(s, _T("起動前停止: %d ms"), wait_time);
        output_debug(s);
        Sleep(wait_time);
        output_debug(_T("待機終了"));
    }

    // commandArg の確保に失敗してても引数無し扱いで起動となる
    output_debug(command_arg);
    boot_with_option(hInstance, command_arg);

    // もはや死ぬだけなので後処理不要

    return 0;
}


int getWaitTime(const TCHAR* s)
{
    if (!s) {
        return 0;
    }

    int result;
    if (tryParseInteger(&result, s)) {
        return result;
    }

    return 0;
}
