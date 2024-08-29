import { Box } from "@mui/material";
import type { FC } from "react";
import type { TableDefineProps } from "../../types/table";
import type { TableDefine } from "../../utils/table";
import { DatabaseTableColumns } from "./DatabaseTableColumns";
import { DatabaseTableIndexes } from "./DatabaseTableIndexes";
import { DatabaseTableName } from "./DatabaseTableName";

interface DatabaseTableProps extends TableDefine, TableDefineProps {}

export const DatabaseTable: FC<DatabaseTableProps> = (
	props: DatabaseTableProps,
) => {
	const { name, columns, indexes, tableDefine } = props;

	return (
		<Box>
			<DatabaseTableName tableDefine={tableDefine} name={name} />
			<DatabaseTableColumns tableDefine={tableDefine} columns={columns} />
			<DatabaseTableIndexes tableDefine={tableDefine} indexes={indexes} />
		</Box>
	);
};
