import type { FC } from "react";
import type { TableDefineProps } from "../../types/table";
import type { TableColumn } from "../../utils/table";

interface DatabaseTableColumnProps extends TableColumn, TableDefineProps {}

export const DatabaseTableColumn: FC<DatabaseTableColumnProps> = (
	props: DatabaseTableColumnProps,
) => {
	return <pre>{JSON.stringify(props)}</pre>;
};
