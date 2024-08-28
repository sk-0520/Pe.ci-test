import { Box, Button } from "@mui/material";
import { type FC, useMemo } from "react";
import {
	convertTable,
	splitRawEntities,
	splitRawSection,
} from "../../utils/table";
import { DatabaseTable } from "./DatabaseTable";

interface DatabaseTablesProps {
	markdown: string;
}

export const DatabaseTables: FC<DatabaseTablesProps> = (
	props: DatabaseTablesProps,
) => {
	const { markdown } = props;

	const tables = useMemo(() => {
		return splitRawEntities(markdown)
			.map((a) => splitRawSection(a))
			.sort((a, b) => a.table.localeCompare(b.table))
			.map((a) => convertTable(a));
	}, [markdown]);

	return (
		<Box>
			<Box>
				{tables.map((a, i) => (
					<DatabaseTable key={a.name} {...a} />
				))}
			</Box>
			<Box>
				<Button>Copy Markdown</Button>
				<Button>Copy SQL</Button>
			</Box>
		</Box>
	);
};
