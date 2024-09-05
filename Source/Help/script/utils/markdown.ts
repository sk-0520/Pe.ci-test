export type TableColumnAlign = "" | "left" | "center" | "right";

export interface TableColumn {
	align: TableColumnAlign;
	title: string;
}

export function buildTable(
	columns: ReadonlyArray<TableColumn>,
	rows: ReadonlyArray<ReadonlyArray<string>>,
): string {
	//const cellLengths: Array<number> = [];

	throw new Error();
}
