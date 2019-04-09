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
	['integer', new Set(['System.Int64'])],
	['real', new Set(['System.Decimal', 'System.Single', 'System.Double'])],
	['text', new Set(['System.String', 'System.Guid', 'System.Version'])],
	['blob', new Set(['System.Byte[]'])],
	['datetime', new Set(['System.DateTime', 'System.String'])],
	['boolean', new Set(['System.Boolean', 'System.Int64'])],
]) as ReadonlyMap<string, ReadonlySet<string>>;

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
}

enum IndexBlockName {
	ColumnName = 'c',
	UniqueKey = 'uk',
	Columns = 'columns'
}

function getElementByName(node: ParentNode, name: string): HTMLElement {
	return node.querySelector('[name="' + name + '"]') as HTMLElement;
}
function getInputElementByName(node: ParentNode, name: string): HTMLInputElement {
	return getElementByName(node, name) as HTMLInputElement;
}
function getSelectElementByName(node: ParentNode, name: string): HTMLSelectElement {
	return getElementByName(node, name) as HTMLSelectElement;
}

function getElementsByName(node: ParentNode, name: string): NodeListOf<HTMLElement> {
	return node.querySelectorAll('[name="' + name + '"]');
}
function getInputElementsByName(node: ParentNode, name: string): NodeListOf<HTMLInputElement> {
	return getElementsByName(node, name) as NodeListOf<HTMLInputElement>;
}
function getSelectElementsByName(node: ParentNode, name: string): NodeListOf<HTMLSelectElement> {
	return getElementsByName(node, name) as NodeListOf<HTMLSelectElement>;
}


function isCheckMark(value: string) {
	return value === 'o';
}

class Entity {
	private readonly tableNamePrefix = '## ';
	private blockElements: BlockElements;

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

		var tableNameElement = getInputElementByName(clonedTemplate, TableBlockName.TableName);
		tableNameElement.value = tableName;

		parentElement.appendChild(clonedTemplate);
	}

	private createLayoutRowNode(columns: ReadonlyArray<string>): Node {
		var layoutRowTemplate = document.getElementById('template-layout-row') as HTMLTemplateElement;
		var clonedTemplate = document.importNode(layoutRowTemplate.content, true);

		var primaryElement = getInputElementByName(clonedTemplate, LayoutBlockName.PrimaryKey);
		var notNullElement = getInputElementByName(clonedTemplate, LayoutBlockName.NotNull)
		primaryElement.checked = isCheckMark(columns[LayoutColumn.PrimaryKey]);
		notNullElement.checked = isCheckMark(columns[LayoutColumn.NotNull]);
		primaryElement.addEventListener('change', ev => {
			notNullElement.disabled = primaryElement.checked;
			if(primaryElement.checked) {
				notNullElement.checked =true;
			}
		});
		primaryElement.dispatchEvent(new Event('change'));

		getInputElementByName(clonedTemplate, LayoutBlockName.ForeignKey).value = columns[LayoutColumn.ForeignKey];

		getInputElementByName(clonedTemplate, LayoutBlockName.LogicalColumnName).value = columns[LayoutColumn.LogicalColumnName];
		getInputElementByName(clonedTemplate, LayoutBlockName.PhysicalColumnName).value = columns[LayoutColumn.PhysicalColumnName];

		var logicalDataElement = getSelectElementByName(clonedTemplate, LayoutBlockName.LogicalType);
		var physicalDataElement = getInputElementByName(clonedTemplate, LayoutBlockName.PhysicalType); // 一方通行イベントで使うのでキャプチャしとく。メモリは無限
		var clrDataElement = getSelectElementByName(clonedTemplate, LayoutBlockName.ClrType);
		logicalDataElement.value = columns[LayoutColumn.LogicalType];
		clrDataElement.value = columns[LayoutColumn.ClrType];
		logicalDataElement.addEventListener('change', ev => {
			var physicalValue = DatabaseTypeMap.get(logicalDataElement.value);
			physicalDataElement.value = physicalValue || 'なんかデータ変';

			// CLR に対して Pe で出来る範囲で型を限定
			var optionElements = clrDataElement.querySelectorAll('option');
			var selectedElement: HTMLOptionElement | null = null;
			var firstEnabledElement: HTMLOptionElement | null = null;
			for (var optionElement of optionElements) {
				var clrValues = ClrMap.get(logicalDataElement.value)!;
				optionElement.disabled = !clrValues.has(optionElement.value);
				if (!optionElement.disabled && !firstEnabledElement) {
					firstEnabledElement = optionElement;
				}
				if (optionElement.selected) {
					selectedElement = optionElement;
				}
			}
			if (selectedElement && selectedElement.disabled) {
				firstEnabledElement!.selected = true;
			}
		});
		logicalDataElement.dispatchEvent(new Event('change'));


		getInputElementByName(clonedTemplate, LayoutBlockName.CheckConstraint).value = columns[LayoutColumn.CheckConstraint];
		getInputElementByName(clonedTemplate, LayoutBlockName.Comment).value = columns[LayoutColumn.Comment];

		return clonedTemplate;
	}

	private buildLayout(parentElement: HTMLDivElement, layoutRows: ReadonlyArray<ReadonlyArray<string>>) {
		var layoutTemplate = document.getElementById('template-layout') as HTMLTemplateElement;
		var clonedTemplate = document.importNode(layoutTemplate.content, true);

		var rowsElement = clonedTemplate.querySelector('tbody') as HTMLElement;

		for (var layoutRow of layoutRows) {
			var rowElement = this.createLayoutRowNode(layoutRow);
			rowsElement.appendChild(rowElement)
		}

		parentElement.appendChild(clonedTemplate);
	}

	private createIndexRowColumnNode(column: string): Node {
		var indexRowColumnTemplate = document.getElementById('template-index-row-column') as HTMLTemplateElement;
		var clonedTemplate = document.importNode(indexRowColumnTemplate.content, true);

		getInputElementByName(clonedTemplate, IndexBlockName.ColumnName).value = column;

		return clonedTemplate;
	}

	private createIndexRowNode(isUnique: boolean, columns: ReadonlyArray<string>): Node {
		var indexRowTemplate = document.getElementById('template-index-row') as HTMLTemplateElement;
		var clonedTemplate = document.importNode(indexRowTemplate.content, true);

		getInputElementByName(clonedTemplate, IndexBlockName.Columns).checked = isUnique;

		var columnsElement = getElementByName(clonedTemplate, IndexBlockName.Columns);
		for (var column of columns) {
			var columnElement = this.createIndexRowColumnNode(column);
			columnsElement.appendChild(columnElement);
		}

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
		return getInputElementByName(this.blockElements.table, TableBlockName.TableName).value;
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

	public getColumnNames(): ReadonlyArray<string> {
		var columnElements = getInputElementsByName(this.blockElements.layout, LayoutBlockName.PhysicalColumnName);
		var result = new Array<string>();
		for(var columnElement of columnElements) {
			result.push(columnElement.value);
		}
		return result;
	}

	private buildForeignKeyColumns(parentElement: HTMLSelectElement, targetEntity: Entity) {
		parentElement.textContent = '';

		var columnNames = targetEntity.getColumnNames();
		for(var columnName of columnNames) {
			var optionElement = document.createElement('option');
			optionElement.value = columnName;
			optionElement.textContent = columnName;
			parentElement.appendChild(optionElement);
		}
	}

	public buildEntities(entities: ReadonlyArray<Entity>) {
		var foreignKeyRootElements = getElementsByName(this.blockElements.layout, LayoutBlockName.ForeignKeyRoot);
		for(var foreignKeyRootElement of foreignKeyRootElements) {

			var tableElement = getSelectElementByName(foreignKeyRootElement, LayoutBlockName.ForeignKeyTable);
			var columnElement = getSelectElementByName(foreignKeyRootElement, LayoutBlockName.ForeignKeyColumn);

			var targetEntities = entities
				.filter(i => i !== this)
			;

			var targetTableNames = targetEntities
				.map(i => i.getTableName())
				.sort()
			;

			this.buildForeignKeyTable(tableElement, targetTableNames);
			tableElement.addEventListener('change', ev => {
				var currentTableElement = (ev.srcElement as HTMLSelectElement);
				var currentColumnElement =  getSelectElementByName(currentTableElement.parentElement!, LayoutBlockName.ForeignKeyColumn);
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
			});

			var kfElement = getInputElementByName(foreignKeyRootElement, LayoutBlockName.ForeignKey);
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
			//TODO
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

}

const erMain = new EntityRelationManager(
	document.getElementById('view-main') as HTMLDivElement,
	document.getElementById('command-main') as HTMLDivElement,
	document.getElementById('define-main') as HTMLTextAreaElement,
	document.getElementById('sql-main') as HTMLTextAreaElement
);
erMain.build();

