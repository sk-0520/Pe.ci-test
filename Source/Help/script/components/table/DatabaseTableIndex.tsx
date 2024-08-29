import type { FC } from "react";
import type { TableDefineProps } from "../../types/table";
import type { TableIndex } from "../../utils/table";

interface DatabaseTableIndexProps extends TableIndex, TableDefineProps {}

export const DatabaseTableIndex: FC<DatabaseTableIndexProps> = (
	props: DatabaseTableIndexProps,
) => {
	return <pre>{JSON.stringify(props)}</pre>;
};
