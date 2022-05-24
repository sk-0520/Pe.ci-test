
/* 自動生成: text.z.parse.c.tt */
#include "text.h"
#include "debug.h"

static TEXT_PARSED_I32_RESULT create_failed_i32_parse_result()
{
    return (TEXT_PARSED_I32_RESULT) {
        .success = false,
    };
}
#ifdef _WIN64
static TEXT_PARSED_I64_RESULT create_failed_i64_parse_result()
{
    return (TEXT_PARSED_I64_RESULT) {
        .success = false,
    };
}
#endif

static bool check_has_i_sign(const TEXT* text)
{
    assert(text);
    assert(text->length);

    return text->value[0] == '-' || text->value[0] == '+';
}

static bool check_has_u_sign(const TEXT* text)
{
    assert(text);
    assert(text->length);

    return text->value[0] == '+';
}

static TEXT skip_base_header(const TEXT* text, size_t base)
{
    if (2 <= text->length) {
        switch (base) {
            case 2:
                if (text->value[0] == _T('0') && (text->value[1] == _T('b') || text->value[0] == _T('B'))) {
                    return reference_text_width_length(text, 2, 0);
                }
                break;

            case 8:
                if (text->value[0] == _T('0') && (text->value[1] == _T('o') || text->value[0] == _T('O'))) {
                    return reference_text_width_length(text, 2, 0);
                }
                break;

            case 16:
                if (text->value[0] == _T('0') && (text->value[1] == _T('x') || text->value[0] == _T('X'))) {
                    return reference_text_width_length(text, 2, 0);
                }
                break;

            default:
                break;
        }
    }

    return *text;
}

TEXT_PARSED_I32_RESULT parse_i32_from_text(const TEXT* input, size_t base)
{
    assert(2 <= base && base <= 36);

    if (!is_enabled_text(input)) {
        return create_failed_i32_parse_result();
    }

    TEXT trimmed_input = trim_whitespace_text_stack(input);
    if (!trimmed_input.length) {
        return create_failed_i32_parse_result();
    }

    bool has_sign = check_has_i_sign(&trimmed_input);

    TEXT sign_skip_text = has_sign ? reference_text_width_length(&trimmed_input, 1, 0) : trimmed_input;
    TEXT parse_target_text = skip_base_header(&sign_skip_text, base);

    int32_t total = 0;

    for (size_t i = 0; i < parse_target_text.length; i++) {
        TCHAR c = parse_target_text.value[i];
        if (i) {
            total *= (int32_t)base;
        }
        if (base <= 10) {
            int32_t n;
            if (_T('0') <= c && c <= (_T('0') + base - 1)) {
                n = c - '0';
            } else {
                return create_failed_i32_parse_result();
            }
            total += n;
        } else {
            int32_t n;
            if ('a' <= c && c <= ((_T('a') + base - 1 - 10))) {
                n = c - 'a' + 10;
            } else if ('A' <= c && c <= ((_T('A') + base - 1 - 10))) {
                n = c - 'A' + 10;
            } else if (_T('0') <= c && c <= (_T('0') + base - 1)) {
                n = c - '0';
            } else {
                return create_failed_i32_parse_result();
            }
            total += n;
        }
    }

    if (has_sign && trimmed_input.value[0] == _T('-')) {
        total *= -1;
    }

    return (TEXT_PARSED_I32_RESULT) {
        .success = true,
        .value = total,
    };
}

#ifdef _WIN64
TEXT_PARSED_I64_RESULT parse_i64_from_text(const TEXT* input, size_t base)
{
    assert(2 <= base && base <= 36);

    if (!is_enabled_text(input)) {
        return create_failed_i64_parse_result();
    }

    TEXT trimmed_input = trim_whitespace_text_stack(input);
    if (!trimmed_input.length) {
        return create_failed_i64_parse_result();
    }

    bool has_sign = check_has_i_sign(&trimmed_input);

    TEXT sign_skip_text = has_sign ? reference_text_width_length(&trimmed_input, 1, 0) : trimmed_input;
    TEXT parse_target_text = skip_base_header(&sign_skip_text, base);

    int64_t total = 0;

    for (size_t i = 0; i < parse_target_text.length; i++) {
        TCHAR c = parse_target_text.value[i];
        if (i) {
            total *= (int64_t)base;
        }
        if (base <= 10) {
            int32_t n;
            if (_T('0') <= c && c <= (_T('0') + base - 1)) {
                n = c - '0';
            } else {
                return create_failed_i64_parse_result();
            }
            total += n;
        } else {
            int32_t n;
            if ('a' <= c && c <= ((_T('a') + base - 1 - 10))) {
                n = c - 'a' + 10;
            } else if ('A' <= c && c <= ((_T('A') + base - 1 - 10))) {
                n = c - 'A' + 10;
            } else if (_T('0') <= c && c <= (_T('0') + base - 1)) {
                n = c - '0';
            } else {
                return create_failed_i64_parse_result();
            }
            total += n;
        }
    }

    if (has_sign && trimmed_input.value[0] == _T('-')) {
        total *= -1;
    }

    return (TEXT_PARSED_I64_RESULT) {
        .success = true,
        .value = total,
    };
}
#endif

