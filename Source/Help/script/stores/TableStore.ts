import { type PrimitiveAtom, atom, useAtom } from "jotai";
import type {
	TableDefine,
	WorkColumn,
	WorkColumns,
	WorkDefine,
	WorkIndex,
	WorkIndexes,
	WorkTable,
} from "../utils/table";

export const WorkTablesAtom = atom<WorkTable[]>([]);

function getNow(): number {
	return new Date().getTime();
}

export function useWorkTable(tableId: string) {
	const [workTables, setWorkTable] = useAtom(WorkTablesAtom);
	const workTable = workTables.find((a) => a.id === tableId);
	if (!workTable) {
		throw new Error(JSON.stringify({ tableId }));
	}

	return {
		workTable,
		updateWorkTable: (newValue: Omit<WorkTable, "lastUpdateTimestamp">) => {
			const index = workTables.indexOf(workTable);
			if (index === -1) {
				throw new Error(JSON.stringify({ tableId }));
			}
			workTables[index] = {
				...newValue,
				lastUpdateTimestamp: getNow(),
			};
			setWorkTable([...workTables]);
		},
	};
}

export function useWorkDefine(tableId: string) {
	const { workTable, updateWorkTable } = useWorkTable(tableId);

	return {
		workDefine: workTable.define,
		updateWorkDefine: (newValue: Omit<WorkDefine, "lastUpdateTimestamp">) => {
			updateWorkTable({
				...workTable,
				define: {
					...newValue,
					lastUpdateTimestamp: getNow(),
				},
			});
		},
	};
}

export function useWorkColumns(tableId: string) {
	const { workTable, updateWorkTable } = useWorkTable(tableId);

	return {
		workColumns: workTable.columns,
		updateWorkColumns: (newValue: Omit<WorkColumns, "lastUpdateTimestamp">) => {
			updateWorkTable({
				...workTable,
				columns: {
					...newValue,
					lastUpdateTimestamp: getNow(),
				},
			});
		},
	};
}

export function useWorkColumn(tableId: string, columnId: string) {
	const { workColumns, updateWorkColumns } = useWorkColumns(tableId);

	const workColumn = workColumns.items.find((a) => a.id === columnId);
	if (!workColumn) {
		throw new Error(JSON.stringify({ tableId, columnId }));
	}

	return {
		workColumn: workColumn,
		updateWorkColumn: (newValue: Omit<WorkColumn, "lastUpdateTimestamp">) => {
			const index = workColumns.items.indexOf(workColumn);
			if(index === -1) {
				throw new Error(JSON.stringify({ tableId, columnId }));
			}

			workColumns.items[index] = {
				...newValue,
				lastUpdateTimestamp: getNow(),
			};

			updateWorkColumns({
				...workColumns,
			})
		}
	}
}
