import type { FC } from "react";
import type { TableDefineProps } from "../../types/table";
import type { TableIndex } from "../../utils/table";
import { DatabaseTableIndex } from "./DatabaseTableIndex";

interface DatabaseTableIndexesProps extends TableDefineProps {
	indexes: TableIndex[];
}

export const DatabaseTableIndexes: FC<DatabaseTableIndexesProps> = (
	props: DatabaseTableIndexesProps,
) => {
	const { tableDefine } = props;
	return props.indexes.map((a) => (
		<DatabaseTableIndex key={a.name} tableDefine={tableDefine} {...a} />
	));
};
