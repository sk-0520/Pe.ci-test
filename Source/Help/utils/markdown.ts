import { NewLine, countSingleChar } from "./string";

export class MarkdownError extends Error {
	constructor(message: string) {
		super(message);
		this.name = this.constructor.name;
	}
}
export class MarkdownTableError extends MarkdownError {}

export type TableColumnAlign = "left" | "center" | "right";
const CellMinWidth = "---".length;
const CellPadding = 2;
export interface TableColumn {
	align?: TableColumnAlign;
	title: string;
}

function escapeCell(raw: string): string {
	return raw.replaceAll("|", "\\|");
}

export function buildCell(
	maxWidth: number,
	value: string,
	align: TableColumnAlign | undefined,
): string {
	const valueWidth = countSingleChar(value);
	const cellWidth = maxWidth;
	if (valueWidth === maxWidth) {
		return value;
	}
	if (cellWidth - valueWidth <= 0) {
		throw new MarkdownTableError("cell width");
	}

	switch (align) {
		case undefined:
		case "left":
			return `${value}${" ".repeat(cellWidth - valueWidth)}`;

		case "right":
			return `${" ".repeat(cellWidth - valueWidth)}${value}`;

		case "center":
			return `${" ".repeat((cellWidth - valueWidth) / 2)}${value}${" ".repeat((cellWidth - valueWidth) / 2 + ((cellWidth - valueWidth) % 2))}`;
	}
}

export function buildTable(
	columns: ReadonlyArray<TableColumn>,
	rows: ReadonlyArray<ReadonlyArray<string>>,
): string {
	if (!columns.length) {
		throw new MarkdownTableError("empty columns");
	}
	if (!rows.length) {
		throw new MarkdownTableError("empty rows");
	}

	const cellMaxLengths = columns.map((a) =>
		Math.max(CellMinWidth, countSingleChar(escapeCell(a.title)) + CellPadding),
	);
	const workRows: string[][] = [];
	for (const row of rows) {
		if (columns.length !== row.length) {
			throw new MarkdownError("size not equal columns rows.cells");
		}
		const workRow: string[] = [];
		for (let i = 0; i < row.length; i++) {
			// @ts-expect-error ts(2345)
			const cell = escapeCell(row[i]);
			workRow.push(cell);
			cellMaxLengths[i] = Math.max(
				// @ts-expect-error ts(2345)
				cellMaxLengths[i],
				countSingleChar(cell) + CellPadding,
			);
		}
		workRows.push(workRow);
	}

	const tableRows: Array<Array<string>> = [];

	tableRows.push(
		columns.map(
			(a, i) =>
				// @ts-expect-error ts(2345)
				` ${buildCell(cellMaxLengths[i] - CellPadding, escapeCell(a.title), "center")} `,
		),
	);
	//TODO: 幅調整はあとでやる
	tableRows.push(
		columns.map((a, i) => {
			switch (a.align) {
				case "left":
					// @ts-expect-error ts(2345)
					return `:${"-".repeat(cellMaxLengths[i] - 1)}`;

				case "right":
					// @ts-expect-error ts(2345)
					return `${"-".repeat(cellMaxLengths[i] - 1)}:`;

				case "center":
					// @ts-expect-error ts(2345)
					return `:${"-".repeat(cellMaxLengths[i] - 2)}:`;

				case undefined:
					// @ts-expect-error ts(2345)
					return `${"-".repeat(cellMaxLengths[i])}`;
			}
		}),
	);

	for (const row of workRows) {
		//TODO: 幅調整はあとでやる
		tableRows.push(
			row.map(
				(a, i) =>
					// @ts-expect-error ts(2345)
					` ${buildCell(cellMaxLengths[i] - CellPadding, a, columns[i].align)} `,
			),
		);
	}

	const result = tableRows.map((a) => `|${a.join("|")}|`).join(NewLine);
	return result;
}
