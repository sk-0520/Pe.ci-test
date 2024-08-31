import { splitLines, trim } from "./string";

const NoneIndex = "*NONE*";

const LayoutColumnIndex = {
	primaryKey: 0,
	notNull: 1,
	foreignKey: 2,
	logicalName: 3,
	physicalName: 4,
	logicalType: 5,
	cliType: 6,
	check: 7,
	comment: 8,
} as const;
const LayoutColumnLength = Object.keys(LayoutColumnIndex).length;

const IndexDefinedIndex = {
	uniqueKey: 0,
	name: 1,
	columnNames: 2,
};
const IndexDefinedLength = Object.keys(IndexDefinedIndex).length;

interface RawSection {
	table: string;
	layout: string[];
	index: string[];
}

export interface ForeignKey {
	table: string;
	column: string;
}

export interface TableColumn {
	isPrimary: boolean;
	notNull: boolean;
	foreignKey: ForeignKey | undefined;
	logical: {
		name: string;
		type: string;
	};
	physicalName: string;
	cliType: string;
	checkConstraints: string;
	comment: string;
}

export interface TableIndex {
	isUnique: boolean;
	name: string;
	columns: string[];
}

export interface TableDefine {
	name: string;
	columns: TableColumn[];
	indexes: TableIndex[];
}

export function splitRawEntities(markdown: string): string[] {
	if (!markdown) {
		return [];
	}

	return markdown.split(/^___$/gm).map((a) => a.trim());
}

const RawSectionHeaderRegex = {
	name: /^#\s(?<TABLE>\w+)$/,
	data: /^##\s(?:layout|index)$/gm,
} as const;

export function splitRawSection(rawEntity: string): RawSection {
	const section: RawSection = {
		table: "",
		layout: [],
		index: [],
	};

	const lines = splitLines(rawEntity);

	if (!lines.length) {
		throw new Error("table");
	}

	let lastIndex = 0;
	const tableLines = lines;
	for (const line of tableLines) {
		try {
			if (!line.trim()) {
				continue;
			}

			const result = line.match(RawSectionHeaderRegex.name);
			if (!result || !result.groups) {
				throw new Error("table");
			}

			section.table = result.groups.TABLE;
			break;
		} finally {
			lastIndex++;
		}
	}

	const dataLines = lines.splice(lastIndex);
	const data = dataLines
		.map((a) => a.trim())
		.filter((a) => a)
		.join("\n");

	const blocks = data.split(RawSectionHeaderRegex.data).filter((a) => a);
	if (blocks.length !== 2) {
		throw new Error("layout/index");
	}

	const layout = blocks[0].trim();
	const index = blocks[1].trim();

	section.layout = splitLines(layout);
	if (index !== NoneIndex) {
		section.index = splitLines(index);
	}

	return section;
}

function toCells(s: string): string[] {
	return trim(s, new Set("|"))
		.split("|")
		.map((a) => a.trim());
}

function isTrue(s: string): boolean {
	return s === "x";
}

export function convertColumns(lines: string[]): TableColumn[] {
	if (lines.length < 3) {
		throw new Error("table markdown");
	}

	const headers = toCells(lines[0]);

	if (headers.length !== LayoutColumnLength) {
		throw new Error("column length");
	}

	const result: TableColumn[] = [];

	const columnLines = lines.splice(2);
	for (const columnLine of columnLines) {
		const columns = toCells(columnLine);
		if (columns.length !== LayoutColumnLength) {
			throw new Error("data length");
		}

		const primaryKey = columns[LayoutColumnIndex.primaryKey];
		const notNull = columns[LayoutColumnIndex.notNull];
		const foreignKey = columns[LayoutColumnIndex.foreignKey];
		const logicalName = columns[LayoutColumnIndex.logicalName];
		const physicalName = columns[LayoutColumnIndex.physicalName];
		const logicalType = columns[LayoutColumnIndex.logicalType];
		const cliType = columns[LayoutColumnIndex.cliType];
		const check = columns[LayoutColumnIndex.check];
		const comment = columns[LayoutColumnIndex.comment];

		const foreignKeys = foreignKey
			.trim()
			.split(".", 2)
			.map((a) => a.trim())
			.filter((a) => a);

		const column: TableColumn = {
			isPrimary: isTrue(primaryKey),
			notNull: isTrue(notNull),
			foreignKey:
				foreignKeys.length === 2
					? {
							table: foreignKeys[0],
							column: foreignKeys[1],
						}
					: undefined,
			logical: {
				name: logicalName,
				type: logicalType,
			},
			physicalName: physicalName,
			cliType: cliType,
			checkConstraints: check,
			comment: comment,
		};

		result.push(column);
	}

	return result;
}

export function convertIndexes(lines: string[]): TableIndex[] {
	if (lines.length < 3) {
		throw new Error("table markdown");
	}

	const headers = toCells(lines[0]);

	if (headers.length !== IndexDefinedLength) {
		throw new Error("column length");
	}

	const result: TableIndex[] = [];

	const columnLines = lines.splice(2);
	for (const columnLine of columnLines) {
		const columns = toCells(columnLine);
		if (columns.length !== IndexDefinedLength) {
			throw new Error("data length");
		}

		const uniqueKey = columns[IndexDefinedIndex.uniqueKey];
		const name = columns[IndexDefinedIndex.name];
		const rawColumnNames = columns[IndexDefinedIndex.columnNames];

		const columnNames = rawColumnNames.split(",").map((a) => a.trim());
		if (!columnNames.length) {
			throw new Error("empty column");
		}

		const index: TableIndex = {
			isUnique: isTrue(uniqueKey),
			name: name,
			columns: columnNames,
		};

		result.push(index);
	}

	return result;
}

export function convertTable(section: RawSection): TableDefine {
	return {
		name: section.table,
		columns: convertColumns(section.layout),
		indexes: section.index.length ? convertIndexes(section.index) : [],
	};
}

export interface WorkOptions {
	id: string;
	tableName: string;
}

export interface WorkColumn extends TableColumn {
	id: string;
}

export interface WorkColumns {
	id: string; 
	items: WorkColumn[];
}

export interface WorkIndex extends TableIndex {
	id: string;
}

export interface WorkIndexes {
	id: string;
	items: WorkIndex[];
}

export interface WorkTable {
	id: string;
	options: WorkOptions;
	columns: WorkColumns;
	indexes: WorkIndexes;
}

export interface WorkIdMapping {
	optionId: string;
	columnsId: string;
	columnItemsId: string[];
	indexesId: string;
	indexItemsId: string[];
}

export function convertWorkTable(tableDefine: TableDefine): WorkTable {
	return {
		id: crypto.randomUUID(),
		options: {
			id: crypto.randomUUID(),
			tableName: tableDefine.name,
		},
		columns: {
			id: crypto.randomUUID(),
			items: tableDefine.columns.map((a) => ({
				...a,
				id: crypto.randomUUID(),
			})),
		},
		indexes: {
			id: crypto.randomUUID(),
			items: tableDefine.indexes.map((a) => ({
				...a,
				id: crypto.randomUUID(),
			})),
		},
	};
}

export function convertIdMap(
	workTables: ReadonlyArray<WorkTable>,
): Map<string, WorkIdMapping> {
	const map = new Map<string, WorkIdMapping>();

	for (const workTable of workTables) {
		map.set(workTable.id, {
			optionId: workTable.options.id,
			columnsId: workTable.columns.id,
			columnItemsId: workTable.columns.items.map((a) => a.id),
			indexesId: workTable.indexes.id,
			indexItemsId: workTable.indexes.items.map((a) => a.id),
		});
	}

	return map;
}
