import type { FC } from "react";
import type { TableColumn } from "../../utils/table";
import { DatabaseTableColumn } from "./DatabaseTableColumn";

interface DatabaseTableColumnsProps {
	columns: TableColumn[];
}

export const DatabaseTableColumns: FC<DatabaseTableColumnsProps> = (
	props: DatabaseTableColumnsProps,
) => {
	const { columns } = props;

	return columns.map((a) => (
		<DatabaseTableColumn key={a.physical.name} {...a} />
	));
};
