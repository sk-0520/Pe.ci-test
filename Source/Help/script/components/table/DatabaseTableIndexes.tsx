import { Box, TableBody, TableHead, TableRow } from "@mui/material";
import type { FC, MouseEvent } from "react";
import { useWorkColumns, useWorkIndexes } from "../../stores/TableStore";
import type { TableBaseProps } from "../../types/table";
import type { TableIndex } from "../../utils/table";
import { DatabaseTableIndex } from "./DatabaseTableIndex";
import { EditorButton, EditorCell, EditorTable } from "./editor";

interface DatabaseTableIndexesProps extends TableBaseProps {}

export const DatabaseTableIndexes: FC<DatabaseTableIndexesProps> = (
	props: DatabaseTableIndexesProps,
) => {
	const { tableId } = props;

	const { workIndexes } = useWorkIndexes(tableId);

	function handleAddIndex(event: MouseEvent): void {
		throw new Error("Function not implemented.");
	}

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
				<TableRow>
					<EditorCell colSpan={11}>
						<Box sx={{ textAlign: "center" }}>
							<EditorButton onClick={handleAddIndex}>
								インデックス追加
							</EditorButton>
						</Box>
					</EditorCell>
				</TableRow>
			</TableBody>
		</EditorTable>
	);
};
