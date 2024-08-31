import { Box, Button, Divider, FormControl } from "@mui/material";
import { useAtom } from "jotai";
import { type FC, useMemo } from "react";
import { TableDefinesAtom, WorkIdMappingAtom, WorkTablesAtom } from "../../stores/TableStore";
import {
	convertIdMap,
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
	const [_, setWorkIdMapping] = useAtom(WorkIdMappingAtom);

	const data = useMemo(() => {
		console.debug("memo!");

		const tables = splitRawEntities(markdown)
			.map((a) => splitRawSection(a))
			.sort((a, b) => a.table.localeCompare(b.table))
			.map((a) => convertTable(a))
			.map((a) => convertWorkTable(a));

		return {
			tables,
			idMap: Object.fromEntries(convertIdMap(tables)),
		};
	}, [markdown]);
	setWorkTables(data.tables);
	setWorkIdMapping(data.idMap);

	return (
		<>
			{data.tables.map((a, i) => (
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
