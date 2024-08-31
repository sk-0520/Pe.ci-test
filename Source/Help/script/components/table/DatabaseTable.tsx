import { useAtom } from "jotai";
import type { FC } from "react";
import { WorkTablesAtom } from "../../stores/TableStore";
import type { TableBaseProps, TableDefineProps } from "../../types/table";
import type { TableDefine } from "../../utils/table";
import { DatabaseTableColumns } from "./DatabaseTableColumns";
import { DatabaseTableDefine } from "./DatabaseTableDefine";
import { DatabaseTableIndexes } from "./DatabaseTableIndexes";

interface DatabaseTableProps extends TableBaseProps {
}

export const DatabaseTable: FC<DatabaseTableProps> = (
	props: DatabaseTableProps,
) => {
	const { tableId } = props;

	return (
		<>
			<DatabaseTableDefine tableId={tableId} />
			<DatabaseTableColumns tableId={tableId} />
			{/* <DatabaseTableIndexes tableId={tableId} /> */}
		</>
	);
};
