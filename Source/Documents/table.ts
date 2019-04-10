import { stringLiteral } from "babel-types";

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
	['text', ['System.String', 'System.Guid', 'System.Version']],
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

		throw 'はい、定義ミス';
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
				var clrValues = ClrMap.get(logicalDataElement.value)!;
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

		getElementByName<HTMLInputElement>(clonedTemplate, IndexBlockName.ColumnName).value = column;
		getElementByName<HTMLButtonElement>(clonedTemplate, IndexBlockName.DeleteColumn).addEventListener('click', ev => {
			var parent = getClosest(ev.srcElement as HTMLElement, e => e.getAttribute('name') === IndexBlockName.IndexRowColumnRoot);
			parent!.remove();
		});

		return clonedTemplate;
	}

	private createIndexRowNode(isUnique: boolean, columns: ReadonlyArray<string>): Node {
		var indexRowTemplate = document.getElementById('template-index-row') as HTMLTemplateElement;
		var clonedTemplate = document.importNode(indexRowTemplate.content, true);

		getElementByName<HTMLInputElement>(clonedTemplate, IndexBlockName.UniqueKey).checked = isUnique;

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
			var columns = indexRow[1].split(',').map(s => s.trim());
			var rowElement = this.createIndexRowNode(isUnique, columns);
			rowsElement.appendChild(rowElement)
		}

		getElementByName<HTMLButtonElement>(clonedTemplate, IndexBlockName.IndexRowAdd).addEventListener('click', ev => {
			var rowElement = this.createIndexRowNode(false, [this.getColumnNames(true)[0]]);
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
	viewElement: HTMLDivElement;
	commandElement: HTMLDivElement;
	defineElement: HTMLTextAreaElement;
	sqlElement: HTMLTextAreaElement;

	entities: Array<Entity> = [];

	constructor(viewElement: HTMLDivElement, commandElement: HTMLDivElement, defineElement: HTMLTextAreaElement, sqlElement: HTMLTextAreaElement) {
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

	private buildCommand(parentElement: HTMLDivElement) {
		var indexTemplate = document.getElementById('template-command') as HTMLTemplateElement;
		var clonedTemplate = document.importNode(indexTemplate.content, true);

		// var importElement = clonedTemplate.querySelector('[name="command-import"]') as HTMLButtonElement;
		// importElement.addEventListener('click', ev => {
		// 	this.reset();
		// 	this.build();
		// });

		var exportElement = clonedTemplate.querySelector('[name="command-export"]') as HTMLButtonElement;
		exportElement.addEventListener('click', ev => {
			this.export();
		});

		parentElement.appendChild(clonedTemplate);
	}

	private buildEntityMapping(entities: ReadonlyArray<Entity>) {
		for(var entity of entities) {
			entity.buildEntities(entities);
		}
	}

	public build() {
		this.buildCommand(this.commandElement);

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
	}

	reset() {
		this.entities = [];
		this.viewElement.textContent = '';
		this.commandElement.textContent = '';
	}

	private exportTable(tableElement: HTMLElement): string {
		var tableNameElement = getElementByName<HTMLInputElement>(tableElement, TableBlockName.TableName);
		return tableNameElement.value;
	}

	private exportLayout(tableName: string, layoutElement: HTMLElement) {
		var rowElements = getElementsByName<HTMLInputElement>(layoutElement, LayoutBlockName.LayoutRowRoot);

		var markdownColumns = new Array<string>();
		var databaseColumns = new Array<string>();

		var primaryKeys = new Array<string>();
		var foreingKeys = new Map<string, Array<{column:string, targetColumn:string}>>();
		for(var rowElement of rowElements) {
			var isPrimary = getElementByName<HTMLInputElement>(rowElement, LayoutBlockName.PrimaryKey).checked;
			var isNotNull = getElementByName<HTMLInputElement>(rowElement, LayoutBlockName.NotNull).checked;
			var columnName = getElementByName<HTMLInputElement>(rowElement, LayoutBlockName.LogicalColumnName).value;
			var logicalName = getElementByName<HTMLInputElement>(rowElement, LayoutBlockName.PhysicalColumnName).value;
			var databaseType = getElementByName<HTMLSelectElement>(rowElement, LayoutBlockName.LogicalType).value;
			var clrType = getElementByName<HTMLSelectElement>(rowElement, LayoutBlockName.ClrType).value;
			var checke = getElementByName<HTMLInputElement>(rowElement, LayoutBlockName.CheckConstraint).value;
			var comment = getElementByName<HTMLInputElement>(rowElement, LayoutBlockName.Comment).value;



		}
	}

	private exportBlock(blockElement: HTMLElement) {
		var tableBlockElement = getElementByName<HTMLInputElement>(blockElement, 'block-table');
		var tableName = this.exportTable(tableBlockElement);

		var layoutElement = getElementByName<HTMLInputElement>(blockElement, 'block-layout');
		var layouts = this.exportLayout(tableName, layoutElement);
	}

	private export() {
		var blockRoots = getElementsByName(this.viewElement, 'block-root');
		[...blockRoots].map(i => this.exportBlock(i));
	}
}

const erMain = new EntityRelationManager(
	document.getElementById('view-main') as HTMLDivElement,
	document.getElementById('command-main') as HTMLDivElement,
	document.getElementById('define-main') as HTMLTextAreaElement,
	document.getElementById('sql-main') as HTMLTextAreaElement
);
erMain.build();

