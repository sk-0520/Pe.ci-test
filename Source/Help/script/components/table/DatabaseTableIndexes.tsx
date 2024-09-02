import { TableBody, TableHead, TableRow } from "@mui/material";
import type { FC } from "react";
import type { TableDefineProps } from "../../types/table";
import type { TableIndex } from "../../utils/table";
import { DatabaseTableIndex } from "./DatabaseTableIndex";
import { EditorCell, EditorTable } from "./editor";

interface DatabaseTableIndexesProps extends TableDefineProps {
	indexes: TableIndex[];
}

export const DatabaseTableIndexes: FC<DatabaseTableIndexesProps> = (
	props: DatabaseTableIndexesProps,
) => {
	const { tableDefine, indexes } = props;

	return (
		<EditorTable>
			<TableHead>
				<TableRow>
					<EditorCell>UK</EditorCell>
					<EditorCell>名前</EditorCell>
					<EditorCell>カラム</EditorCell>
				</TableRow>
			</TableHead>
			<TableBody>
				{indexes.map((a) => (
					<DatabaseTableIndex key={a.name} tableDefine={tableDefine} {...a} />
				))}
				<TableRow>add</TableRow>
			</TableBody>
		</EditorTable>
	);
};
