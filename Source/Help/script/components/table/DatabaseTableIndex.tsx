import type { FC } from "react";
import type { TableIndex } from "../../utils/table";

interface DatabaseTableIndexProps extends TableIndex {}

export const DatabaseTableIndex: FC<DatabaseTableIndexProps> = (
	props: DatabaseTableIndexProps,
) => {
	return <pre>{JSON.stringify(props)}</pre>;
};
