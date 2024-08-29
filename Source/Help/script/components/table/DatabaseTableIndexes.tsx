import { Table, TableBody, TableCell, TableHead, TableRow } from "@mui/material";
import type { FC } from "react";
import type { TableDefineProps } from "../../types/table";
import type { TableIndex } from "../../utils/table";
import { DatabaseTableIndex } from "./DatabaseTableIndex";

interface DatabaseTableIndexesProps extends TableDefineProps {
	indexes: TableIndex[];
}

export const DatabaseTableIndexes: FC<DatabaseTableIndexesProps> = (
	props: DatabaseTableIndexesProps,
) => {
	const { tableDefine, indexes } = props;

	return (
		<Table>
			<TableHead>
				<TableRow>
					<TableCell>UK</TableCell>
					<TableCell>名前</TableCell>
					<TableCell>カラム</TableCell>
				</TableRow>
			</TableHead>
			<TableBody>
				{indexes.map((a) => (
					<DatabaseTableIndex key={a.name} tableDefine={tableDefine} {...a} />
				))}
			</TableBody>
		</Table>
	);
};
