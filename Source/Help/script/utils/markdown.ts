import { NewLine, countSingleChar } from "./string";

export type TableColumnAlign = "" | "left" | "center" | "right";
const MarkdownTableCellMinWidth = "---".length;

export interface TableColumn {
	align: TableColumnAlign;
	title: string;
}

export function buildTable(
	columns: ReadonlyArray<TableColumn>,
	rows: ReadonlyArray<ReadonlyArray<string>>,
): string {
	//TODO: 幅調整はあとでやる
	const cellLengths = columns.map((a) =>
		Math.max(MarkdownTableCellMinWidth, countSingleChar(a.title)),
	);
	for (const row of rows) {
		for (let i = 0; i < row.length; i++) {
			const col = row[i];
			cellLengths[i] = Math.max(cellLengths[i], countSingleChar(col));
		}
	}

	const tableRows: Array<Array<string>> = [];

	tableRows.push(columns.map((a) => a.title));
	//TODO: 幅調整はあとでやる
	tableRows.push(columns.map((a) => "---"));

	for (const row of rows) {
		//TODO: 幅調整はあとでやる
		tableRows.push(row.map((a) => a));
	}

	const result = tableRows.map((a) => ["|", a.join("|"), "|"]).join(NewLine);
	return result;
}
