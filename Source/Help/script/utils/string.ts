export const NewLine = "\n";

/**
 * トリムの未指定時の対象文字。
 */
const DefaultTrimCharacters: ReadonlySet<string> = new Set([
	"\u{0009}",
	"\u{000a}",
	"\u{000b}",
	"\u{000c}",
	"\u{000d}",
	"\u{0085}",
	"\u{00a0}",
	"\u{0020}",
	"\u{2000}",
	"\u{2001}",
	"\u{2002}",
	"\u{2003}",
	"\u{2004}",
	"\u{2005}",
	"\u{2006}",
	"\u{2007}",
	"\u{2008}",
	"\u{2009}",
	"\u{200A}",
	"\u{202F}",
	"\u{205F}",
	"\u{3000}",
]);

/**
 * 先頭文字列のトリム処理。
 * @param s
 * @param characters
 * @returns
 */
export function trimStart(s: string, characters?: ReadonlySet<string>): string {
	const workCharacters = characters ?? DefaultTrimCharacters;

	if (!workCharacters.size) {
		return s;
	}

	for (let i = 0; i < s.length; i++) {
		if (workCharacters.has(s[i])) {
			continue;
		}

		return s.substring(i);
	}

	return "";
}

/**
 * 終端文字列のトリム処理。
 * @param s
 * @param characters
 * @returns
 */
export function trimEnd(s: string, characters?: ReadonlySet<string>): string {
	const workCharacters = characters ?? DefaultTrimCharacters;

	if (!workCharacters.size) {
		return s;
	}

	for (let i = 0; i < s.length; i++) {
		if (workCharacters.has(s[s.length - i - 1])) {
			continue;
		}

		return s.substring(0, s.length - i);
	}

	return "";
}

/**
 * 前後のトリム処理。
 * @param s
 * @param characters
 * @returns
 */
export function trim(s: string, characters?: ReadonlySet<string>): string {
	const workCharacters = characters ?? DefaultTrimCharacters;

	if (!workCharacters.size) {
		return s;
	}

	return trimEnd(trimStart(s, workCharacters), workCharacters);
}

const NewLineRegex = /\r\n|\n|\r/;

export function splitLines(s: string): string[] {
	return s.split(NewLineRegex);
}

const CharactersRegex = /[\uD800-\uDBFF][\uDC00-\uDFFF]|[^\uD800-\uDFFF]/g;

export function countSingleChar(s: string): number {
	if (!s || !s.length) {
		return 0;
	}
	const chars = s.match(CharactersRegex) || [];
	let length = 0;
	for (const c of chars) {
		if (c.length === 1) {
			// biome-ignore lint/suspicious/noControlCharactersInRegex: <explanation>
			if (!c.match(/[^\x01-\x7E]/) || !c.match(/[^\uFF65-\uFF9F]/)) {
				length += 1;
			} else {
				length += 2;
			}
		} else {
			// もういいでしょ
			length += 2;
		}
	}
	return length;
}
