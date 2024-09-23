import type { FC } from "react";
import type { TableBaseProps } from "../../types/table";
import { DatabaseTableColumns } from "./DatabaseTableColumns";
import { DatabaseTableDefine } from "./DatabaseTableDefine";
import { DatabaseTableIndexes } from "./DatabaseTableIndexes";

interface DatabaseTableProps extends TableBaseProps {}

export const DatabaseTable: FC<DatabaseTableProps> = (
	props: DatabaseTableProps,
) => {
	const { tableId } = props;

	return (
		<>
			<DatabaseTableDefine tableId={tableId} />
			<DatabaseTableColumns tableId={tableId} />
			<DatabaseTableIndexes tableId={tableId} />
		</>
	);
};
