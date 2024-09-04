import { Box, TableBody, TableHead, TableRow } from "@mui/material";
import type { FC, MouseEvent } from "react";
import {
	useWorkColumns,
	useWorkIndexes,
	useWorkTable,
} from "../../stores/TableStore";
import type { TableBaseProps } from "../../types/table";
import { type TableIndex, generateIndexesId } from "../../utils/table";
import { DatabaseTableIndex } from "./DatabaseTableIndex";
import { EditorButton, EditorCell, EditorTable } from "./editor";

interface DatabaseTableIndexesProps extends TableBaseProps {}

export const DatabaseTableIndexes: FC<DatabaseTableIndexesProps> = (
	props: DatabaseTableIndexesProps,
) => {
	const { tableId } = props;

	const { workTable } = useWorkTable(tableId);
	const { workIndexes, updateWorkIndexes } = useWorkIndexes(tableId);

	function handleAddIndex(event: MouseEvent): void {
		workIndexes.items.push({
			id: generateIndexesId(),
			isUnique: false,
			name: `idx_${workTable.define.tableName}_${workIndexes.items.length + 1}`,
			columns: [],
			columnIds: [],
		});
		updateWorkIndexes({
			...workIndexes,
		});
	}

	return (
		<EditorTable>
			<TableHead>
				<TableRow>
					<EditorCell>削除</EditorCell>
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
