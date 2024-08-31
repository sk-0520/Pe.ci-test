import { type PrimitiveAtom, atom, useAtom } from "jotai";
import type {
	TableDefine,
	WorkColumn,
	WorkColumns,
	WorkIdMapping,
	WorkIndex,
	WorkIndexes,
	WorkOptions,
	WorkTable,
} from "../utils/table";

export const WorkTablesAtom = atom<WorkTable[]>([]);

export const WorkIdMappingAtom = atom<{ [tableId in string]: WorkIdMapping }>(
	{},
);

export function useWorkTable(tableId: string) {
	const [workTables, setWorkTable] = useAtom(WorkTablesAtom);
	const workTable = workTables.find((a) => a.id === tableId);
	if (!workTable) {
		throw new Error(JSON.stringify({ tableId }));
	}

	return {
		workTable,
		updateWorkTable: (newValue: WorkTable) => {
			const index = workTables.indexOf(workTable);
			if (index === -1) {
				throw new Error(JSON.stringify({ tableId }));
			}
			workTables[index] = newValue;
			setWorkTable([...workTables]);
		},
	};
}
