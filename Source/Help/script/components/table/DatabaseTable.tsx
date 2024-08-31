import { useAtom } from "jotai";
import type { FC } from "react";
import { WorkTablesAtom } from "../../stores/TableStore";
import type { TableDefineProps } from "../../types/table";
import type { TableDefine } from "../../utils/table";
import { DatabaseTableColumns } from "./DatabaseTableColumns";
import { DatabaseTableIndexes } from "./DatabaseTableIndexes";
import { DatabaseTableName } from "./DatabaseTableName";

interface DatabaseTableProps {
	tableId: string;
}

export const DatabaseTable: FC<DatabaseTableProps> = (
	props: DatabaseTableProps,
) => {
	const { tableId } = props;
	const workTables = useAtom(WorkTablesAtom)

	return (
		<>
			<DatabaseTableName tableId={tableId} />
			<DatabaseTableColumns tableId={tableId} />
			<DatabaseTableIndexes tableId={tableId} />
		</>
	);
};
