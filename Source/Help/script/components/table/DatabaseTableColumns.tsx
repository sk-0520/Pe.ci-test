import {
	TableBody,
	TableHead,
	TableRow,
} from "@mui/material";
import type { FC } from "react";
import { useWorkColumns } from "../../stores/TableStore";
import type { TableBaseProps } from "../../types/table";
import { DatabaseTableColumn } from "./DatabaseTableColumn";
import { EditorCell, EditorTable } from "./editor";

interface DatabaseTableColumnsProps extends TableBaseProps {}

export const DatabaseTableColumns: FC<DatabaseTableColumnsProps> = (
	props: DatabaseTableColumnsProps,
) => {
	const { tableId } = props;

	const { workColumns: columns } = useWorkColumns(tableId);

	return (
		<EditorTable>
			<TableHead>
				<TableRow>
					<EditorCell>PK</EditorCell>
					<EditorCell>NN</EditorCell>
					<EditorCell>FK</EditorCell>
					<EditorCell>論理名</EditorCell>
					<EditorCell>物理名</EditorCell>
					<EditorCell>論理型</EditorCell>
					<EditorCell>物理型</EditorCell>
					<EditorCell>CLR</EditorCell>
					<EditorCell>チェック制約</EditorCell>
					<EditorCell>コメント</EditorCell>
					<EditorCell>削除</EditorCell>
				</TableRow>
			</TableHead>
			<TableBody>
				{columns.items.map((a) => (
					<DatabaseTableColumn
						key={a.id}
						tableId={tableId}
						columnId={a.id}
					/>
				))}
				<TableRow>add</TableRow>
			</TableBody>
		</EditorTable>
	);
};
