import { type PrimitiveAtom, atom, useAtom } from "jotai";
import type {
	TableDefine,
	WorkColumn,
	WorkColumns,
	WorkIndex,
	WorkIndexes,
	WorkTable,
} from "../utils/table";

export const WorkTablesAtom = atom<WorkTable[]>([]);

export function useWorkTableAtom(tableId: string) {
	const [workTables] = useAtom(WorkTablesAtom);
	const workTable = workTables.find((a) => a.id === tableId);
	if (!workTable) {
		throw new Error(JSON.stringify({ tableId }));
	}
	return atom(workTable);
}

export function useWorkOptionsAtom(tableId: string) {
	const workTableAtom = useWorkTableAtom(tableId)
	const [workTable] = useAtom(workTableAtom);
	return atom(workTable.options)
}

export const WorkTableAtom = atom<{ [id: string]: PrimitiveAtom<WorkTable> }>(
	{},
);

export const WorkColumnsAtom = atom<{
	[id: string]: PrimitiveAtom<WorkColumns>;
}>({});
export const WorkColumnAtom = atom<{
	[id: string]: PrimitiveAtom<WorkColumn>;
}>({});
export const WorkIndexesAtom = atom<{
	[id: string]: PrimitiveAtom<WorkIndexes>;
}>({});
export const WorkIndexAtom = atom<{ [id: string]: PrimitiveAtom<WorkIndex> }>(
	{},
);
