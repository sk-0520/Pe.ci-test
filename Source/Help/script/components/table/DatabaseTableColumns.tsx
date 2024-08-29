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

	return columns.map((a) => (
		<DatabaseTableColumn
			key={a.physical.name}
			tableDefine={tableDefine}
			{...a}
		/>
	));
};
