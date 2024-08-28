import type { FC } from "react";
import type { TableIndex } from "../../utils/table";
import { DatabaseTableIndex } from "./DatabaseTableIndex";

interface DatabaseTableIndexesProps {
	indexes: TableIndex[];
}

export const DatabaseTableIndexes: FC<DatabaseTableIndexesProps> = (
	props: DatabaseTableIndexesProps,
) => {
	return props.indexes.map((a) => <DatabaseTableIndex key={a.name} {...a} />);
};
