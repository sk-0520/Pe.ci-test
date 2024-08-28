import type { FC } from "react";
import type { TableColumn } from "../../utils/table";

interface DatabaseTableColumnProps extends TableColumn {}

export const DatabaseTableColumn: FC<DatabaseTableColumnProps> = (
	props: DatabaseTableColumnProps,
) => {
	return <pre>{JSON.stringify(props)}</pre>;
};
