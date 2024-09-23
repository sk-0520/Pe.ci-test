import { atom, useAtom } from "jotai";
import { useEffect, useState } from "react";
import {
	type WorkColumn,
	type WorkColumns,
	type WorkDefine,
	type WorkIndex,
	type WorkIndexes,
	type WorkTable,
	generateTimestamp,
} from "../utils/table";

export const WorkTablesAtom = atom<WorkTable[]>([]);

export function useWorkTable(tableId: string) {
	const [workTables, setWorkTable] = useAtom(WorkTablesAtom);
	const [tempTable, setTempTable] =
		useState<Omit<WorkTable, "lastUpdateTimestamp">>();

	// 子から親更新であれこれ警告出る対策
	useEffect(() => {
		if (!tempTable) {
			return;
		}
		const index = workTables.findIndex((a) => a.id === tempTable.id);
		if (index === -1) {
			throw new Error(JSON.stringify({ tableId }));
		}
		workTables[index] = {
			...tempTable,
			lastUpdateTimestamp: generateTimestamp(),
		};
		setWorkTable([...workTables]);
	}, [tableId, workTables, setWorkTable, tempTable]);

	const workTable = workTables.find((a) => a.id === tableId);
	if (!workTable) {
		throw new Error(JSON.stringify({ tableId }));
	}

	return {
		workTable,
		updateWorkTable: (newValue: Omit<WorkTable, "lastUpdateTimestamp">) => {
			setTempTable(newValue);
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
					lastUpdateTimestamp: generateTimestamp(),
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
					lastUpdateTimestamp: generateTimestamp(),
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
			if (index === -1) {
				throw new Error(JSON.stringify({ tableId, columnId }));
			}

			workColumns.items[index] = {
				...newValue,
				lastUpdateTimestamp: generateTimestamp(),
			};

			updateWorkColumns({
				...workColumns,
			});
		},
	};
}

export function useWorkIndexes(tableId: string) {
	const { workTable, updateWorkTable } = useWorkTable(tableId);

	return {
		workIndexes: workTable.indexes,
		updateWorkIndexes: (newValue: Omit<WorkIndexes, "lastUpdateTimestamp">) => {
			updateWorkTable({
				...workTable,
				indexes: {
					...newValue,
					lastUpdateTimestamp: generateTimestamp(),
				},
			});
		},
	};
}

export function useWorkIndex(tableId: string, indexId: string) {
	const { workIndexes, updateWorkIndexes } = useWorkIndexes(tableId);

	const workIndex = workIndexes.items.find((a) => a.id === indexId);
	if (!workIndex) {
		throw new Error(JSON.stringify({ tableId, indexId }));
	}

	return {
		workIndex,
		updateWorkIndex: (newValue: Omit<WorkIndex, "lastUpdateTimestamp">) => {
			const index = workIndexes.items.indexOf(workIndex);
			if (index === -1) {
				throw new Error(JSON.stringify({ tableId, indexId }));
			}

			workIndexes.items[index] = {
				...newValue,
			};

			updateWorkIndexes({
				...workIndexes,
			});
		},
	};
}
