
interface BlockElement {
    root: HTMLDivElement;
    title: HTMLHeadingElement;
    table: HTMLTableElement;
}

class TableSqlBlock {
    sqlLines: Array<string>;
    blockElement: BlockElement;

    constructor(sql: string, tableElement: BlockElement) {
        this.sqlLines = sql.split(/\r?\n/);
        this.blockElement = tableElement;
    }

    public build() {
    }
}

class ErManager {
    viewElement: HTMLDivElement;
    rawSqlElement:  HTMLTextAreaElement;
    tableSqlItems: Array<TableSqlBlock> = [];
    constructor(viewElement: HTMLDivElement, rawSqlElement: HTMLTextAreaElement) {
        this.viewElement = viewElement;
        this.rawSqlElement = rawSqlElement;
    }

    public build() {
        const blockFactory = () => {
            const rootElement = document.createElement('div') as HTMLDivElement;
            const titleElement = document.createElement('h3') as HTMLHeadingElement;
            const tableElement = document.createElement('table') as HTMLTableElement;

            rootElement.appendChild(titleElement);
            rootElement.appendChild(tableElement);
            this.viewElement.appendChild(rootElement);

            return {
                root: rootElement,
                title: titleElement,
                table: tableElement,
            } as BlockElement;
        };
        var sqlBlocks = this.rawSqlElement.value.split(/^\s*;\s*$/gm);
        this.tableSqlItems = sqlBlocks.map(s => new TableSqlBlock(s, blockFactory()));
        for(const item of this.tableSqlItems) {
            item.build();
        }
    }
}

const erManager = new ErManager(
    document.getElementById('view-table') as HTMLDivElement,
    document.getElementById('raw-sql') as HTMLTextAreaElement
);
erManager.build();

