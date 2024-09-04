import type { FC } from "react";
import type { TableBaseProps } from "../../types/table";
import { isCommonColumnName } from "../../utils/table";

export interface DatabaseTableIndexColumnProps extends TableBaseProps {
	indexId: string;
}
export const DatabaseTableIndexColumn: FC<DatabaseTableIndexColumnProps> = (
	props: DatabaseTableIndexColumnProps,
) => {
	return <></>;
};
