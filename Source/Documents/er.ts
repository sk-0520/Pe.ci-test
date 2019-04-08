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

const DatabaseTypeMap = new Map<string, string>([
    // 通常
    ['integer', 'integer'],
    ['real', 'real'],
    ['text', 'text'],
    ['blob', 'blob'],
    // 意味だけ
    ['datetime', 'text'],
    ['boolean', 'integer'],
]) as ReadonlyMap<string, string>;

const ClrMapping = [
    'System.String',
    'System.Int64',
    'System.DateTime',
    'System.Guid',
    'System.Byte[]',
    'System.Version',
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

function getInputElementByName(node: ParentNode, name: string): HTMLInputElement {
    return node.querySelector('[name="' + name + '"]') as HTMLInputElement;
}
function getSelectElementByName(node: ParentNode, name: string): HTMLSelectElement {
    return node.querySelector('[name="' + name + '"]') as HTMLSelectElement;
}

function isCheckMark(value: string) {
    return value === 'o';
}

class Entity {
    readonly tableNamePrefix = '## ';
    blockElements: BlockElements;

    constructor(blockElements: BlockElements) {
        this.blockElements = blockElements;
    }

    getIndex(lines: ReadonlyArray<string>) {
        var regLayout = /^###\s*layout\s*/;
        var regIndex  = /^###\s*index\s*/;

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

        for(var i = 0; i < lines.length; i++) {
            var line = lines[i];

            switch(state) {
                case State.Table:
                    if(line.startsWith(this.tableNamePrefix)) {
                        lineIndex.table = i;
                        state = State.Layout;
                    }
                    break;

                case State.Layout:
                    if(regLayout.test(line)) {
                        lineIndex.layout.head = i + 1;
                        state = State.Index;
                    }
                    break;
                    
                case State.Index:
                    if(regIndex.test(line)) {
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

    trimMarkdownTable(lines: ReadonlyArray<string>): ReadonlyArray<string> {
        return lines
            .map(s => s.trim())
            .filter(s => s.startsWith('|') && s.endsWith('|'))
        ;
    }

    trimDefine(rawDefine: EntityDefine): EntityDefine {
        var result = {
            table: rawDefine.table.substr(this.tableNamePrefix.length),
            layout: this.trimMarkdownTable(rawDefine.layout),
            index: this.trimMarkdownTable(rawDefine.index),
        } as EntityDefine;

        return result;
    }

    convertRowLines(markdownTableLines: ReadonlyArray<string>): ReadonlyArray<ReadonlyArray<string>> {
        var rows = markdownTableLines
            .map(i => i.replace(/(^\|)|(|$)/, ''))
            .map(i => i.split('|').map(s => s.trim()))
        ;
        if(2 < rows.length) {
            return rows.slice(2);
        }

        return [];
    }

    buildTable(parentElement: HTMLDivElement, tableName: string) {
        var tableTemplate = document.getElementById('template-table') as HTMLTemplateElement;
        var clonedTemplate = document.importNode(tableTemplate.content, true);

        var tableNameElement = getInputElementByName(clonedTemplate, 'table-name');
        tableNameElement.value = tableName;

        parentElement.appendChild(clonedTemplate);
    }

    createLayoutRowNode(columns: ReadonlyArray<string>): Node {
        var layoutRowTemplate = document.getElementById('template-layout-row') as HTMLTemplateElement;
        var clonedTemplate = document.importNode(layoutRowTemplate.content, true);

        getInputElementByName(clonedTemplate, 'pk').checked = isCheckMark(columns[LayoutColumn.PrimaryKey]);
        getInputElementByName(clonedTemplate, 'nn').checked = isCheckMark(columns[LayoutColumn.NotNull]);

        getInputElementByName(clonedTemplate, 'fk').value = columns[LayoutColumn.ForeignKey];

        getInputElementByName(clonedTemplate, 'name-logical').value = columns[LayoutColumn.LogicalColumnName];
        getInputElementByName(clonedTemplate, 'name-physical').value = columns[LayoutColumn.PhysicalColumnName];

        var logicalDataElement = getSelectElementByName(clonedTemplate, 'data-logical');
        var physicalDataElement = getInputElementByName(clonedTemplate, 'data-physical'); // 一方通行イベントで使うのでキャプチャしとく。メモリは無限
        logicalDataElement.value = columns[LayoutColumn.LogicalType];
        logicalDataElement.addEventListener('change', ev => {
            var physicalValue = DatabaseTypeMap.get(logicalDataElement.value);
            physicalDataElement.value = physicalValue || 'なんかデータ変';
        });
        logicalDataElement.dispatchEvent(new Event('change'));

        getSelectElementByName(clonedTemplate, 'data-clr').value = columns[LayoutColumn.ClrType];
        
        getInputElementByName(clonedTemplate, 'check').value = columns[LayoutColumn.CheckConstraint];
        getInputElementByName(clonedTemplate, 'comment').value = columns[LayoutColumn.Comment];
        
        return clonedTemplate;
    }

    buildLayout(parentElement: HTMLDivElement, layoutRows: ReadonlyArray<ReadonlyArray<string>>) {
        var layoutTemplate = document.getElementById('template-layout') as HTMLTemplateElement;
        var clonedTemplate = document.importNode(layoutTemplate.content, true);

        var rowsElement = clonedTemplate.querySelector('tbody') as HTMLElement;
        
        for(var layoutRow of layoutRows) {
            var rowElement = this.createLayoutRowNode(layoutRow);
            rowsElement.appendChild(rowElement)
        }

        parentElement.appendChild(clonedTemplate);
    }

    createIndexRowColumnNode(column: string): Node {
        var indexRowColumnTemplate = document.getElementById('template-index-row-column') as HTMLTemplateElement;
        var clonedTemplate = document.importNode(indexRowColumnTemplate.content, true);

        getInputElementByName(clonedTemplate, 'c').value = column;

        return clonedTemplate;
    }

    createIndexRowNode(isUnique: boolean, columns: ReadonlyArray<string>): Node {
        var indexRowTemplate = document.getElementById('template-index-row') as HTMLTemplateElement;
        var clonedTemplate = document.importNode(indexRowTemplate.content, true);

        getInputElementByName(clonedTemplate, 'uk').checked = isUnique;

        var columnsElement = clonedTemplate.querySelector('[name="columns"]')!;
        for(var column of columns) {
            var columnElement = this.createIndexRowColumnNode(column);
            columnsElement.appendChild(columnElement);
        }

        return clonedTemplate;
    }

    buildIndex(parentElement: HTMLDivElement, indexRows: ReadonlyArray<ReadonlyArray<string>>) {
        var indexTemplate = document.getElementById('template-index') as HTMLTemplateElement;
        var clonedTemplate = document.importNode(indexTemplate.content, true);

        var rowsElement = clonedTemplate.querySelector('tbody') as HTMLElement;
        
        for(var indexRow of indexRows) {
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
}

class EntityRelationManager {
    viewElement: HTMLDivElement;
    defineElement:  HTMLTextAreaElement;

    constructor(viewElement: HTMLDivElement, defineElement: HTMLTextAreaElement) {
        this.viewElement = viewElement;
        this.defineElement = defineElement;
    }

    createBlockElements(): BlockElements {
        var blockTemplate = document.getElementById('template-block') as HTMLTemplateElement;
        var clonedTemplate = document.importNode(blockTemplate.content, true);
        var block = {
            root: clonedTemplate.querySelector('[name="block-root"]'),
            table: clonedTemplate.querySelector('[name="block-table"]'),
            layout:clonedTemplate.querySelector('[name="block-layout"]'),
            index:  clonedTemplate.querySelector('[name="block-index"]'),
        } as BlockElements;

        return block;
    }

    public build() {
        var defines = this.defineElement.value.split('___');
        defines.shift();

        for(var define of defines) {
            var blockElements = this.createBlockElements();
            var entity = new Entity(blockElements);
            entity.build(define);

            this.viewElement.appendChild(blockElements.root);
        }
    }
}

const erMain = new EntityRelationManager(
    document.getElementById('view-main') as HTMLDivElement,
    document.getElementById('define-main') as HTMLTextAreaElement
);
erMain.build();

