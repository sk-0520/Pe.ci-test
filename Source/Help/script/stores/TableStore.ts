import { type PrimitiveAtom, atom, useAtom } from "jotai";
import type {
	TableDefine,
	WorkColumn,
	WorkColumns,
	WorkDefine,
	WorkIdMapping,
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
		define: workTable.define,
		updateDefine: (newValue: Omit<WorkDefine, "lastUpdateTimestamp">) => {
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
