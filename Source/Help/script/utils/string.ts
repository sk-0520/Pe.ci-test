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
