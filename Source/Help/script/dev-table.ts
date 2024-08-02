// カラム名変更とか追加とか削除とかはキッツいので手動対応

interface BlockElements {
	root: HTMLDivElement;
	table: HTMLDivElement;
	layout: HTMLDivElement;
	index: HTMLDivElement;
}

interface EntityDefine {
	table: string;
	layout: ReadonlyArray<string>;
	index: ReadonlyArray<string>;
}

interface LayoutRowData {
	isPrimary: boolean;
	isNotNull: boolean;
	foreignTable: string;
	foreignColumn: string;
	columnName: string;
	logicalName: string;
	databaseType: string;
	clrType: string;
	check: string;
	comment: string;
}

interface IndexRowData {
	isUnique: boolean;
	/** インデックス名 */
	name: string;
	columns: Array<string>;
}

interface ExportData {
	markdown: string;
	database: string;
}

enum MarkdownTablePosition {
	left = 0,
	center = 1,
	right = 2,
}

const DatabaseTypeMap = new Map([
	// 通常
	["integer", "integer"],
	["real", "real"],
	["text", "text"],
	["blob", "blob"],
	// 意味だけ
	["datetime", "text"],
	["boolean", "integer"],
]) as ReadonlyMap<string, string>;

const ClrMap = new Map([
	["integer", ["System.Int64"]],
	["real", ["System.Decimal", "System.Single", "System.Double"]],
	[
		"text",
		["System.String", "System.Guid", "System.Version", "System.TimeSpan"],
	],
	["blob", ["System.Byte[]"]],
	["datetime", ["System.DateTime", "System.String"]],
	["boolean", ["System.Boolean", "System.Int64"]],
]) as ReadonlyMap<string, ReadonlyArray<string>>;

const CommonCreatedColumns = [
	"CreatedTimestamp",
	"CreatedAccount",
	"CreatedProgramName",
	"CreatedProgramVersion",
] as ReadonlyArray<string>;
const CommonUpdatedColumns = [
	"UpdatedTimestamp",
	"UpdatedAccount",
	"UpdatedProgramName",
	"UpdatedProgramVersion",
	"UpdatedCount",
] as ReadonlyArray<string>;

enum LayoutColumn {
	PrimaryKey = 0,
	NotNull = 1,
	ForeignKey = 2,
	LogicalColumnName = 3,
	PhysicalColumnName = 4,
	LogicalType = 5,
	ClrType = 6,
	CheckConstraint = 7,
	Comment = 8,
}

const LayoutMarkdownHeaders = (() => {
	const map = new Map<LayoutColumn, string>([
		[LayoutColumn.PrimaryKey, "PK"],
		[LayoutColumn.NotNull, "NN"],
		[LayoutColumn.ForeignKey, "FK"],
		[LayoutColumn.LogicalColumnName, "論理カラム名"],
		[LayoutColumn.PhysicalColumnName, "物理カラム名"],
		[LayoutColumn.LogicalType, "論理データ型"],
		[LayoutColumn.ClrType, "マッピング型"],
		[LayoutColumn.CheckConstraint, "チェック制約"],
		[LayoutColumn.Comment, "コメント"],
	]);
	// biome-ignore lint/style/noNonNullAssertion: <explanation>
	return [...map.keys()].sort((a, b) => a - b).map((i) => map.get(i)!);
})();

const IndexMarkdownHeaders = ["UK", "名前", "カラム(CSV)"];

enum TableBlockName {
	TableName = "table-name",
}

enum LayoutBlockName {
	LayoutRowRoot = "layout-row-root",
	LayoutRowAdd = "add",
	PrimaryKey = "pk",
	NotNull = "nn",
	ForeignKeyRoot = "fk-root",
	ForeignKey = "fk",
	ForeignKeyTable = "fk-table",
	ForeignKeyColumn = "fk-column",
	LogicalColumnName = "name-logical",
	PhysicalColumnName = "name-physical",
	LogicalType = "data-logical",
	PhysicalType = "data-physical",
	ClrType = "data-clr",
	CheckConstraint = "check",
	Comment = "comment",
	Delete = "delete",
}

enum IndexBlockName {
	IndexRowRoot = "index-row-root",
	IndexRowAdd = "add",
	ColumnAdd = "add-col",
	IndexRowColumnRoot = "index-row-column-root",
	ColumnName = "c",
	UniqueKey = "uk",
	IndexName = "name",
	Columns = "columns",
	Column = "column",
	DeleteColumn = "delete-col",
	DeleteIndex = "delete",
}

function getElementByName<THTMLElement extends HTMLElement>(
	node: ParentNode,
	name: string,
): THTMLElement {
	return node.querySelector(`[name="${name}"]`) as THTMLElement;
}

function getElementsByName<THTMLElement extends HTMLElement>(
	node: ParentNode,
	name: string,
): NodeListOf<THTMLElement> {
	return node.querySelectorAll(`[name="${name}"]`);
}

function getClosest(
	element: HTMLElement,
	func: (target: HTMLElement) => boolean,
): HTMLElement | null {
	let workElement = element;

	while (workElement.parentElement) {
		if (func(workElement.parentElement)) {
			return workElement.parentElement;
		}

		workElement = workElement.parentElement;
	}

	return null;
}

function isCheckMark(value: string) {
	return value === "o";
}
function toCheckMark(value: boolean) {
	return value ? "o" : "";
}

function countSingleChar(s: string): number {
	if (!s || !s.length) {
		return 0;
	}
	const chars =
		s.match(/[\uD800-\uDBFF][\uDC00-\uDFFF]|[^\uD800-\uDFFF]/g) || [];
	let length = 0;
	for (const c of chars) {
		if (c.length === 1) {
			// biome-ignore lint/suspicious/noControlCharactersInRegex: <explanation>
			if (!c.match(/[^\x01-\x7E]/) || !c.match(/[^\uFF65-\uFF9F]/)) {
				length += 1;
			} else {
				length += 2;
			}
		} else {
			// もういいでしょ
			length += 2;
		}
	}
	return length;
}

class Entity {
	private readonly tableNamePrefix = "## ";
	private blockElements: BlockElements;
	private entities: ReadonlyArray<Entity> = [];

	constructor(blockElements: BlockElements) {
		this.blockElements = blockElements;
	}

	private getIndex(lines: ReadonlyArray<string>) {
		const regLayout = /^###\s*layout\s*/;
		const regIndex = /^###\s*index\s*/;

		enum State {
			Table = 0,
			Layout = 1,
			Index = 2,
		}

		let state = State.Table;

		const lineIndex = {
			table: -1,
			layout: {
				head: -1,
				tail: -1,
			},
			index: {
				head: -1,
				tail: -1,
			},
		};

		for (let i = 0; i < lines.length; i++) {
			const line = lines[i];

			switch (state) {
				case State.Table:
					if (line.startsWith(this.tableNamePrefix)) {
						lineIndex.table = i;
						state = State.Layout;
					}
					break;

				case State.Layout:
					if (regLayout.test(line)) {
						lineIndex.layout.head = i + 1;
						state = State.Index;
					}
					break;

				case State.Index:
					if (regIndex.test(line)) {
						lineIndex.layout.tail = i;
						lineIndex.index.head = i + 1;
						lineIndex.index.tail = lines.length;
						return lineIndex;
					}
					break;

				default:
					throw Error("こねーよ！");
			}
		}

		throw Error(`はい、定義ミス:${JSON.stringify(lines)}`);
	}

	private trimMarkdownTable(
		lines: ReadonlyArray<string>,
	): ReadonlyArray<string> {
		return lines
			.map((s) => s.trim())
			.filter((s) => s.startsWith("|") && s.endsWith("|"));
	}

	private trimDefine(rawDefine: EntityDefine): EntityDefine {
		const result = {
			table: rawDefine.table.substring(this.tableNamePrefix.length),
			layout: this.trimMarkdownTable(rawDefine.layout),
			index: this.trimMarkdownTable(rawDefine.index),
		} as EntityDefine;

		return result;
	}

	private convertRowLines(
		markdownTableLines: ReadonlyArray<string>,
	): ReadonlyArray<ReadonlyArray<string>> {
		const rows = markdownTableLines
			.map((i) => i.replace(/(^\|)|(|$)/, ""))
			.map((i) => i.split("|").map((s) => s.trim()));
		if (2 < rows.length) {
			return rows.slice(2);
		}

		return [];
	}

	private buildTable(parentElement: HTMLDivElement, tableName: string) {
		const tableTemplate = document.getElementById(
			"template-table",
		) as HTMLTemplateElement;
		const clonedTemplate = document.importNode(tableTemplate.content, true);

		const tableNameElement = getElementByName<HTMLInputElement>(
			clonedTemplate,
			TableBlockName.TableName,
		);
		tableNameElement.value = tableName;

		parentElement.appendChild(clonedTemplate);
	}

	private createLayoutRowNode(columns: ReadonlyArray<string>) {
		const layoutRowTemplate = document.getElementById(
			"template-layout-row",
		) as HTMLTemplateElement;
		const clonedTemplate = document.importNode(layoutRowTemplate.content, true);

		const primaryElement = getElementByName<HTMLInputElement>(
			clonedTemplate,
			LayoutBlockName.PrimaryKey,
		);
		const notNullElement = getElementByName<HTMLInputElement>(
			clonedTemplate,
			LayoutBlockName.NotNull,
		);
		primaryElement.checked = isCheckMark(columns[LayoutColumn.PrimaryKey]);
		notNullElement.checked = isCheckMark(columns[LayoutColumn.NotNull]);
		primaryElement.addEventListener("change", (_) => {
			notNullElement.disabled = primaryElement.checked;
			if (primaryElement.checked) {
				notNullElement.checked = true;
			}
		});
		primaryElement.dispatchEvent(new Event("change"));

		getElementByName<HTMLInputElement>(
			clonedTemplate,
			LayoutBlockName.ForeignKey,
		).value = columns[LayoutColumn.ForeignKey];

		getElementByName<HTMLInputElement>(
			clonedTemplate,
			LayoutBlockName.LogicalColumnName,
		).value = columns[LayoutColumn.LogicalColumnName];
		getElementByName<HTMLInputElement>(
			clonedTemplate,
			LayoutBlockName.PhysicalColumnName,
		).value = columns[LayoutColumn.PhysicalColumnName];

		const logicalDataElement = getElementByName<HTMLSelectElement>(
			clonedTemplate,
			LayoutBlockName.LogicalType,
		);
		const physicalDataElement = getElementByName<HTMLInputElement>(
			clonedTemplate,
			LayoutBlockName.PhysicalType,
		); // 一方通行イベントで使うのでキャプチャしとく。メモリは無限
		const clrDataElement = getElementByName<HTMLSelectElement>(
			clonedTemplate,
			LayoutBlockName.ClrType,
		);
		logicalDataElement.value = columns[LayoutColumn.LogicalType];
		clrDataElement.value = columns[LayoutColumn.ClrType];
		logicalDataElement.addEventListener("change", (_) => {
			const physicalValue = DatabaseTypeMap.get(logicalDataElement.value);
			physicalDataElement.value = physicalValue || "なんかデータ変";

			// CLR に対して Pe で出来る範囲で型を限定
			const optionElements = clrDataElement.querySelectorAll("option");
			let selectedElement: HTMLOptionElement | null = null;
			let defaultElement: HTMLOptionElement | null = null;
			for (const optionElement of optionElements) {
				const clrValues = ClrMap.get(logicalDataElement.value);
				if (!clrValues) {
					logicalDataElement.parentElement?.parentElement?.parentElement?.classList.add(
						"error-parent",
					);
					logicalDataElement.parentElement?.parentElement?.classList.add(
						"error-row",
					);
					throw new Error(
						`clrValues が取得できない, たぶん 論理型 が不明: ${logicalDataElement.value}:${physicalValue}`,
					);
				}
				optionElement.disabled = !clrValues.some(
					(i) => i === optionElement.value,
				);
				if (!optionElement.disabled && !defaultElement) {
					if (clrValues[0] === optionElement.value) {
						defaultElement = optionElement;
					}
				}
				if (optionElement.selected) {
					selectedElement = optionElement;
				}
			}
			if (defaultElement && selectedElement?.disabled) {
				defaultElement.selected = true;
			}
		});
		logicalDataElement.dispatchEvent(new Event("change"));

		getElementByName<HTMLInputElement>(
			clonedTemplate,
			LayoutBlockName.CheckConstraint,
		).value = columns[LayoutColumn.CheckConstraint];
		getElementByName<HTMLInputElement>(
			clonedTemplate,
			LayoutBlockName.Comment,
		).value = columns[LayoutColumn.Comment];

		getElementByName<HTMLButtonElement>(
			clonedTemplate,
			LayoutBlockName.Delete,
		).addEventListener("click", (ev) => {
			let element = ev.target as HTMLElement;
			while (element.getAttribute("name") !== LayoutBlockName.LayoutRowRoot) {
				element = element.parentElement as HTMLElement;
			}
			element.remove();
		});

		return clonedTemplate;
	}

	private createEmptyLayout(): ReadonlyArray<string> {
		const defaultDatabaseType = "integer";

		const map = new Map<LayoutColumn, string>([
			[LayoutColumn.PrimaryKey, ""],
			[LayoutColumn.NotNull, ""],
			[LayoutColumn.ForeignKey, ""],
			[LayoutColumn.LogicalColumnName, ""],
			[LayoutColumn.PhysicalColumnName, ""],
			[LayoutColumn.LogicalType, defaultDatabaseType],
			// biome-ignore lint/style/noNonNullAssertion: <explanation>
			[LayoutColumn.ClrType, ClrMap.get(defaultDatabaseType)![0]],
			[LayoutColumn.CheckConstraint, ""],
			[LayoutColumn.Comment, ""],
		]);

		// biome-ignore lint/style/noNonNullAssertion: <explanation>
		return [...map.keys()].sort((a, b) => a - b).map((i) => map.get(i)!);
	}

	private buildLayout(
		parentElement: HTMLDivElement,
		layoutRows: ReadonlyArray<ReadonlyArray<string>>,
	) {
		const layoutTemplate = document.getElementById(
			"template-layout",
		) as HTMLTemplateElement;
		const clonedTemplate = document.importNode(layoutTemplate.content, true);

		const rowsElement = clonedTemplate.querySelector("tbody") as HTMLElement;

		for (const layoutRow of layoutRows) {
			const rowElement = this.createLayoutRowNode(layoutRow);
			rowsElement.appendChild(rowElement);
		}

		getElementByName<HTMLButtonElement>(
			clonedTemplate,
			LayoutBlockName.LayoutRowAdd,
		).addEventListener("click", (ev) => {
			let element = ev.target as HTMLElement;
			while (element.tagName !== "TABLE") {
				element = element.parentElement as HTMLElement;
			}

			const emptyLayout = this.createEmptyLayout();
			const rowElement = this.createLayoutRowNode(emptyLayout);

			rowsElement.appendChild(rowElement);
			const newRowElement = rowsElement.lastElementChild as HTMLElement;
			const tableElement = getElementByName<HTMLSelectElement>(
				newRowElement,
				LayoutBlockName.ForeignKeyTable,
			);
			const targetEntities = this.filterMyself(this.entities);
			const targetTableNames = this.getTableNamesFromEntities(targetEntities);
			this.buildForeignKeyTable(tableElement, targetTableNames);
			tableElement.addEventListener("change", (ev) =>
				this.changedTableElement(ev, targetEntities),
			);
		});

		parentElement.appendChild(clonedTemplate);
	}

	private createIndexRowColumnNode(column: string) {
		const indexRowColumnTemplate = document.getElementById(
			"template-index-row-column",
		) as HTMLTemplateElement;
		const clonedTemplate = document.importNode(
			indexRowColumnTemplate.content,
			true,
		);

		const columnElement = getElementByName<HTMLInputElement>(
			clonedTemplate,
			IndexBlockName.ColumnName,
		);
		columnElement.value = column;

		const columnOptionElement = getElementByName<HTMLOptionElement>(
			clonedTemplate,
			IndexBlockName.Column,
		);
		columnOptionElement.addEventListener("change", (_) => {
			columnElement.value = columnOptionElement.value;
		});

		getElementByName<HTMLButtonElement>(
			clonedTemplate,
			IndexBlockName.DeleteColumn,
		).addEventListener("click", (ev) => {
			const parent = getClosest(
				ev.target as HTMLElement,
				(e) => e.getAttribute("name") === IndexBlockName.IndexRowColumnRoot,
			);
			parent?.remove();
		});

		return clonedTemplate;
	}

	private createIndexRowNode(
		isUnique: boolean,
		indexName: string,
		columns: ReadonlyArray<string>,
	): Node {
		const indexRowTemplate = document.getElementById(
			"template-index-row",
		) as HTMLTemplateElement;
		const clonedTemplate = document.importNode(indexRowTemplate.content, true);

		getElementByName<HTMLInputElement>(
			clonedTemplate,
			IndexBlockName.UniqueKey,
		).checked = isUnique;

		getElementByName<HTMLInputElement>(
			clonedTemplate,
			IndexBlockName.IndexName,
		).value = indexName;

		const columnsElement = getElementByName(
			clonedTemplate,
			IndexBlockName.Columns,
		);
		for (const column of columns) {
			const columnElement = this.createIndexRowColumnNode(column);
			columnsElement.appendChild(columnElement);
		}

		getElementByName<HTMLButtonElement>(
			clonedTemplate,
			IndexBlockName.ColumnAdd,
		).addEventListener("click", (_) => {
			const columnElement = this.createIndexRowColumnNode(
				this.getColumnNames(true)[0],
			);
			columnsElement.appendChild(columnElement);

			const columnNames = this.getColumnNames(true);
			const targetElement = getElementByName<HTMLSelectElement>(
				// biome-ignore lint/style/noNonNullAssertion: <explanation>
				columnsElement.lastElementChild!,
				IndexBlockName.Column,
			);
			this.setIndexColumnNames(targetElement, columnNames);
		});

		getElementByName<HTMLButtonElement>(
			clonedTemplate,
			IndexBlockName.DeleteIndex,
		).addEventListener("click", (ev) => {
			const parent = getClosest(
				ev.target as HTMLElement,
				(e) => e.getAttribute("name") === IndexBlockName.IndexRowRoot,
			);
			parent?.remove();
		});

		return clonedTemplate;
	}

	private buildIndex(
		parentElement: HTMLDivElement,
		indexRows: ReadonlyArray<ReadonlyArray<string>>,
	) {
		const indexTemplate = document.getElementById(
			"template-index",
		) as HTMLTemplateElement;
		const clonedTemplate = document.importNode(indexTemplate.content, true);

		const rowsElement = clonedTemplate.querySelector("tbody") as HTMLElement;

		for (const indexRow of indexRows) {
			const isUnique = isCheckMark(indexRow[0]);
			const indexName = indexRow[1].trim();
			const columns = indexRow[2].split(",").map((s) => s.trim());
			const rowElement = this.createIndexRowNode(isUnique, indexName, columns);
			rowsElement.appendChild(rowElement);
		}

		getElementByName<HTMLButtonElement>(
			clonedTemplate,
			IndexBlockName.IndexRowAdd,
		).addEventListener("click", (_) => {
			const rowElement = this.createIndexRowNode(false, "", [
				this.getColumnNames(true)[0],
			]);
			rowsElement.appendChild(rowElement);

			const columnElement = getElementByName<HTMLSelectElement>(
				// biome-ignore lint/style/noNonNullAssertion: <explanation>
				rowsElement.lastElementChild!,
				IndexBlockName.Column,
			);
			const columnNames = this.getColumnNames(true);
			this.setIndexColumnNames(columnElement, columnNames);
		});

		parentElement.appendChild(clonedTemplate);
	}

	public build(rawDefine: string) {
		const lines = rawDefine.split(/\r?\n/gm);
		const index = this.getIndex(lines);

		const define = this.trimDefine({
			table: lines[index.table],
			layout: lines.slice(index.layout.head, index.layout.tail),
			index: lines.slice(index.index.head, index.index.tail),
		});

		this.buildTable(this.blockElements.table, define.table);
		this.buildLayout(
			this.blockElements.layout,
			this.convertRowLines(define.layout),
		);
		this.buildIndex(
			this.blockElements.index,
			this.convertRowLines(define.index),
		);
	}

	public getTableName(): string {
		return getElementByName<HTMLInputElement>(
			this.blockElements.table,
			TableBlockName.TableName,
		).value;
	}

	private buildForeignKeyTable(
		parentElement: HTMLSelectElement,
		tableNames: ReadonlyArray<string>,
	) {
		parentElement.appendChild(document.createElement("option"));
		for (const tableName of tableNames) {
			const optionElement = document.createElement(
				"option",
			) as HTMLOptionElement;
			optionElement.value = tableName;
			optionElement.textContent = tableName;

			parentElement.appendChild(optionElement);
		}
	}

	public getColumnNames(ignoreCommonColumn: boolean): ReadonlyArray<string> {
		const isIgnoreColumn = (s: string) => {
			if (CommonCreatedColumns.some((i) => i === s)) {
				return true;
			}
			if (CommonUpdatedColumns.some((i) => i === s)) {
				return true;
			}

			return false;
		};
		const columnElements = getElementsByName<HTMLInputElement>(
			this.blockElements.layout,
			LayoutBlockName.PhysicalColumnName,
		);
		const result = new Array<string>();
		for (const columnElement of columnElements) {
			if (ignoreCommonColumn && isIgnoreColumn(columnElement.value)) {
				continue;
			}
			result.push(columnElement.value);
		}
		return result;
	}

	private buildForeignKeyColumns(
		parentElement: HTMLSelectElement,
		targetEntity: Entity,
	) {
		parentElement.textContent = "";

		const columnNames = targetEntity.getColumnNames(true);
		for (const columnName of columnNames) {
			const optionElement = document.createElement("option");
			optionElement.value = columnName;
			optionElement.textContent = columnName;
			parentElement.appendChild(optionElement);
		}
	}

	private filterMyself(entities: ReadonlyArray<Entity>): ReadonlyArray<Entity> {
		const targetEntities = entities.filter((i) => i !== this);

		return targetEntities;
	}

	private getTableNamesFromEntities(entities: ReadonlyArray<Entity>) {
		return entities
			.map((i) => i.getTableName())
			.sort((a, b) => a.localeCompare(b));
	}

	private changedTableElement(
		ev: Event,
		targetEntities: ReadonlyArray<Entity>,
	) {
		const currentTableElement = ev.target as HTMLSelectElement;
		const currentColumnElement = getElementByName<HTMLSelectElement>(
			// biome-ignore lint/style/noNonNullAssertion: <explanation>
			currentTableElement.parentElement!,
			LayoutBlockName.ForeignKeyColumn,
		);
		const targetEntity = targetEntities.find(
			(i) => i.getTableName() === currentTableElement.value,
		);

		if (targetEntity) {
			currentColumnElement.disabled = false;
			this.buildForeignKeyColumns(currentColumnElement, targetEntity);
		} else {
			currentColumnElement.disabled = true;
			currentColumnElement.textContent = "";
		}
	}

	private buildEntityForeignKey(entities: ReadonlyArray<Entity>) {
		const foreignKeyRootElements = getElementsByName(
			this.blockElements.layout,
			LayoutBlockName.ForeignKeyRoot,
		);
		for (const foreignKeyRootElement of foreignKeyRootElements) {
			const tableElement = getElementByName<HTMLSelectElement>(
				foreignKeyRootElement,
				LayoutBlockName.ForeignKeyTable,
			);
			const columnElement = getElementByName<HTMLSelectElement>(
				foreignKeyRootElement,
				LayoutBlockName.ForeignKeyColumn,
			);

			const targetEntities = this.filterMyself(entities);
			const targetTableNames = this.getTableNamesFromEntities(targetEntities);

			this.buildForeignKeyTable(tableElement, targetTableNames);
			tableElement.addEventListener("change", (ev) =>
				this.changedTableElement(ev, targetEntities),
			);

			const kfElement = getElementByName<HTMLInputElement>(
				foreignKeyRootElement,
				LayoutBlockName.ForeignKey,
			);
			if (kfElement.value) {
				const v = kfElement.value.split(".");
				const kfPair = {
					table: v[0].trim(),
					column: v[1].trim(),
				};
				tableElement.value = kfPair.table;
				tableElement.dispatchEvent(new Event("change"));
				columnElement.value = kfPair.column;
			} else {
				tableElement.dispatchEvent(new Event("change"));
			}
			kfElement.value = "";
		}
	}

	private setIndexColumnNames(
		parentElement: HTMLSelectElement,
		columnNames: ReadonlyArray<string>,
	) {
		for (const columnName of columnNames) {
			const optionElement = document.createElement("option");
			optionElement.value = columnName;
			optionElement.textContent = columnName;

			parentElement.appendChild(optionElement);
		}
	}

	private buildEntityIndex() {
		const columnNames = this.getColumnNames(true);
		const indexRowRootElements = getElementsByName(
			this.blockElements.index,
			IndexBlockName.IndexRowRoot,
		);
		for (const indexRowRootElement of indexRowRootElements) {
			const columnsElements = getElementsByName<HTMLSelectElement>(
				indexRowRootElement,
				IndexBlockName.Column,
			);
			for (const columnsElement of columnsElements) {
				/*
				for(var columnName of columnNames) {
					var optionElement = document.createElement('option');
					optionElement.value = columnName;
					optionElement.textContent = columnName;

					columnsElement.appendChild(optionElement);
				}
				*/
				this.setIndexColumnNames(columnsElement, columnNames);
				columnsElement.value = getElementByName<HTMLInputElement>(
					// biome-ignore lint/style/noNonNullAssertion: <explanation>
					columnsElement.parentElement!,
					IndexBlockName.ColumnName,
				).value;
			}
		}
	}

	public buildEntities(entities: ReadonlyArray<Entity>) {
		this.entities = entities;
		this.buildEntityForeignKey(this.entities);
		this.buildEntityIndex();
	}
}

class EntityRelationManager {
	filterElemenet: HTMLDivElement;
	viewElement: HTMLDivElement;
	commandElement: HTMLDivElement;
	defineElement: HTMLTextAreaElement;
	sqlElement: HTMLTextAreaElement;

	entities: Array<Entity> = [];

	constructor(
		filterElemenet: HTMLDivElement,
		viewElement: HTMLDivElement,
		commandElement: HTMLDivElement,
		defineElement: HTMLTextAreaElement,
		sqlElement: HTMLTextAreaElement,
	) {
		this.filterElemenet = filterElemenet;
		this.viewElement = viewElement;
		this.commandElement = commandElement;
		this.defineElement = defineElement;
		this.sqlElement = sqlElement;
	}

	private createBlockElements(): BlockElements {
		const blockTemplate = document.getElementById(
			"template-block",
		) as HTMLTemplateElement;
		const clonedTemplate = document.importNode(blockTemplate.content, true);
		const block = {
			root: clonedTemplate.querySelector('[name="block-root"]'),
			table: clonedTemplate.querySelector('[name="block-table"]'),
			layout: clonedTemplate.querySelector('[name="block-layout"]'),
			index: clonedTemplate.querySelector('[name="block-index"]'),
		} as BlockElements;

		return block;
	}

	private buildFilter(parentElement: HTMLDivElement) {
		const filterTemplate = document.getElementById(
			"template-filter",
		) as HTMLTemplateElement;
		const clonedTemplate = document.importNode(filterTemplate.content, true);

		const tableFilterElement = getElementByName<HTMLInputElement>(
			clonedTemplate,
			"table-filter",
		);

		tableFilterElement.addEventListener("input", (_) => {
			const elements = getElementsByName<HTMLDivElement>(
				this.viewElement,
				"block-root",
			);
			const inputValue = tableFilterElement.value.trim().toLowerCase();
			const reg = (() => {
				try {
					if (inputValue.indexOf("/") === 0) {
						return new RegExp(inputValue.substr(1));
					}
					const headPattern = `^\\s*${inputValue.replace(/[-/\\^$*+?.()|[\]{}]/g, "\\$&")}`;
					return new RegExp(headPattern);
				} catch {
					return /(?:)/;
				}
			})();

			for (const element of elements) {
				const tableName = getElementByName<HTMLInputElement>(
					element,
					TableBlockName.TableName,
				)
					.value.trim()
					.toLowerCase();
				if (reg.test(tableName)) {
					element.style.display = "block";
				} else {
					element.style.display = "none";
				}
			}
		});

		parentElement.appendChild(clonedTemplate);
	}

	private buildCommand(parentElement: HTMLDivElement) {
		const commandTemplate = document.getElementById(
			"template-command",
		) as HTMLTemplateElement;
		const clonedTemplate = document.importNode(commandTemplate.content, true);

		const importElement = clonedTemplate.querySelector(
			'[name="command-import"]',
		) as HTMLButtonElement;
		importElement.addEventListener("click", (_) => {
			this.viewElement.textContent = "";
			this.buildCore(true);
			this.export();
		});

		const exportElement = clonedTemplate.querySelector(
			'[name="command-export"]',
		) as HTMLButtonElement;
		exportElement.addEventListener("click", (_) => {
			this.export();
		});

		const copyDefineElement = clonedTemplate.querySelector(
			'[name="command-copy-define"]',
		) as HTMLButtonElement;
		copyDefineElement.addEventListener("click", (_) => {
			this.copyDefine();
		});

		const copySqlElement = clonedTemplate.querySelector(
			'[name="command-copy-sql"]',
		) as HTMLButtonElement;
		copySqlElement.addEventListener("click", (_) => {
			this.copySql();
		});

		parentElement.appendChild(clonedTemplate);
	}

	private buildEntityMapping(entities: ReadonlyArray<Entity>) {
		for (const entity of entities) {
			entity.buildEntities(entities);
		}
	}

	private buildCore(rebuild: boolean) {
		if (rebuild) {
			const filterElements = document.getElementsByClassName("filter");
			for (const filterElement of filterElements) {
				filterElement.textContent = "";
			}
		} else {
			this.buildCommand(this.commandElement);
		}

		const defines = this.defineElement.value.split("___");
		defines.shift();

		for (const define of defines) {
			const blockElements = this.createBlockElements();
			const entity = new Entity(blockElements);
			entity.build(define);

			this.entities.push(entity);

			this.viewElement.appendChild(blockElements.root);
		}

		this.buildEntityMapping(this.entities);

		this.buildFilter(this.filterElemenet);
	}

	public build() {
		this.buildCore(false);
	}

	reset() {
		this.filterElemenet.textContent = "";
		this.entities = [];
		this.viewElement.textContent = "";
		this.commandElement.textContent = "";
	}

	private exportTable(tableElement: HTMLElement): string {
		const tableNameElement = getElementByName<HTMLInputElement>(
			tableElement,
			TableBlockName.TableName,
		);
		return tableNameElement.value;
	}

	private getLayoutRowData(rowElement: HTMLTableRowElement): LayoutRowData {
		const data = {
			isPrimary: getElementByName<HTMLInputElement>(
				rowElement,
				LayoutBlockName.PrimaryKey,
			).checked,
			isNotNull: getElementByName<HTMLInputElement>(
				rowElement,
				LayoutBlockName.NotNull,
			).checked,
			foreignTable: "",
			foreignColumn: "",
			columnName: getElementByName<HTMLInputElement>(
				rowElement,
				LayoutBlockName.PhysicalColumnName,
			).value,
			logicalName: getElementByName<HTMLInputElement>(
				rowElement,
				LayoutBlockName.LogicalColumnName,
			).value,
			databaseType: getElementByName<HTMLSelectElement>(
				rowElement,
				LayoutBlockName.LogicalType,
			).value,
			clrType: getElementByName<HTMLSelectElement>(
				rowElement,
				LayoutBlockName.ClrType,
			).value,
			check: getElementByName<HTMLInputElement>(
				rowElement,
				LayoutBlockName.CheckConstraint,
			).value,
			comment: getElementByName<HTMLInputElement>(
				rowElement,
				LayoutBlockName.Comment,
			).value,
		} as LayoutRowData;

		const ft = getElementByName<HTMLInputElement>(
			rowElement,
			LayoutBlockName.ForeignKeyTable,
		).value;
		const fc = getElementByName<HTMLInputElement>(
			rowElement,
			LayoutBlockName.ForeignKeyColumn,
		).value;
		if (ft.length && fc.length) {
			data.foreignTable = ft;
			data.foreignColumn = fc;
		}

		return data;
	}

	private toLayoutMarkdown(row: LayoutRowData): ReadonlyArray<string> {
		const map = new Map<LayoutColumn, string>([
			[LayoutColumn.PrimaryKey, toCheckMark(row.isPrimary)],
			[LayoutColumn.NotNull, toCheckMark(row.isNotNull)],
			[
				LayoutColumn.ForeignKey,
				row.foreignTable.length && row.foreignColumn.length
					? `${row.foreignTable}.${row.foreignColumn}`
					: "",
			],
			[LayoutColumn.LogicalColumnName, row.logicalName],
			[LayoutColumn.PhysicalColumnName, row.columnName],
			[LayoutColumn.LogicalType, row.databaseType],
			[LayoutColumn.ClrType, row.clrType],
			[LayoutColumn.CheckConstraint, row.check],
			[LayoutColumn.Comment, row.comment],
		]);
		// biome-ignore lint/style/noNonNullAssertion: <explanation>
		return [...map.keys()].sort((a, b) => a - b).map((i) => map.get(i)!);
	}

	private toLayoutDatabase(row: LayoutRowData): string {
		let sql = `[${row.columnName}] ${row.databaseType}`;
		if (row.isNotNull) {
			sql += " not null";
		}
		if (row.check.length) {
			sql += ` check( ${row.check} )`;
		}

		if (row.logicalName.length || row.comment.length) {
			sql += ` /* ${row.logicalName}`;
			if (row.logicalName.length) {
				sql += " ";
			}
			sql += row.comment;
			sql += " */";
		}

		return sql;
	}

	private getMarkdownCellSpace(
		data: ReadonlyArray<ReadonlyArray<string>>,
	): ReadonlyArray<number> {
		const result = new Array<number>(data[0].length).fill(0);

		for (let col = 0; col < data[0].length; col++) {
			for (const row of data) {
				const colLength = countSingleChar(row[col]);
				result[col] = Math.max(result[col], colLength);
			}
		}

		return result;
	}

	private toMarkdownCell(
		value: string,
		length: number,
		position: MarkdownTablePosition,
	) {
		const valueLength = countSingleChar(value);

		if (valueLength === length) {
			return value;
		}
		switch (position) {
			case MarkdownTablePosition.left:
				return value + " ".repeat(length - valueLength);

			case MarkdownTablePosition.right:
				return " ".repeat(length - valueLength) + value;

			case MarkdownTablePosition.center:
				return (
					" ".repeat((length - valueLength) / 2) +
					value +
					" ".repeat((length - valueLength) / 2 + ((length - valueLength) % 2))
				);
		}
	}

	private toMarkdownTableCells(
		cells: ReadonlyArray<string>,
		positions: Map<number, MarkdownTablePosition>,
		space: ReadonlyArray<number>,
	): ReadonlyArray<string> {
		const result = new Array<string>(cells.length);
		for (let i = 0; i < cells.length; i++) {
			const position = positions.get(i) ?? MarkdownTablePosition.left;
			const value = this.toMarkdownCell(cells[i], space[i], position);
			result[i] = value;
		}

		return result;
	}

	private toMarkdown(
		header: ReadonlyArray<string>,
		positions: Map<number, MarkdownTablePosition>,
		contents: ReadonlyArray<ReadonlyArray<string>>,
	) {
		const cellSpace = this.getMarkdownCellSpace([header].concat(contents));

		const lines = new Array<string>();
		lines.push(
			`| ${this.toMarkdownTableCells(header, positions, cellSpace).join(" | ")} |`,
		);
		let sep = "|";
		for (let i = 0; i < header.length; i++) {
			const position = positions.get(i) ?? MarkdownTablePosition.left;
			switch (position) {
				case MarkdownTablePosition.center:
					sep += `:${"-".repeat(cellSpace[i])}:`;
					break;
				case MarkdownTablePosition.right:
					sep += `${"-".repeat(cellSpace[i] + 1)}:`;
					break;
				case MarkdownTablePosition.left:
					sep += `:${"-".repeat(cellSpace[i] + 1)}`;
					break;
			}
			sep += "|";
		}
		lines.push(sep);

		for (const content of contents) {
			lines.push(
				`| ${this.toMarkdownTableCells(content, positions, cellSpace).join(" | ")} |`,
			);
		}

		return lines.join("\r\n");
	}

	private exportLayout(
		tableName: string,
		layoutElement: HTMLElement,
	): ExportData {
		const rowElements = getElementsByName<HTMLTableRowElement>(
			layoutElement,
			LayoutBlockName.LayoutRowRoot,
		);

		const markdownColumns = new Array<ReadonlyArray<string>>();
		const databaseColumns = new Array<string>();

		const primaryKeys = new Array<string>();
		const foreignKeys = new Map<
			string,
			Array<{ column: string; targetColumn: string }>
		>();
		for (const rowElement of rowElements) {
			const rowData = this.getLayoutRowData(rowElement);

			const layoutRow = this.toLayoutMarkdown(rowData);
			markdownColumns.push(layoutRow);

			const databaseCol = this.toLayoutDatabase(rowData);
			databaseColumns.push(databaseCol);

			if (rowData.isPrimary) {
				primaryKeys.push(rowData.columnName);
			}
			if (rowData.foreignTable.length && rowData.foreignColumn.length) {
				if (!foreignKeys.has(rowData.foreignTable)) {
					foreignKeys.set(
						rowData.foreignTable,
						new Array<{ column: string; targetColumn: string }>(),
					);
				}
				// biome-ignore lint/style/noNonNullAssertion: <explanation>
				const table = foreignKeys.get(rowData.foreignTable)!;
				table.push({
					column: rowData.columnName,
					targetColumn: rowData.foreignColumn,
				});
			}
		}

		const exportData = {
			markdown: this.toMarkdown(
				LayoutMarkdownHeaders,
				new Map([
					[LayoutColumn.PrimaryKey, MarkdownTablePosition.center],
					[LayoutColumn.NotNull, MarkdownTablePosition.center],
				]),
				markdownColumns,
			),
			database: "",
		} as ExportData;

		let sql = `create table [${tableName}] (\r\n`;
		sql += `\t${databaseColumns.join(",\r\n\t")}`;
		if (primaryKeys.length) {
			sql += ",\r\n";
			sql += "\tprimary key(\r\n";
			sql += `\t\t${primaryKeys.map((i) => `[${i}]`).join(",\r\n\t\t")}`;
			sql += "\r\n\t)";
		}
		if (foreignKeys.size) {
			for (const [targetTableName, column] of foreignKeys) {
				sql += ",\r\n";
				sql += `\tforeign key(${column.map((i) => `[${i.column}]`).join(", ")}) references [${targetTableName}](${column.map((i) => `[${i.targetColumn}]`).join(", ")})`;
			}
		}
		sql += "\r\n)\r\n";
		sql += ";\r\n";

		exportData.database = sql;

		return exportData;
	}

	private getIndexRowData(rowElement: HTMLTableRowElement): IndexRowData {
		const row = {
			isUnique: getElementByName<HTMLInputElement>(
				rowElement,
				IndexBlockName.UniqueKey,
			).checked,
			name: getElementByName<HTMLInputElement>(
				rowElement,
				IndexBlockName.IndexName,
			).value,
			columns: [
				...getElementsByName<HTMLInputElement>(
					rowElement,
					IndexBlockName.ColumnName,
				),
			].map((i) => i.value),
		} as IndexRowData;

		return row;
	}

	private toIndexMarkdown(row: IndexRowData): ReadonlyArray<string> {
		const result = [toCheckMark(row.isUnique)];
		result.push(row.name);
		result.push(row.columns.join(", "));
		return result;
	}

	private toIndexDatabase(tableName: string, row: IndexRowData): string {
		let sql = `--// index: idx_${tableName}_${row.name}\r\n`;
		sql += "create";
		if (row.isUnique) {
			sql += " unique";
		}
		sql += " index";
		sql += ` [idx_${tableName}_${row.name}]`;
		sql += " on";
		sql += ` [${tableName}](\r\n`;
		sql += `${row.columns.map((i) => `\t[${i}]`).join(",\r\n")}\r\n`;
		sql += ")\r\n";
		sql += ";\r\n";

		return sql;
	}

	private exportIndex(
		tableName: string,
		indexElement: HTMLElement,
	): ExportData {
		const rowElements = getElementsByName<HTMLTableRowElement>(
			indexElement,
			IndexBlockName.IndexRowRoot,
		);

		const markdownColumns = new Array<ReadonlyArray<string>>();
		const databaseStatements = new Array<string>();

		if (!rowElements.length) {
			return {
				markdown: "",
				database: "",
			} as ExportData;
		}

		for (const rowElement of rowElements) {
			const rowData = this.getIndexRowData(rowElement);

			const markdownRow = this.toIndexMarkdown(rowData);
			markdownColumns.push(markdownRow);

			const databaseIndex = this.toIndexDatabase(tableName, rowData);
			databaseStatements.push(databaseIndex);
		}
		const exportData = {
			markdown: this.toMarkdown(
				IndexMarkdownHeaders,
				new Map([[0, MarkdownTablePosition.center]]),
				markdownColumns,
			),
			database: databaseStatements.join("\r\n"),
		} as ExportData;

		return exportData;
	}

	private exportBlock(blockElement: HTMLElement) {
		const tableBlockElement = getElementByName<HTMLInputElement>(
			blockElement,
			"block-table",
		);
		const tableName = this.exportTable(tableBlockElement);

		const layoutElement = getElementByName<HTMLInputElement>(
			blockElement,
			"block-layout",
		);
		const layout = this.exportLayout(tableName, layoutElement);

		const indexElement = getElementByName<HTMLInputElement>(
			blockElement,
			"block-index",
		);
		const index = this.exportIndex(tableName, indexElement);

		return {
			table: tableName,
			layout: layout,
			index: index,
		};
	}

	public export() {
		const markdowns = Array<string>();
		const databaseTables = Array<string>();
		const databaseIndexs = Array<string>();

		const exportItems = [
			...getElementsByName(this.viewElement, "block-root"),
		].map((i) => this.exportBlock(i));

		for (const exportItem of exportItems) {
			let markdown = "\r\n___\r\n\r\n";
			markdown += `## ${exportItem.table}\r\n`;
			markdown += "\r\n";

			markdown += "### layout\r\n";
			markdown += "\r\n";
			markdown += exportItem.layout.markdown;
			markdown += "\r\n";
			markdown += "\r\n";

			markdown += "### index\r\n";
			markdown += "\r\n";
			if (exportItem.index.markdown.length) {
				markdown += exportItem.index.markdown;
			} else {
				markdown += "*NONE*";
			}
			markdown += "\r\n";
			markdown += "\r\n";

			markdowns.push(markdown);

			let databaseTable = `--// table: ${exportItem.table}\r\n`;
			databaseTable += exportItem.layout.database;

			databaseTables.push(databaseTable);

			databaseIndexs.push(exportItem.index.database);
		}

		this.defineElement.value = markdowns.join("\r\n");
		this.sqlElement.value = `${databaseTables.join("\r\n")}\r\n${databaseIndexs.join("\r\n")}`;
	}

	private copyElement(element: HTMLTextAreaElement) {
		element.focus();
		element.select();
		document.execCommand("copy");
	}
	public copyDefine() {
		this.copyElement(this.defineElement);
	}
	public copySql() {
		this.copyElement(this.sqlElement);
	}
}

// ぶんかつがだるい
const baseIds = ["main", "file"];
for (const baseId of baseIds) {
	const viewElement = document.getElementById(`view-${baseId}`);
	if (viewElement) {
		const erm = new EntityRelationManager(
			document.getElementById(`filter-${baseId}`) as HTMLDivElement,
			viewElement as HTMLDivElement,
			document.getElementById(`command-${baseId}`) as HTMLDivElement,
			document.getElementById(`define-${baseId}`) as HTMLTextAreaElement,
			document.getElementById(`sql-${baseId}`) as HTMLTextAreaElement,
		);
		erm.build();
		erm.export();
	}
}
