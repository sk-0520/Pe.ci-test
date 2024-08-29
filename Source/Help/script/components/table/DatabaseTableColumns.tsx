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

interface DatabaseTableColumnsProps extends TableDefineProps {
	columns: TableColumn[];
}

export const DatabaseTableColumns: FC<DatabaseTableColumnsProps> = (
	props: DatabaseTableColumnsProps,
) => {
	const { columns, tableDefine } = props;

	return (
		<Table>
			<TableHead>
				<TableRow>
					<TableCell>PK</TableCell>
					<TableCell>NN</TableCell>
					<TableCell>FK</TableCell>
					<TableCell>論理名</TableCell>
					<TableCell>物理名</TableCell>
					<TableCell>論理型</TableCell>
					<TableCell>物理型</TableCell>
					<TableCell>CLR</TableCell>
					<TableCell>チェック制約</TableCell>
					<TableCell>コメント</TableCell>
					<TableCell>削除</TableCell>
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
		</Table>
	);
};
