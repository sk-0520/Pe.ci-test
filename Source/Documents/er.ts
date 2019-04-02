
interface BlockElement {
    root: HTMLDivElement;
    title: HTMLHeadingElement;
    table: HTMLTableElement;
}

class TableSqlBlock {
    sql: string;
    blockElement: BlockElement;

    constructor(sql: string, tableElement: BlockElement) {
        this.sql = sql;
        this.blockElement = tableElement;
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

    public import() {
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
        var sqlBlocks = this.rawSqlElement.value.split(/^\s*;\s*\\/);
        this.tableSqlItems = sqlBlocks.map(s => new TableSqlBlock(s, blockFactory()));
    }
}

const sqlLoader = new ErManager(
    document.getElementById('view-table') as HTMLDivElement,
    document.getElementById('raw-sql') as HTMLTextAreaElement
);
sqlLoader.import();

