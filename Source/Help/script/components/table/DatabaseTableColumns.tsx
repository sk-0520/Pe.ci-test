import { Box, TableBody, TableHead, TableRow } from "@mui/material";
import type { FC, MouseEvent } from "react";
import { useWorkColumns } from "../../stores/TableStore";
import type { TableBaseProps } from "../../types/table";
import { getElement } from "../../utils/access";
import type { Sqlite3Type } from "../../utils/sqlite";
import {
	ClrMap,
	type ClrTypeFullName,
	generateColumnId,
	generateTimestamp,
} from "../../utils/table";

import { DatabaseTableColumn } from "./DatabaseTableColumn";
import { DatabaseTableSection } from "./DatabaseTableSection";
import { EditorButton, EditorCell, EditorTable } from "./editor";

interface DatabaseTableColumnsProps extends TableBaseProps {}

export const DatabaseTableColumns: FC<DatabaseTableColumnsProps> = (
	props: DatabaseTableColumnsProps,
) => {
	const { tableId } = props;

	const { workColumns, updateWorkColumns } = useWorkColumns(tableId);

	function handleAddColumn(event: MouseEvent): void {
		const newColumnNumber = workColumns.items.length + 1;

		const newItems = [...workColumns.items];
		const defaultLogicalType: Sqlite3Type = "integer";
		const selectableClrTypes = getElement(ClrMap, defaultLogicalType);

		newItems.push({
			id: generateColumnId(),
			isPrimary: false,
			notNull: true,
			logical: {
				name: `column${newColumnNumber}`,
				type: defaultLogicalType,
			},
			physicalName: `column${newColumnNumber}`,
			clrType: selectableClrTypes[0] as ClrTypeFullName,
			foreignKey: undefined,
			foreignKeyId: undefined,
			comment: "",
			lastUpdateTimestamp: generateTimestamp(),
		});
		updateWorkColumns({
			...workColumns,
			items: newItems,
		});
	}

	return (
		<DatabaseTableSection title="レイアウト">
			<EditorTable>
				<TableHead>
					<TableRow>
						<EditorCell align="center">削除</EditorCell>
						<EditorCell align="center">PK</EditorCell>
						<EditorCell align="center">NN</EditorCell>
						<EditorCell align="center">FK</EditorCell>
						<EditorCell align="center">論理名</EditorCell>
						<EditorCell align="center">物理名</EditorCell>
						<EditorCell align="center">論理型</EditorCell>
						<EditorCell align="center">物理型</EditorCell>
						<EditorCell align="center">CLR</EditorCell>
						<EditorCell align="center">コメント</EditorCell>
					</TableRow>
				</TableHead>
				<TableBody>
					{workColumns.items.map((a) => (
						<DatabaseTableColumn key={a.id} tableId={tableId} columnId={a.id} />
					))}
					<TableRow>
						<EditorCell colSpan={11}>
							<Box sx={{ textAlign: "center" }}>
								<EditorButton onClick={handleAddColumn}>
									カラム追加
								</EditorButton>
							</Box>
						</EditorCell>
					</TableRow>
				</TableBody>
			</EditorTable>
		</DatabaseTableSection>
	);
};
