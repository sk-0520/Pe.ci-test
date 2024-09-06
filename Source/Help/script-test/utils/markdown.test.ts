import {
	MarkdownTableError,
	type TableColumn,
	type TableColumnAlign,
	buildCell,
	buildTable,
} from "../../script/utils/markdown";

describe("buildCell", () => {
	test("equal", () => {
		expect(buildCell(1, "s", undefined)).toBe("s");
	});

	test("cell width", () => {
		expect(() => buildCell(1, "あ", undefined)).toThrow(
			new MarkdownTableError("cell width"),
		);
	});

	test.each([
		["s", 1, "s", undefined],
		["s", 1, "s", "left" as TableColumnAlign],
		["s", 1, "s", "center" as TableColumnAlign],
		["s", 1, "s", "right" as TableColumnAlign],
		["s ", 2, "s", undefined],
		["s ", 2, "s", "left" as TableColumnAlign],
		["s ", 2, "s", "center" as TableColumnAlign],
		[" s", 2, "s", "right" as TableColumnAlign],
		["あ", 2, "あ", undefined],
		["あ", 2, "あ", "left" as TableColumnAlign],
		["あ", 2, "あ", "center" as TableColumnAlign],
		["あ", 2, "あ", "right" as TableColumnAlign],
		["s  ", 3, "s", undefined],
		["s  ", 3, "s", "left" as TableColumnAlign],
		[" s ", 3, "s", "center" as TableColumnAlign],
		["  s", 3, "s", "right" as TableColumnAlign],
		["あ ", 3, "あ", undefined],
		["あ ", 3, "あ", "left" as TableColumnAlign],
		["あ ", 3, "あ", "center" as TableColumnAlign],
		[" あ", 3, "あ", "right" as TableColumnAlign],
		["s   ", 4, "s", undefined],
		["s   ", 4, "s", "left" as TableColumnAlign],
		[" s  ", 4, "s", "center" as TableColumnAlign],
		["   s", 4, "s", "right" as TableColumnAlign],
		["あ  ", 4, "あ", undefined],
		["あ  ", 4, "あ", "left" as TableColumnAlign],
		[" あ ", 4, "あ", "center" as TableColumnAlign],
		["  あ", 4, "あ", "right" as TableColumnAlign],
	])(
		"each 期待値: [%s], maxWidth: [%d], value: [%s], align: [%s]",
		(
			expected: string,
			maxWidth: number,
			value: string,
			align: TableColumnAlign | undefined,
		) => {
			expect(buildCell(maxWidth, value, align)).toBe(expected);
		},
	);
});

describe("buildTable", () => {
	test("empty: columns", () => {
		expect(() => buildTable([], [])).toThrow(
			new MarkdownTableError("empty columns"),
		);
	});

	test("empty: rows", () => {
		expect(() => buildTable([{ title: "" }], [])).toThrow(
			new MarkdownTableError("empty rows"),
		);
	});

	test("size not equal columns rows.cells", () => {
		expect(() => buildTable([{ title: "" }], [["a", "b"]])).toThrow(
			new MarkdownTableError("size not equal columns rows.cells"),
		);
	});

	test.each([
		[
			`| A |
|---|
| a |`,
			[
				{
					title: "A",
				},
			],
			[["a"]],
		],
		[
			`| A |
|:--|
| a |`,
			[
				{
					title: "A",
					align: "left",
				},
			] satisfies TableColumn[],
			[["a"]],
		],
		[
			`| A |
|:-:|
| a |`,
			[
				{
					title: "A",
					align: "center",
				},
			] satisfies TableColumn[],
			[["a"]],
		],
		[
			`| A |
|--:|
| a |`,
			[
				{
					title: "A",
					align: "right",
				},
			] satisfies TableColumn[],
			[["a"]],
		],
		[
			`| A | あ |
|---|---:|
| a | 𩸽 |`,
			[
				{
					title: "A",
				},
				{
					title: "あ",
					align: "right",
				},
			] satisfies TableColumn[],
			[["a", "𩸽"]],
		],
		[
			`| A |  あ  |
|---|-----:|
| a |   𩸽 |
|   | 🐎🦌 |`,
			[
				{
					title: "A",
				},
				{
					title: "あ",
					align: "right",
				},
			] satisfies TableColumn[],
			[
				["a", "𩸽"],
				["", "🐎🦌"],
			],
		],
	])(
		"each 期待値: [%s]",
		(expected: string, columns: TableColumn[], rows: string[][]) => {
			expect(buildTable(columns, rows)).toBe(expected);
		},
	);
});
