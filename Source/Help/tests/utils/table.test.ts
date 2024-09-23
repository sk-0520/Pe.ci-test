import {
	convertColumns,
	convertIndexes,
	splitRawEntities,
	splitRawSection,
} from "../../utils/table";

describe("splitRawEntities", () => {
	test("empty", () => {
		const actual = splitRawEntities("");
		expect(actual).toHaveLength(0);
	});

	test("1 line", () => {
		const actual = splitRawEntities("A\n___\nB");
		expect(actual).toHaveLength(2);
		expect(actual[0]).toBe("A");
		expect(actual[1]).toBe("B");
	});

	test("2 line", () => {
		const actual = splitRawEntities("A\n___\nB\n___\nC");
		expect(actual).toHaveLength(3);
		expect(actual[0]).toBe("A");
		expect(actual[1]).toBe("B");
		expect(actual[2]).toBe("C");
	});
});

describe("splitRawSection", () => {
	test("normal", () => {
		const actual = splitRawSection(`
# TABLE

## layout

LAYOUT

## index

*NONE*

`);
		expect(actual.table).toBe("TABLE");
		expect(actual.layout).toStrictEqual(["LAYOUT"]);
		expect(actual.index).toStrictEqual([]);
	});

	test("shrink", () => {
		const actual = splitRawSection(`
# TABLE
## layout
LAYOUT
## index
*NONE*
`);
		expect(actual.table).toBe("TABLE");
		expect(actual.layout).toStrictEqual(["LAYOUT"]);
		expect(actual.index).toStrictEqual([]);
	});

	test("index", () => {
		const actual = splitRawSection(`
# TABLE

## layout

LAYOUT

## index

A
B
C

`);
		expect(actual.table).toBe("TABLE");
		expect(actual.layout).toStrictEqual(["LAYOUT"]);
		expect(actual.index).toStrictEqual(["A", "B", "C"]);
	});

	test("table: not found", () => {
		expect(() => splitRawSection("table")).toThrow("table");
	});
});

describe("convertColumns", () => {
	test("empty", () => {
		expect(() => convertColumns([])).toThrow("table markdown");
	});

	test("1 line", () => {
		expect(() => convertColumns([""])).toThrow("table markdown");
	});

	test("2 line", () => {
		expect(() => convertColumns(["", ""])).toThrow("table markdown");
	});

	test("header short", () => {
		expect(() => convertColumns(["0|1|2|3|4|5|6", "", ""])).toThrow(
			"column length",
		);
	});

	test("header long", () => {
		expect(() => convertColumns(["0|1|2|3|4|5|6|*|8", "", ""])).toThrow(
			"column length",
		);
	});

	test("header short", () => {
		expect(() =>
			convertColumns(["0|1|2|3|4|5|6|7", "", "0|1|2|3|4|5|6"]),
		).toThrow("data length");
	});

	test("header long", () => {
		expect(() =>
			convertColumns(["0|1|2|3|4|5|6|7", "", "0|1|2|3|4|5|6|*|7"]),
		).toThrow("data length");
	});

	test.each([
		[true, "x"],
		[false, "o"],
		[false, " "],
	])("PK: 期待値 [%s], 入力値 [%s]", (expected: boolean, input: string) => {
		const columns = convertColumns([
			"0|1|2|3|4|5|6|7",
			"",
			`${input}|1|2|3|4|5|6|7`,
		]);
		expect(columns).toHaveLength(1);
		expect(columns[0]).toHaveProperty("isPrimary", expected);
	});

	test.each([
		[true, "x"],
		[false, "o"],
		[false, " "],
	])("NN: 期待値 [%s], 入力値 [%s]", (expected: boolean, input: string) => {
		const columns = convertColumns([
			"0|1|2|3|4|5|6|7",
			"",
			`0|${input}|2|3|4|5|6|7`,
		]);
		expect(columns).toHaveLength(1);
		expect(columns[0]).toHaveProperty("notNull", expected);
	});

	test("FK", () => {
		const columns = convertColumns([
			"0|1|2|3|4|5|6|7",
			"",
			"0|1|T.C|3|4|5|6|7",
		]);
		expect(columns).toHaveLength(1);
		expect(columns[0]).toHaveProperty("foreignKey", {
			table: "T",
			column: "C",
		});
	});

	test.each([[""], ["T"], [".C"]])("FK: undefined", (input) => {
		const columns = convertColumns([
			"0|1|2|3|4|5|6|7",
			"",
			`0|1|${input}|3|4|5|6|7`,
		]);
		expect(columns).toHaveLength(1);
		expect(columns[0]).toHaveProperty("foreignKey", undefined);
	});

	test("logical", () => {
		const columns = convertColumns(["0|1|2|3|4|5|6|7", "", "0|1|2|3|4|5|6|7"]);
		expect(columns).toHaveLength(1);
		expect(columns[0]).toHaveProperty("logical", {
			name: "3",
			type: "5",
		});
	});

	test("physical", () => {
		const columns = convertColumns(["0|1|2|3|4|5|6|7", "", "0|1|2|3|4|5|6|7"]);
		expect(columns).toHaveLength(1);
		expect(columns[0]).toHaveProperty("physicalName", "4");
		expect(columns[0]).toHaveProperty("clrType", "6");
	});

	test("check", () => {
		const columns = convertColumns(["0|1|2|3|4|5|6|7", "", "0|1|2|3|4|5|6|7"]);
		expect(columns).toHaveLength(1);
		expect(columns[0]).toHaveProperty("comment", "7");
	});
});

describe("convertIndexes", () => {
	test("empty", () => {
		expect(() => convertIndexes([])).toThrow("table markdown");
	});

	test("1 line", () => {
		expect(() => convertIndexes([""])).toThrow("table markdown");
	});

	test("2 line", () => {
		expect(() => convertIndexes(["", ""])).toThrow("table markdown");
	});

	test("header short", () => {
		expect(() => convertIndexes(["0|1", "", ""])).toThrow("column length");
	});

	test("header long", () => {
		expect(() => convertIndexes(["0|1|*|3", "", ""])).toThrow("column length");
	});

	test("header short", () => {
		expect(() => convertIndexes(["0|1|2", "", "0|1"])).toThrow("data length");
	});

	test("header long", () => {
		expect(() => convertIndexes(["0|1|2", "", "0|1|*|3"])).toThrow(
			"data length",
		);
	});

	test.each([
		[true, "x"],
		[false, "o"],
		[false, " "],
	])("UK: 期待値 [%s], 入力値 [%s]", (expected: boolean, input: string) => {
		const columns = convertIndexes(["0|1|2", "", `${input}|1|2`]);
		expect(columns).toHaveLength(1);
		expect(columns[0]).toHaveProperty("isUnique", expected);
	});

	test("name", () => {
		const columns = convertIndexes(["0|1|2", "", "0|1|2"]);
		expect(columns).toHaveLength(1);
		expect(columns[0]).toHaveProperty("name", "1");
	});

	test.each([
		[["A"], "A"],
		[["A", "B"], "A,B"],
		[["A", "B", "C"], "A , B, C "],
	])("columns", (expected: string[], input: string) => {
		const columns = convertIndexes(["0|1|2", "", `0|1|${input}`]);
		expect(columns).toHaveLength(1);
		expect(columns[0]).toHaveProperty("columns", expected);
	});
});
