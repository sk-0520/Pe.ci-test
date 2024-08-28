import { Box, TextField } from "@mui/material";
import { type FC, useState } from "react";

interface DatabaseTableNameProps {
	name: string;
}

export const DatabaseTableName: FC<DatabaseTableNameProps> = (
	props: DatabaseTableNameProps,
) => {
	const { name } = props;
	const [tableName, setTableName] = useState(name);

	return (
		<Box>
			<TextField
				label="テーブル名"
				value={tableName}
				onChange={(ev) => setTableName(ev.target.value)}
			/>
		</Box>
	);
};
