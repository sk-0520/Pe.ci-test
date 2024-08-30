import {
	Table,
	TableBody,
	TableCell,
	TableHead,
	TableRow,
} from "@mui/material";
import type { FC } from "react";
import type { TableDefineProps } from "../../types/table";
import type { TableColumn } from "../../utils/table";
import { DatabaseTableColumn } from "./DatabaseTableColumn";
import { EditorCell, EditorTable } from "./editor";

interface DatabaseTableColumnsProps extends TableDefineProps {
	columns: TableColumn[];
}

export const DatabaseTableColumns: FC<DatabaseTableColumnsProps> = (
	props: DatabaseTableColumnsProps,
) => {
	const { columns, tableDefine } = props;

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
				{columns.map((a) => (
					<DatabaseTableColumn
						key={a.physicalName}
						tableDefine={tableDefine}
						{...a}
					/>
				))}
				<TableRow>add</TableRow>
			</TableBody>
		</EditorTable>
	);
};
