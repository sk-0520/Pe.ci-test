import { useAtom } from "jotai";
import type { FC } from "react";
import { WorkTablesAtom, useWorkTableAtom } from "../../stores/TableStore";
import type { TableDefineProps } from "../../types/table";
import type { TableDefine } from "../../utils/table";
import { DatabaseTableColumns } from "./DatabaseTableColumns";
import { DatabaseTableIndexes } from "./DatabaseTableIndexes";
import { DatabaseTableOptions } from "./DatabaseTableOptions";

interface DatabaseTableProps {
	tableId: string;
}

export const DatabaseTable: FC<DatabaseTableProps> = (
	props: DatabaseTableProps,
) => {
	const { tableId } = props;

	return (
		<>
			<DatabaseTableOptions tableId={tableId} />
			{/* <DatabaseTableColumns tableId={tableId} />
			<DatabaseTableIndexes tableId={tableId} /> */}
		</>
	);
};
