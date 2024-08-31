import { atom } from "jotai";
import type { TableDefine, WorkIndex, WorkIndexes, WorkTable } from "../utils/table";

export const WorkTablesAtom = atom<WorkTable[]>([]);

export const WorkTableAtom = atom(
	(get) => get(WorkTablesAtom)
)
