import {
	splitLines,
	trim,
	trimEnd,
	trimStart,
} from "../../script/utils/string";

describe("trimStart", () => {
	test.each([
		["", "", [""]],
		["", " ", [" "]],
		["a ", " a ", [" "]],
		["a", "   a", [" "]],
		["　  a", "  　  a", [" "]],
		["a", "  　  a", [" ", "　"]],
	])(
		"each 期待値: [%s], 入力: [%s], 文字: [%s]",
		(expected: string, input: string, characters: string[]) => {
			expect(trimStart(input, new Set(characters))).toBe(expected);
		},
	);
});

describe("trimEnd", () => {
	test.each([
		["", "", [""]],
		["", " ", [" "]],
		[" a", " a ", [" "]],
		[" a 　", " a 　 ", [" "]],
		[" a", " a  　  ", [" ", "　"]],
	])(
		"each 期待値: [%s], 入力: [%s], 文字: [%s]",
		(expected: string, input: string, characters: string[]) => {
			expect(trimEnd(input, new Set(characters))).toBe(expected);
		},
	);
});

describe("trim", () => {
	test.each([
		["", "", [""]],
		["a", " a ", [" "]],
		["　 a 　", " 　 a 　 ", [" "]],
		["a", " 　 a 　 ", [" ", "　"]],
	])(
		"each 期待値: [%s], 入力: [%s], 文字: [%s]",
		(expected: string, input: string, characters: string[]) => {
			expect(trim(input, new Set(characters))).toBe(expected);
		},
	);

	test.each([
		["", ""],
		["", " "],
		["a", " a "],
		["a", " 　 a 　 "],
		["a", " 　 a 　 "],
	])(
		"each:default 期待値: [%s], 入力: [%s]",
		(expected: string, input: string) => {
			expect(trim(input)).toBe(expected);
		},
	);
});

describe("splitLines", () => {
	test.each([
		[[""], ""],
		[["", ""], "\n"],
		[["", ""], "\r"],
		[["", ""], "\r\n"],
		[["A"], "A"],
		[["A", "B"], "A\nB"],
		[["A", "B"], "A\rB"],
		[["A", "B"], "A\r\nB"],
		[["A", ""], "A\n"],
		[["A", "B", ""], "A\nB\n"],
		[["A", "B", ""], "A\rB\n"],
		[["A", "B", ""], "A\r\nB\n"],
	])("each 期待値: [%s], 入力: [%s]", (expected: string[], input: string) => {
		expect(splitLines(input)).toStrictEqual(expected);
	});
});
