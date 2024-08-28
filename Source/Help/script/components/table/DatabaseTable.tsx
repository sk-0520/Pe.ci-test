import { Box } from "@mui/material";
import type { FC } from "react";
import type { TableDefine } from "../../utils/table";
import { DatabaseTableName } from "./DatabaseTableName";

interface DatabaseTableProps extends TableDefine {}

export const DatabaseTable: FC<DatabaseTableProps> = (
	props: DatabaseTableProps,
) => {
	const { name, columns, indexes } = props;

	return (
		<Box>
			<DatabaseTableName name={name} />
			<pre>{JSON.stringify({ columns, indexes })}</pre>
		</Box>
	);
};
