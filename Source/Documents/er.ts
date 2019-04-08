interface BlockElements {
    root: HTMLDivElement;
    table: HTMLHeadingElement;
    layout: HTMLTableElement;
    index: HTMLTableElement;
}

interface EntityDefine {
    table: string;
    layout: ReadonlyArray<string>;
    index: ReadonlyArray<string>;
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

    public build(define: string) {
        var lines = define.split(/\r?\n/mg);
        var index = this.getIndex(lines);

        var rawDefines = {
            table: lines[index.table],
            layout: lines.slice(index.layout.head, index.layout.tail),
            index: lines.slice(index.index.head, index.index.tail)
        } as EntityDefine;
        var trimedDefines = this.trimDefine(rawDefines);
        alert(JSON.stringify(trimedDefines));
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
        var cloneTemplate = document.importNode(blockTemplate.content, true);
        var block = {
            root: cloneTemplate.querySelector('[name="block-root"]'),
            table: cloneTemplate.querySelector('[name="block-table"]'),
            layout:cloneTemplate.querySelector('[name="block-layout"]'),
            index:  cloneTemplate.querySelector('[name="block-index"]'),
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

const erManager = new EntityRelationManager(
    document.getElementById('view') as HTMLDivElement,
    document.getElementById('define') as HTMLTextAreaElement
);
erManager.build();

