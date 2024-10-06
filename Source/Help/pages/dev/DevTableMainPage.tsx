import { default as markdown } from "bundle-text:../../../../Define/table-main.md";
import type { FC } from "react";
import { DatabaseTables } from "../../components/table/DatabaseTables";
import type { PageProps } from "../../types/page";

export const DevTableMainPage: FC<PageProps> = (props: PageProps) => {
	return <DatabaseTables markdown={markdown} />;
};
