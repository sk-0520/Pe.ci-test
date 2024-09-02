import { TableBody, TableHead, TableRow } from "@mui/material";
import type { FC } from "react";
import { useWorkColumns, useWorkIndexes } from "../../stores/TableStore";
import type { TableBaseProps } from "../../types/table";
import type { TableIndex } from "../../utils/table";
import { DatabaseTableIndex } from "./DatabaseTableIndex";
import { EditorCell, EditorTable } from "./editor";

interface DatabaseTableIndexesProps extends TableBaseProps {
}

export const DatabaseTableIndexes: FC<DatabaseTableIndexesProps> = (
	props: DatabaseTableIndexesProps,
) => {
	const { tableId } = props;

	const { workIndexes } = useWorkIndexes(tableId);

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
				{workIndexes.items.map((a) => (
					<DatabaseTableIndex key={a.id} tableId={tableId} indexId={a.id} />
				))}
				<TableRow>add</TableRow>
			</TableBody>
		</EditorTable>
	);
};
