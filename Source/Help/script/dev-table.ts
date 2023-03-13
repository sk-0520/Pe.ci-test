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
	isPrimary: boolean,
	isNotNull: boolean,
	foreignTable: string,
	foreignColumn: string,
	columnName: string,
	logicalName: string,
	databaseType: string,
	clrType: string,
	check: string,
	comment: string,
}

interface IndexRowData {
	isUnique: boolean,
	/** インデックス名 */
	name: string,
	columns: Array<string>;
}

interface ExportData {
	markdown: string,
	database: string,
}

enum MarkdownTablePosition {
	left,
	center,
	right
}

const DatabaseTypeMap = new Map([
	// 通常
	['integer', 'integer'],
	['real', 'real'],
	['text', 'text'],
	['blob', 'blob'],
	// 意味だけ
	['datetime', 'text'],
	['boolean', 'integer'],
]) as ReadonlyMap<string, string>;

const ClrMap = new Map([
	['integer', ['System.Int64']],
	['real', ['System.Decimal', 'System.Single', 'System.Double']],
	['text', ['System.String', 'System.Guid', 'System.Version', 'System.TimeSpan']],
	['blob', ['System.Byte[]']],
	['datetime', ['System.DateTime', 'System.String']],
	['boolean', ['System.Boolean', 'System.Int64']],
]) as ReadonlyMap<string, ReadonlyArray<string>>;

const CommonCreatedColumns = [
	'CreatedTimestamp',
	'CreatedAccount',
	'CreatedProgramName',
	'CreatedProgramVersion',
] as ReadonlyArray<string>;
const CommonUpdatedColumns = [
	'UpdatedTimestamp',
	'UpdatedAccount',
	'UpdatedProgramName',
	'UpdatedProgramVersion',
	'UpdatedCount',
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

const LayoutMarkdownHeaders = function() {
	var map = new Map<LayoutColumn, string>([
		[LayoutColumn.PrimaryKey, 'PK'],
		[LayoutColumn.NotNull, 'NN'],
		[LayoutColumn.ForeignKey, 'FK'],
		[LayoutColumn.LogicalColumnName, '論理カラム名'],
		[LayoutColumn.PhysicalColumnName, '物理カラム名'],
		[LayoutColumn.LogicalType, '論理データ型'],
		[LayoutColumn.ClrType, 'マッピング型'],
		[LayoutColumn.CheckConstraint, 'チェック制約'],
		[LayoutColumn.Comment, 'コメント'],
	]);
	return [ ...map.keys() ]
		.sort()
		.map(i => map.get(i)!)
	;
}();

const IndexMarkdownHeaders = ['UK', '名前', 'カラム(CSV)'];

enum TableBlockName {
	TableName = 'table-name',
}

enum LayoutBlockName {
	LayoutRowRoot = 'layout-row-root',
	LayoutRowAdd = 'add',
	PrimaryKey = 'pk',
	NotNull = 'nn',
	ForeignKeyRoot = 'fk-root',
	ForeignKey = 'fk',
	ForeignKeyTable = 'fk-table',
	ForeignKeyColumn = 'fk-column',
	LogicalColumnName = 'name-logical',
	PhysicalColumnName = 'name-physical',
	LogicalType = 'data-logical',
	PhysicalType = 'data-physical',
	ClrType = 'data-clr',
	CheckConstraint = 'check',
	Comment = 'comment',
	Delete = 'delete',
}

enum IndexBlockName {
	IndexRowRoot = 'index-row-root',
	IndexRowAdd = 'add',
	ColumnAdd = 'add-col',
	IndexRowColumnRoot = 'index-row-column-root',
	ColumnName = 'c',
	UniqueKey = 'uk',
	IndexName = 'name',
	Columns = 'columns',
	Column = 'column',
	DeleteColumn = 'delete-col',
	DeleteIndex = 'delete',
}

function getElementByName<THTMLElement extends HTMLElement>(node: ParentNode, name: string): THTMLElement {
	return node.querySelector('[name="' + name + '"]') as THTMLElement;
}

function getElementsByName<THTMLElement extends HTMLElement>(node: ParentNode, name: string): NodeListOf<THTMLElement> {
	return node.querySelectorAll('[name="' + name + '"]');
}

function getClosest(element: HTMLElement, func: (target: HTMLElement) => boolean): HTMLElement | null {
	while(element.parentElement) {
		if(func(element.parentElement)) {
			return element.parentElement;
		}

		element = element.parentElement;
	}
	return null;
}

function isCheckMark(value: string) {
	return value === 'o';
}
function toCheckMark(value: boolean) {
	return value ? 'o': '';
}

function countSingleChar(s: string): number {
	if(!s || !s.length) {
		return 0;
	}
	var chars = s.match(/[\uD800-\uDBFF][\uDC00-\uDFFF]|[^\uD800-\uDFFF]/g) || [];
	var length = 0;
	for(var c of chars) {
		if(c.length == 1) {
			if(!c.match(/[^\x01-\x7E]/) || !c.match(/[^\uFF65-\uFF9F]/)) {
				length += 1;
			}  else {
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
	private readonly tableNamePrefix = '## ';
	private blockElements: BlockElements;
	private entities: ReadonlyArray<Entity> = [];

	constructor(blockElements: BlockElements) {
		this.blockElements = blockElements;
	}

	private getIndex(lines: ReadonlyArray<string>) {
		var regLayout = /^###\s*layout\s*/;
		var regIndex = /^###\s*index\s*/;

		enum State {
			Table,
			Layout,
			Index,
		};

		var state = State.Table;

		var lineIndex = {
			table: -1,
			layout: {
				head: -1,
				tail: -1
			},
			index: {
				head: -1,
				tail: -1
			}
		};

		for (var i = 0; i < lines.length; i++) {
			var line = lines[i];

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
					throw 'こねーよ！';
			}
		}

		throw 'はい、定義ミス:' + JSON.stringify(lines);
	}

	private trimMarkdownTable(lines: ReadonlyArray<string>): ReadonlyArray<string> {
		return lines
			.map(s => s.trim())
			.filter(s => s.startsWith('|') && s.endsWith('|'))
			;
	}

	private trimDefine(rawDefine: EntityDefine): EntityDefine {
		var result = {
			table: rawDefine.table.substr(this.tableNamePrefix.length),
			layout: this.trimMarkdownTable(rawDefine.layout),
			index: this.trimMarkdownTable(rawDefine.index),
		} as EntityDefine;

		return result;
	}

	private convertRowLines(markdownTableLines: ReadonlyArray<string>): ReadonlyArray<ReadonlyArray<string>> {
		var rows = markdownTableLines
			.map(i => i.replace(/(^\|)|(|$)/, ''))
			.map(i => i.split('|').map(s => s.trim()))
			;
		if (2 < rows.length) {
			return rows.slice(2);
		}

		return [];
	}

	private buildTable(parentElement: HTMLDivElement, tableName: string) {
		var tableTemplate = document.getElementById('template-table') as HTMLTemplateElement;
		var clonedTemplate = document.importNode(tableTemplate.content, true);

		var tableNameElement = getElementByName<HTMLInputElement>(clonedTemplate, TableBlockName.TableName);
		tableNameElement.value = tableName;

		parentElement.appendChild(clonedTemplate);
	}

	private createLayoutRowNode(columns: ReadonlyArray<string>) {
		var layoutRowTemplate = document.getElementById('template-layout-row') as HTMLTemplateElement;
		var clonedTemplate = document.importNode(layoutRowTemplate.content, true);

		var primaryElement = getElementByName<HTMLInputElement>(clonedTemplate, LayoutBlockName.PrimaryKey);
		var notNullElement = getElementByName<HTMLInputElement>(clonedTemplate, LayoutBlockName.NotNull)
		primaryElement.checked = isCheckMark(columns[LayoutColumn.PrimaryKey]);
		notNullElement.checked = isCheckMark(columns[LayoutColumn.NotNull]);
		primaryElement.addEventListener('change', ev => {
			notNullElement.disabled = primaryElement.checked;
			if(primaryElement.checked) {
				notNullElement.checked =true;
			}
		});
		primaryElement.dispatchEvent(new Event('change'));

		getElementByName<HTMLInputElement>(clonedTemplate, LayoutBlockName.ForeignKey).value = columns[LayoutColumn.ForeignKey];

		getElementByName<HTMLInputElement>(clonedTemplate, LayoutBlockName.LogicalColumnName).value = columns[LayoutColumn.LogicalColumnName];
		getElementByName<HTMLInputElement>(clonedTemplate, LayoutBlockName.PhysicalColumnName).value = columns[LayoutColumn.PhysicalColumnName];

		var logicalDataElement = getElementByName<HTMLSelectElement>(clonedTemplate, LayoutBlockName.LogicalType);
		var physicalDataElement = getElementByName<HTMLInputElement>(clonedTemplate, LayoutBlockName.PhysicalType); // 一方通行イベントで使うのでキャプチャしとく。メモリは無限
		var clrDataElement = getElementByName<HTMLSelectElement>(clonedTemplate, LayoutBlockName.ClrType);
		logicalDataElement.value = columns[LayoutColumn.LogicalType];
		clrDataElement.value = columns[LayoutColumn.ClrType];
		logicalDataElement.addEventListener('change', ev => {

			var physicalValue = DatabaseTypeMap.get(logicalDataElement.value);
			physicalDataElement.value = physicalValue || 'なんかデータ変';

			// CLR に対して Pe で出来る範囲で型を限定
			var optionElements = clrDataElement.querySelectorAll('option');
			var selectedElement: HTMLOptionElement | null = null;
			var defaultElement: HTMLOptionElement | null = null;
			for (var optionElement of optionElements) {
				var clrValues = ClrMap.get(logicalDataElement.value);
				if(!clrValues) {
					logicalDataElement.parentElement?.parentElement?.parentElement?.classList.add('error-parent');
					logicalDataElement.parentElement?.parentElement?.classList.add('error-row');
					throw "clrValues が取得できない, たぶん 論理型 が不明: " + logicalDataElement.value + ":" + physicalValue;
				}
				optionElement.disabled = !clrValues.some(i => i === optionElement.value);
				if (!optionElement.disabled && !defaultElement) {
					if(clrValues[0] === optionElement.value) {
						defaultElement = optionElement;
					}
				}
				if (optionElement.selected) {
					selectedElement = optionElement;
				}
			}
			if (selectedElement && selectedElement.disabled) {
				defaultElement!.selected = true;
			}
		});
		logicalDataElement.dispatchEvent(new Event('change'));


		getElementByName<HTMLInputElement>(clonedTemplate, LayoutBlockName.CheckConstraint).value = columns[LayoutColumn.CheckConstraint];
		getElementByName<HTMLInputElement>(clonedTemplate, LayoutBlockName.Comment).value = columns[LayoutColumn.Comment];

		getElementByName<HTMLButtonElement>(clonedTemplate, LayoutBlockName.Delete).addEventListener('click', ev => {
			var element = ev.srcElement as HTMLElement;
			while(element.getAttribute('name') !== LayoutBlockName.LayoutRowRoot) {
				element = element.parentElement as HTMLElement;
			}
			element.remove();
		});

		return clonedTemplate;
	}

	private createEmptyLayout(): ReadonlyArray<string> {
		var defaultDatabaseType = 'integer';

		var map = new Map<LayoutColumn, string>([
			[LayoutColumn.PrimaryKey, ''],
			[LayoutColumn.NotNull, ''],
			[LayoutColumn.ForeignKey, ''],
			[LayoutColumn.LogicalColumnName, ''],
			[LayoutColumn.PhysicalColumnName, ''],
			[LayoutColumn.LogicalType, defaultDatabaseType],
			[LayoutColumn.ClrType, ClrMap.get(defaultDatabaseType)![0]],
			[LayoutColumn.CheckConstraint, ''],
			[LayoutColumn.Comment, ''],
		]);

		return [ ...map.keys() ]
			.sort()
			.map(i => map.get(i)!)
		;
	}

	private buildLayout(parentElement: HTMLDivElement, layoutRows: ReadonlyArray<ReadonlyArray<string>>) {
		var layoutTemplate = document.getElementById('template-layout') as HTMLTemplateElement;
		var clonedTemplate = document.importNode(layoutTemplate.content, true);

		var rowsElement = clonedTemplate.querySelector('tbody') as HTMLElement;

		for (var layoutRow of layoutRows) {
			var rowElement = this.createLayoutRowNode(layoutRow);
			rowsElement.appendChild(rowElement)
		}

		getElementByName<HTMLButtonElement>(clonedTemplate, LayoutBlockName.LayoutRowAdd).addEventListener('click', ev => {
			var element = ev.srcElement as HTMLElement;
			while(element.tagName !== 'TABLE') {
				element = element.parentElement as HTMLElement;
			}

			var emptyLayout = this.createEmptyLayout();
			var rowElement = this.createLayoutRowNode(emptyLayout);

			rowsElement.appendChild(rowElement);
			var newRowElement = rowsElement.lastElementChild as HTMLElement;
			var tableElement = getElementByName<HTMLSelectElement>(newRowElement, LayoutBlockName.ForeignKeyTable);
			var targetEntities = this.filterMyself(this.entities);
			var targetTableNames = this.getTableNamesFromEntities(targetEntities);
			this.buildForeignKeyTable(tableElement, targetTableNames);
			tableElement.addEventListener('change', ev => this.changedTableElement(ev, targetEntities));
		});


		parentElement.appendChild(clonedTemplate);
	}

	private createIndexRowColumnNode(column: string) {
		var indexRowColumnTemplate = document.getElementById('template-index-row-column') as HTMLTemplateElement;
		var clonedTemplate = document.importNode(indexRowColumnTemplate.content, true);

		const columnElement = getElementByName<HTMLInputElement>(clonedTemplate, IndexBlockName.ColumnName);
		columnElement.value = column;

		const columnOptionElement = getElementByName<HTMLOptionElement>(clonedTemplate, IndexBlockName.Column);
		columnOptionElement.addEventListener('change', ev => {
			columnElement.value = columnOptionElement.value;
		});

		getElementByName<HTMLButtonElement>(clonedTemplate, IndexBlockName.DeleteColumn).addEventListener('click', ev => {
			var parent = getClosest(ev.srcElement as HTMLElement, e => e.getAttribute('name') === IndexBlockName.IndexRowColumnRoot);
			parent!.remove();
		});

		return clonedTemplate;
	}

	private createIndexRowNode(isUnique: boolean, indexName: string, columns: ReadonlyArray<string>): Node {
		var indexRowTemplate = document.getElementById('template-index-row') as HTMLTemplateElement;
		var clonedTemplate = document.importNode(indexRowTemplate.content, true);

		getElementByName<HTMLInputElement>(clonedTemplate, IndexBlockName.UniqueKey).checked = isUnique;

		getElementByName<HTMLInputElement>(clonedTemplate, IndexBlockName.IndexName).value = indexName;

		var columnsElement = getElementByName(clonedTemplate, IndexBlockName.Columns);
		for (var column of columns) {
			var columnElement = this.createIndexRowColumnNode(column);
			columnsElement.appendChild(columnElement);
		}

		getElementByName<HTMLButtonElement>(clonedTemplate, IndexBlockName.ColumnAdd).addEventListener('click', ev => {
			var columnElement = this.createIndexRowColumnNode(this.getColumnNames(true)[0]);
			columnsElement.appendChild(columnElement);

			var columnNames = this.getColumnNames(true);
			var targetElement = getElementByName<HTMLSelectElement>(columnsElement.lastElementChild!, IndexBlockName.Column)
			this.setIndexColumnNames(targetElement, columnNames);
		});

		getElementByName<HTMLButtonElement>(clonedTemplate, IndexBlockName.DeleteIndex).addEventListener('click', ev => {
			var parent = getClosest(ev.srcElement as HTMLElement, e => e.getAttribute('name') === IndexBlockName.IndexRowRoot);
			parent!.remove();
		});

		return clonedTemplate;
	}

	private buildIndex(parentElement: HTMLDivElement, indexRows: ReadonlyArray<ReadonlyArray<string>>) {
		var indexTemplate = document.getElementById('template-index') as HTMLTemplateElement;
		var clonedTemplate = document.importNode(indexTemplate.content, true);

		var rowsElement = clonedTemplate.querySelector('tbody') as HTMLElement;

		for (var indexRow of indexRows) {
			var isUnique = isCheckMark(indexRow[0]);
			var indexName = indexRow[1].trim();
			var columns = indexRow[2].split(',').map(s => s.trim());
			var rowElement = this.createIndexRowNode(isUnique, indexName, columns);
			rowsElement.appendChild(rowElement)
		}

		getElementByName<HTMLButtonElement>(clonedTemplate, IndexBlockName.IndexRowAdd).addEventListener('click', ev => {
			var rowElement = this.createIndexRowNode(false, '', [this.getColumnNames(true)[0]]);
			rowsElement.appendChild(rowElement);

			var columnElement = getElementByName<HTMLSelectElement>(rowsElement.lastElementChild!, IndexBlockName.Column);
			var columnNames = this.getColumnNames(true);
			this.setIndexColumnNames(columnElement, columnNames);
		});

		parentElement.appendChild(clonedTemplate);
	}

	public build(rawDefine: string) {
		var lines = rawDefine.split(/\r?\n/mg);
		var index = this.getIndex(lines);

		var define = this.trimDefine({
			table: lines[index.table],
			layout: lines.slice(index.layout.head, index.layout.tail),
			index: lines.slice(index.index.head, index.index.tail)
		});

		this.buildTable(this.blockElements.table, define.table);
		this.buildLayout(this.blockElements.layout, this.convertRowLines(define.layout));
		this.buildIndex(this.blockElements.index, this.convertRowLines(define.index));
	}

	public getTableName(): string {
		return getElementByName<HTMLInputElement>(this.blockElements.table, TableBlockName.TableName).value;
	}

	private buildForeignKeyTable(parentElement: HTMLSelectElement, tableNames: ReadonlyArray<string>) {
		parentElement.appendChild(document.createElement('option'));
		for(var tableName of tableNames) {
			var optionElement = document.createElement('option') as HTMLOptionElement;
			optionElement.value = tableName;
			optionElement.textContent = tableName;

			parentElement.appendChild(optionElement);
		}
	}

	public getColumnNames(ignoreCommonColumn: boolean): ReadonlyArray<string> {
		var isIgnoreColumn = (s:string) => {
			if(CommonCreatedColumns.some(i => i === s)) {
				return true;
			}
			if(CommonUpdatedColumns.some(i => i === s)) {
				return true;
			}

			return false;
		}
		var columnElements = getElementsByName<HTMLInputElement>(this.blockElements.layout, LayoutBlockName.PhysicalColumnName);
		var result = new Array<string>();
		for(var columnElement of columnElements) {
			if(ignoreCommonColumn && isIgnoreColumn(columnElement.value)) {
					continue;
			}
			result.push(columnElement.value);
		}
		return result;
	}

	private buildForeignKeyColumns(parentElement: HTMLSelectElement, targetEntity: Entity) {
		parentElement.textContent = '';

		var columnNames = targetEntity.getColumnNames(true);
		for(var columnName of columnNames) {
			var optionElement = document.createElement('option');
			optionElement.value = columnName;
			optionElement.textContent = columnName;
			parentElement.appendChild(optionElement);
		}
	}

	private filterMyself(entities: ReadonlyArray<Entity>): ReadonlyArray<Entity> {
		var targetEntities = entities
			.filter(i => i !== this)
		;

		return targetEntities;
	}

	private getTableNamesFromEntities(entities: ReadonlyArray<Entity>) {
		return entities
			.map(i => i.getTableName())
			.sort()
		;
	}

	private changedTableElement(ev: Event, targetEntities: ReadonlyArray<Entity>) {
		var currentTableElement = (ev.srcElement as HTMLSelectElement);
		var currentColumnElement =  getElementByName<HTMLSelectElement>(currentTableElement.parentElement!, LayoutBlockName.ForeignKeyColumn);
		var targetEntity = targetEntities
			.find(i => i.getTableName() == currentTableElement.value)
		;

		if(targetEntity) {
			currentColumnElement.disabled = false;
			this.buildForeignKeyColumns(currentColumnElement, targetEntity);
		} else {
			currentColumnElement.disabled = true;
			currentColumnElement.textContent = '';
		}
	}

	private buildEntityForeignKey(entities: ReadonlyArray<Entity>){
		var foreignKeyRootElements = getElementsByName(this.blockElements.layout, LayoutBlockName.ForeignKeyRoot);
		for(var foreignKeyRootElement of foreignKeyRootElements) {

			var tableElement = getElementByName<HTMLSelectElement>(foreignKeyRootElement, LayoutBlockName.ForeignKeyTable);
			var columnElement = getElementByName<HTMLSelectElement>(foreignKeyRootElement, LayoutBlockName.ForeignKeyColumn);

			var targetEntities = this.filterMyself(entities);
			var targetTableNames = this.getTableNamesFromEntities(targetEntities);

			this.buildForeignKeyTable(tableElement, targetTableNames);
			tableElement.addEventListener('change', ev => this.changedTableElement(ev, targetEntities));

			var kfElement = getElementByName<HTMLInputElement>(foreignKeyRootElement, LayoutBlockName.ForeignKey);
			if(kfElement.value) {
				var v = kfElement.value.split('.');
				var kfPair = {
					table: v[0].trim(),
					column: v[1].trim(),
				};
				tableElement.value = kfPair.table;
				tableElement.dispatchEvent(new Event('change'));
				columnElement.value = kfPair.column;
			} else {
				tableElement.dispatchEvent(new Event('change'));
			}
			kfElement.value = '';
		}
	}

	private setIndexColumnNames(parentElement: HTMLSelectElement, columnNames: ReadonlyArray<string>) {
		for(var columnName of columnNames) {
			var optionElement = document.createElement('option');
			optionElement.value = columnName;
			optionElement.textContent = columnName;

			parentElement.appendChild(optionElement);
		}
}

	private buildEntityIndex() {
		var columnNames = this.getColumnNames(true);
		var indexRowRootElements = getElementsByName(this.blockElements.index, IndexBlockName.IndexRowRoot);
		for(var indexRowRootElement of indexRowRootElements) {
			var columnsElements = getElementsByName<HTMLSelectElement>(indexRowRootElement, IndexBlockName.Column);
			for(var columnsElement of columnsElements) {
				/*
				for(var columnName of columnNames) {
					var optionElement = document.createElement('option');
					optionElement.value = columnName;
					optionElement.textContent = columnName;

					columnsElement.appendChild(optionElement);
				}
				*/
				this.setIndexColumnNames(columnsElement, columnNames);
				columnsElement.value = getElementByName<HTMLInputElement>(columnsElement.parentElement!, IndexBlockName.ColumnName).value;
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

	constructor(filterElemenet: HTMLDivElement, viewElement: HTMLDivElement, commandElement: HTMLDivElement, defineElement: HTMLTextAreaElement, sqlElement: HTMLTextAreaElement) {
		this.filterElemenet = filterElemenet;
		this.viewElement = viewElement;
		this.commandElement = commandElement;
		this.defineElement = defineElement;
		this.sqlElement = sqlElement;
	}

	private createBlockElements(): BlockElements {
		var blockTemplate = document.getElementById('template-block') as HTMLTemplateElement;
		var clonedTemplate = document.importNode(blockTemplate.content, true);
		var block = {
			root: clonedTemplate.querySelector('[name="block-root"]'),
			table: clonedTemplate.querySelector('[name="block-table"]'),
			layout: clonedTemplate.querySelector('[name="block-layout"]'),
			index: clonedTemplate.querySelector('[name="block-index"]'),
		} as BlockElements;

		return block;
	}

	private buildFilter(parentElement: HTMLDivElement) {
		var filterTemplate = document.getElementById('template-filter') as HTMLTemplateElement;
		var clonedTemplate = document.importNode(filterTemplate.content, true);

		var tableFilterElement = getElementByName<HTMLInputElement>(clonedTemplate, 'table-filter');

		tableFilterElement.addEventListener('input', ev => {
			const elements = getElementsByName<HTMLDivElement>(this.viewElement, 'block-root');
			var inputValue = tableFilterElement.value.trim().toLowerCase();
			var reg = function() {
				try {
					if(inputValue.indexOf('/') === 0) {
						return new RegExp(inputValue.substr(1));
					}
					var headPattern = "^\\s*" + inputValue.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&');
					return new RegExp(headPattern);
				} catch {
					return RegExp('');
				}
			}();

			for(const element of elements) {
				const tableName = getElementByName<HTMLInputElement>(element, TableBlockName.TableName).value.trim().toLowerCase();
				if(reg.test(tableName)) {
					element.style.display = 'block';
				} else {
					element.style.display = 'none';
				}
			}
		});

		parentElement.appendChild(clonedTemplate);
	}

	private buildCommand(parentElement: HTMLDivElement) {
		var commandTemplate = document.getElementById('template-command') as HTMLTemplateElement;
		var clonedTemplate = document.importNode(commandTemplate.content, true);

		var exportElement = clonedTemplate.querySelector('[name="command-import"]') as HTMLButtonElement;
		exportElement.addEventListener('click', ev => {
			this.viewElement.textContent = '';
			this.buildCore(true);
			this.export();
		});

		var exportElement = clonedTemplate.querySelector('[name="command-export"]') as HTMLButtonElement;
		exportElement.addEventListener('click', ev => {
			this.export();
		});

		var copyDefineElement = clonedTemplate.querySelector('[name="command-copy-define"]') as HTMLButtonElement;
		copyDefineElement.addEventListener('click', ev => {
			this.copyDefine();
		});

		var copyDefineElement = clonedTemplate.querySelector('[name="command-copy-sql"]') as HTMLButtonElement;
		copyDefineElement.addEventListener('click', ev => {
			this.copySql();
		});

		parentElement.appendChild(clonedTemplate);
	}

	private buildEntityMapping(entities: ReadonlyArray<Entity>) {
		for(var entity of entities) {
			entity.buildEntities(entities);
		}
	}

	private buildCore(rebuild: boolean) {
		if(rebuild) {
			const filterElements = document.getElementsByClassName('filter');
			for(const filterElement of filterElements) {
				filterElement.textContent = '';
			}
		} else {
			this.buildCommand(this.commandElement);
		}

		var defines = this.defineElement.value.split('___');
		defines.shift();

		for (var define of defines) {
			var blockElements = this.createBlockElements();
			var entity = new Entity(blockElements);
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
		this.filterElemenet.textContent = '';
		this.entities = [];
		this.viewElement.textContent = '';
		this.commandElement.textContent = '';
	}

	private exportTable(tableElement: HTMLElement): string {
		var tableNameElement = getElementByName<HTMLInputElement>(tableElement, TableBlockName.TableName);
		return tableNameElement.value;
	}

	private getLayoutRowData(rowElement: HTMLTableRowElement): LayoutRowData {
		var data = {
			isPrimary: getElementByName<HTMLInputElement>(rowElement, LayoutBlockName.PrimaryKey).checked,
			isNotNull: getElementByName<HTMLInputElement>(rowElement, LayoutBlockName.NotNull).checked,
			foreignTable: '',
			foreignColumn: '',
			columnName: getElementByName<HTMLInputElement>(rowElement, LayoutBlockName.PhysicalColumnName).value,
			logicalName: getElementByName<HTMLInputElement>(rowElement, LayoutBlockName.LogicalColumnName).value,
			databaseType: getElementByName<HTMLSelectElement>(rowElement, LayoutBlockName.LogicalType).value,
			clrType: getElementByName<HTMLSelectElement>(rowElement, LayoutBlockName.ClrType).value,
			check: getElementByName<HTMLInputElement>(rowElement, LayoutBlockName.CheckConstraint).value,
			comment: getElementByName<HTMLInputElement>(rowElement, LayoutBlockName.Comment).value,
		} as LayoutRowData;

		var ft = getElementByName<HTMLInputElement>(rowElement, LayoutBlockName.ForeignKeyTable).value
		var fc = getElementByName<HTMLInputElement>(rowElement, LayoutBlockName.ForeignKeyColumn).value
		if(ft.length && fc.length) {
			data.foreignTable = ft;
			data.foreignColumn = fc;
		}

		return data;
	}

	private toLayoutMarkdown(row: LayoutRowData): ReadonlyArray<string> {
		var map = new Map<LayoutColumn, string>([
			[LayoutColumn.PrimaryKey, toCheckMark(row.isPrimary)],
			[LayoutColumn.NotNull, toCheckMark(row.isNotNull)],
			[LayoutColumn.ForeignKey, row.foreignTable.length && row.foreignColumn.length ? `${row.foreignTable}.${row.foreignColumn}`: ''],
			[LayoutColumn.LogicalColumnName, row.logicalName],
			[LayoutColumn.PhysicalColumnName, row.columnName],
			[LayoutColumn.LogicalType, row.databaseType],
			[LayoutColumn.ClrType, row.clrType],
			[LayoutColumn.CheckConstraint, row.check],
			[LayoutColumn.Comment, row.comment],
		]);
		return [ ...map.keys() ]
			.sort()
			.map(i => map.get(i)!)
		;
	}

	private toLayoutDatabase(row: LayoutRowData): string {
		var sql = `[${row.columnName}] ${row.databaseType}`;
		if(row.isNotNull) {
			sql += " not null";
		}
		if(row.check.length) {
			sql += ` check( ${row.check} )`
		}

		if(row.logicalName.length || row.comment.length) {
			sql += ` /* ${row.logicalName}`;
			if(row.logicalName.length) {
				sql += ' ';
			}
			sql += row.comment;
			sql += " */";
		}

		return sql;
	}

	private getMarkdownCellSpace(data:ReadonlyArray<ReadonlyArray<string>>): ReadonlyArray<number> {

		var result = new Array<number>(data[0].length).fill(0);

		for(var col = 0; col < data[0].length; col++) {
			for(var row of data) {
				var colLength = countSingleChar(row[col]);
				result[col] = Math.max(result[col], colLength);
			}
		}

		return result;
	}

	private toMarkdownCell(value: string, length: number, position: MarkdownTablePosition) {
		var valueLength = countSingleChar(value);

		if(valueLength == length) {
			return value;
		}
		switch(position) {
			case MarkdownTablePosition.left:
				return value + ' '.repeat(length - valueLength);

			case MarkdownTablePosition.right:
				return ' '.repeat(length - valueLength) + value;

			case MarkdownTablePosition.center:
			return ' '.repeat((length - valueLength) / 2) + value + ' '.repeat(((length - valueLength) / 2) + ((length - valueLength) % 2));
		}
	}

	private toMarkdownTableCells(cells: ReadonlyArray<string>, positions: Map<number, MarkdownTablePosition>, space: ReadonlyArray<number>):ReadonlyArray<string> {
		var result = new Array<string>(cells.length);
		for(var i = 0; i < cells.length; i++) {
			var position = positions.get(i) || MarkdownTablePosition.left;
			var value = this.toMarkdownCell(cells[i], space[i], position);
			result[i] = value;
		}

		return result;
	}

	private toMarkdown(header: ReadonlyArray<string>, positions: Map<number, MarkdownTablePosition>, contents: ReadonlyArray<ReadonlyArray<string>>) {
		var cellSpace = this.getMarkdownCellSpace([header].concat(contents));

		var lines = new Array<string>();
		lines.push('| ' + this.toMarkdownTableCells(header, positions, cellSpace).join(' | ') + ' |');
		var sep = '|'
		for(var i = 0; i < header.length; i++) {
			var position = positions.get(i) ||  MarkdownTablePosition.left;
			switch(position) {
				case MarkdownTablePosition.center:
					sep += ':' + '-'.repeat(cellSpace[i]) + ':';
					break;
				case MarkdownTablePosition.right:
					sep += '-'.repeat(cellSpace[i] + 1) + ':';
					break;
				case MarkdownTablePosition.left:
					sep += ':' + '-'.repeat(cellSpace[i] + 1);
					break;
			}
			sep += '|';
		}
		lines.push(sep);

		for(var content of contents) {
			lines.push('| ' + this.toMarkdownTableCells(content, positions, cellSpace).join(' | ') + ' |');
		}

		return lines.join("\r\n");
	}

	private exportLayout(tableName: string, layoutElement: HTMLElement):ExportData {
		var rowElements = getElementsByName<HTMLTableRowElement>(layoutElement, LayoutBlockName.LayoutRowRoot);

		var markdownColumns = new Array<ReadonlyArray<string>>();
		var databaseColumns = new Array<string>();

		var primaryKeys = new Array<string>();
		var foreingKeys = new Map<string, Array<{column:string, targetColumn:string}>>();
		for(var rowElement of rowElements) {
			var rowData = this.getLayoutRowData(rowElement);

			var layoutRow = this.toLayoutMarkdown(rowData);
			markdownColumns.push(layoutRow);

			var databaseCol = this.toLayoutDatabase(rowData);
			databaseColumns.push(databaseCol);

			if(rowData.isPrimary) {
				primaryKeys.push(rowData.columnName);
			}
			if(rowData.foreignTable.length && rowData.foreignColumn.length) {
				if(!foreingKeys.has(rowData.foreignTable)) {
					foreingKeys.set(rowData.foreignTable, new Array<{column:string, targetColumn:string}>());
				}
				var table = foreingKeys.get(rowData.foreignTable)!;
				table.push({column: rowData.columnName, targetColumn: rowData.foreignColumn});
			}
		}

		var exportData = {
			markdown: this.toMarkdown(
				LayoutMarkdownHeaders,
				new Map([
					[LayoutColumn.PrimaryKey, MarkdownTablePosition.center],
					[LayoutColumn.NotNull, MarkdownTablePosition.center],
				]),
				markdownColumns
			),
			database: '',
		} as ExportData;

		var sql = `create table [${tableName}] (\r\n`;
		sql += "\t" + databaseColumns.join(",\r\n\t");
		if(primaryKeys.length) {
			sql += ",\r\n";
			sql += "\tprimary key(\r\n"
			sql += "\t\t" + primaryKeys.map(i => `[${i}]`).join(",\r\n\t\t")
			sql += "\r\n\t)";
		};
		if(foreingKeys.size) {
			for(var [targetTableName, column] of foreingKeys) {
				sql += ",\r\n";
				sql += `\tforeign key(${column.map(i => `[${i.column}]`).join(', ')}) references [${targetTableName}](${column.map(i => `[${i.targetColumn}]`).join(', ')})`
			}
		}
		sql += "\r\n)\r\n";
		sql += ";\r\n";

		exportData.database = sql;

		return exportData;
	}

	private getIndexRowData(rowElement: HTMLTableRowElement):IndexRowData {
		var row = {
			isUnique: getElementByName<HTMLInputElement>(rowElement, IndexBlockName.UniqueKey).checked,
			name: getElementByName<HTMLInputElement>(rowElement, IndexBlockName.IndexName).value,
			columns: [ ...getElementsByName<HTMLInputElement>(rowElement, IndexBlockName.ColumnName)]
				.map(i => i.value)
			,
		} as IndexRowData;

		return row;
	}

	private toIndexMarkdown(row:IndexRowData): ReadonlyArray<string> {
		var result = [toCheckMark(row.isUnique)];
		result.push(row.name);
		result.push(row.columns.join(', '));
		return result;
	}

	private toIndexDatabase(tableName: string, row:IndexRowData): string {
		var sql = `--// index: idx_${tableName}_${row.name}\r\n`;
		sql += "create";
		if(row.isUnique) {
			sql += " unique";
		}
		sql += " index";
		sql += ` [idx_${tableName}_${row.name}]`;
		sql += " on";
		sql += ` [${tableName}](\r\n`;
		sql += row.columns.map(i => `\t[${i}]`).join(",\r\n") + "\r\n";
		sql += ")\r\n";
		sql += ";\r\n";

		return sql;
	}

	private exportIndex(tableName: string, indexElement: HTMLElement):ExportData {
		var rowElements = getElementsByName<HTMLTableRowElement>(indexElement, IndexBlockName.IndexRowRoot);

		var markdownColumns = new Array<ReadonlyArray<string>>();
		var databaseStatements = new Array<string>();

		if(!rowElements.length) {
			return {
				markdown: '',
				database: '',
			} as ExportData;
		}

		for(var rowElement of rowElements) {
			var rowData = this.getIndexRowData(rowElement);

			var markdownRow = this.toIndexMarkdown(rowData);
			markdownColumns.push(markdownRow);

			var databaseIndex = this.toIndexDatabase(tableName, rowData);
			databaseStatements.push(databaseIndex);
		}
		var exportData = {
			markdown: this.toMarkdown(
				IndexMarkdownHeaders,
				new Map([
					[0, MarkdownTablePosition.center]
				]),
				markdownColumns
			),
			database: databaseStatements.join("\r\n"),
		} as ExportData;

		return exportData;
	}

	private exportBlock(blockElement: HTMLElement) {
		var tableBlockElement = getElementByName<HTMLInputElement>(blockElement, 'block-table');
		var tableName = this.exportTable(tableBlockElement);

		var layoutElement = getElementByName<HTMLInputElement>(blockElement, 'block-layout');
		var layout = this.exportLayout(tableName, layoutElement);

		var indexElement = getElementByName<HTMLInputElement>(blockElement, 'block-index');
		var index = this.exportIndex(tableName, indexElement);

		return {
			table: tableName,
			layout: layout,
			index: index,
		}
	}

	public export() {
		var markdowns = Array<string>();
		var databaseTables = Array<string>();
		var databaseIndexs = Array<string>();

		var exportItems = [...getElementsByName(this.viewElement, 'block-root')]
			.map(i => this.exportBlock(i))
		;

		for(var exportItem of exportItems) {
			var markdown = "\r\n___\r\n\r\n";
			markdown += `## ${exportItem.table}\r\n`;
			markdown += "\r\n";

			markdown += "### layout\r\n";
			markdown += "\r\n";
			markdown += exportItem.layout.markdown;
			markdown += "\r\n";
			markdown += "\r\n";

			markdown += "### index\r\n";
			markdown += "\r\n";
			if(exportItem.index.markdown.length) {
				markdown += exportItem.index.markdown;
			} else {
				markdown += "*NONE*";
			}
			markdown += "\r\n";
			markdown += "\r\n";

			markdowns.push(markdown);

			var databaseTable = `--// table: ${exportItem.table}\r\n`;
			databaseTable += exportItem.layout.database;

			databaseTables.push(databaseTable);

			databaseIndexs.push(exportItem.index.database);
		}

		this.defineElement.value = markdowns.join("\r\n");
		this.sqlElement.value = databaseTables.join("\r\n") + "\r\n" + databaseIndexs.join("\r\n");

	}

	private copyElement(element: HTMLTextAreaElement) {
		element.focus();
		element.select();
		document.execCommand('copy');
	}
	public copyDefine() {
		this.copyElement(this.defineElement);
	}
	public copySql() {
		this.copyElement(this.sqlElement);
	}
}

// ぶんかつがだるい
const baseIds = ['main', 'file'];
for(var baseId of baseIds) {
	var viewElement = document.getElementById(`view-${baseId}`);
	if(viewElement) {
		var erm = new EntityRelationManager(
			document.getElementById(`filter-${baseId}`) as HTMLDivElement,
			viewElement as HTMLDivElement,
			document.getElementById(`command-${baseId}`) as HTMLDivElement,
			document.getElementById(`define-${baseId}`) as HTMLTextAreaElement,
			document.getElementById(`sql-${baseId}`) as HTMLTextAreaElement
		);
		erm.build();
		erm.export();
	}
}
