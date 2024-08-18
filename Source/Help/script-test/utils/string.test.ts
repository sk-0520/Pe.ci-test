import { trim, trimEnd, trimStart } from "../../script/utils/string";

describe("trimStart", () => {
	test.each([
		["", "", [""]],
		["", " ", [" "]],
		["a ", " a ", [" "]],
		["a", "   a", [" "]],
		["　  a", "  　  a", [" "]],
		["a", "  　  a", [" ", "　"]],
	])("each 期待値: [%s], 入力: [%s], 文字: [%s]", (expected: string, input: string, characters: string[]) => {
		expect(trimStart(input, new Set(characters))).toBe(expected);
	});
});

describe("trimEnd", () => {
	test.each([
		["", "", [""]],
		["", " ", [" "]],
		[" a", " a ", [" "]],
		[" a 　", " a 　 ", [" "]],
		[" a", " a  　  ", [" ", "　"]],
	])("each 期待値: [%s], 入力: [%s], 文字: [%s]", (expected: string, input: string, characters: string[]) => {
		expect(trimEnd(input, new Set(characters))).toBe(expected);
	});
});

describe("trim", () => {
	test.each([
		["", "", [""]],
		["a", " a ", [" "]],
		["　 a 　", " 　 a 　 ", [" "]],
		["a", " 　 a 　 ", [" ", "　"]],
	])("each 期待値: [%s], 入力: [%s], 文字: [%s]", (expected: string, input: string, characters: string[]) => {
		expect(trim(input, new Set(characters))).toBe(expected);
	});

	test.each([
		["", ""],
		["", " "],
		["a", " a "],
		["a", " 　 a 　 "],
		["a", " 　 a 　 "],
	])("each:default 期待値: [%s], 入力: [%s]", (expected: string, input: string) => {
		expect(trim(input)).toBe(expected);
	});
});
