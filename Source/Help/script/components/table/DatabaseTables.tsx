import { Box, Button, Divider, FormControl } from "@mui/material";
import { useAtom } from "jotai";
import { type FC, useMemo } from "react";
import { TableDefinesAtom, WorkTablesAtom } from "../../stores/TableStore";
import {
	convertTable,
	convertWorkTable,
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
	const [workTables, setWorkTables] = useAtom(WorkTablesAtom);

	const tables = useMemo(() => {
		console.debug("memo!")

		return splitRawEntities(markdown)
			.map((a) => splitRawSection(a))
			.sort((a, b) => a.table.localeCompare(b.table))
			.map((a) => convertTable(a))
			.map(a => convertWorkTable(a))
	}, [markdown]);
	setWorkTables(tables);
	console.debug(tables)

	return (
		<>
			{tables.map((a, i) => (
				<>
					{i !== 0 && <Divider sx={{ marginBlock: "5rem" }} />}
					<DatabaseTable key={a.id} tableId={a.id} />
				</>
			))}

			<Box>
				<Button>Copy Markdown</Button>
				<Button>Copy SQL</Button>
			</Box>
			<Box
				sx={{
					position: "fixed",
					zIndex: 999999,
					right: "3ch",
					bottom: 0,
					height: "35vh",
					width: "40vw",
					overflow: "auto",
					background: "gray",
					fontSize: 10,
					fontFamily: "monospace",
				}}
			>
				<pre>{JSON.stringify(workTables, undefined, 2)}</pre>
			</Box>
		</>
	);
};
