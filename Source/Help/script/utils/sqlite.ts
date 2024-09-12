import { NewLine } from "./string";
import type { ForeignKey, TableColumn, TableDefine } from "./table";

export class Sqlite3Error extends Error {
	constructor(message: string) {
		super(message);
		this.name = this.constructor.name;
	}
}

const Indent = "\t";

export const Sqlite3BasicTypes = ["integer", "real", "text", "blob"] as const;
export const Sqlite3AffinityTypes = [
	"datetime", // sqlite3 的には数値っぽいから違うんやけどね
	"boolean",
] as const;
export type Sqlite3BasicType = (typeof Sqlite3BasicTypes)[number];
export type Sqlite3AffinityType = (typeof Sqlite3BasicTypes)[number];
export const Sqlite3Types = [
	...Sqlite3BasicTypes,
	...Sqlite3AffinityTypes,
] as const;
export type Sqlite3Type = (typeof Sqlite3Types)[number];

export const SqliteTypeMap = new Map<Sqlite3Type, Sqlite3BasicType>([
	// 通常
	["integer", "integer"],
	["real", "real"],
	["text", "text"],
	["blob", "blob"],
	// 意味だけ
	["datetime", "text"],
	["boolean", "integer"],
]) as ReadonlyMap<Sqlite3Type, Sqlite3BasicType>;

export function buildTable(table: TableDefine): string {
	const workLines: string[] = [];

	workLines.push(`create table ${table.name}`);
	workLines.push("(");

	const primaryColumns = table.columns.filter((a) => a.isPrimary);
	const foreignKeys = table.columns.filter(
		(a): a is TableColumn & { foreignKey: ForeignKey } =>
			a.foreignKey !== undefined,
	);

	const hasNext = primaryColumns.length || foreignKeys.length;

	for (let i = 0; i < table.columns.length; i++) {
		const column = table.columns[i];
		let columnStatement = `${column.physicalName} ${column.logical.type}`;
		if (column.notNull) {
			columnStatement += " not null";
		}
		const statementSeparator =
			i < table.columns.length - 1 ||
			(i === table.columns.length - 1 && hasNext);
		if (statementSeparator) {
			columnStatement += ",";
		}
		if (column.comment) {
			columnStatement += ` -- ${column.logical.name} ${column.comment}`;
		} else {
			columnStatement += ` -- ${column.logical.name}`;
		}

		workLines.push(Indent + columnStatement);
	}
	if (hasNext) {
		if (primaryColumns.length) {
			workLines.push(`${Indent}primary key`);
			workLines.push(`${Indent}(`);

			const primaries = primaryColumns.map((a) => a.physicalName);
			for (let i = 0; i < primaries.length; i++) {
				const isLast = i === primaries.length - 1;
				const primary = primaries[i];
				workLines.push(`${Indent}${Indent}${primary}${isLast ? "" : ","}`);
			}
			workLines.push(`${Indent})${foreignKeys.length ? "," : ""}`);
		}
		if (foreignKeys.length) {
			const foreignKeyItems = foreignKeys.map(
				(a) =>
					`foreign key(${a.physicalName}) references ${a.foreignKey.table}(${a.foreignKey.column})`,
			);
			for (let i = 0; i < foreignKeyItems.length; i++) {
				const isLast = i === foreignKeyItems.length - 1;
				const foreignKey = foreignKeyItems[i];
				workLines.push(`${Indent}${foreignKey}${isLast ? "" : ","}`);
			}
		}
	}

	workLines.push(")");
	workLines.push(";");

	return workLines.join(NewLine);
}

export function buildIndex(table: TableDefine): string {
	if (!table.indexes.length) {
		throw new Sqlite3Error("not found index");
	}

	const workLines: string[] = [];

	for (const index of table.indexes) {
		workLines.push(
			`create${index.isUnique ? " unique" : ""} index ${index.name} on ${table.name}`,
		);
		workLines.push("(");

		for (let i = 0; i < index.columns.length; i++) {
			const isLast = i === index.columns.length - 1;
			const column = index.columns[i];
			workLines.push(`${Indent}${column}${isLast ? "" : ","}`);
		}

		workLines.push(")");
		workLines.push(";");
	}

	return workLines.join(NewLine);
}
