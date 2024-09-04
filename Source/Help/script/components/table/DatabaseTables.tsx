import {
	Box,
	Divider,
	MenuItem,
	Select,
	type SelectChangeEvent,
	useTheme,
} from "@mui/material";
import { useAtom } from "jotai";
import { type FC, type ReactNode, useEffect, useMemo, useState } from "react";
import { WorkTablesAtom } from "../../stores/TableStore";
import {
	convertTable,
	convertWorkTable,
	splitRawEntities,
	splitRawSection,
	updateRelations,
} from "../../utils/table";
import { DatabaseTable } from "./DatabaseTable";
import { EditorButton } from "./editor";

interface DatabaseTablesProps {
	markdown: string;
}

export const DatabaseTables: FC<DatabaseTablesProps> = (
	props: DatabaseTablesProps,
) => {
	const { markdown } = props;
	const theme = useTheme();
	const [workTables, setWorkTables] = useAtom(WorkTablesAtom);
	const [selectedTableId, setSelectedTableId] = useState<string>("");

	const tables = useMemo(() => {
		console.debug("memo!");

		const tables = splitRawEntities(markdown)
			.map((a) => splitRawSection(a))
			.sort((a, b) => a.table.localeCompare(b.table))
			.map((a) => convertTable(a))
			.map((a) => convertWorkTable(a));

		updateRelations(tables);

		return tables;
	}, [markdown]);
	setWorkTables(tables);

	useEffect(() => {
		if (tables.length) {
			setSelectedTableId(tables[0].id);
		}
	}, [tables]);

	if (!workTables.length || !selectedTableId) {
		return <>...loading...</>;
	}

	function handleChange(
		event: SelectChangeEvent<string>,
		child: ReactNode,
	): void {
		setSelectedTableId(event.target.value);
	}

	return (
		<Box>
			<Box>
				<Select
					fullWidth
					value={selectedTableId}
					onChange={handleChange}
					sx={{
						background: theme.palette.primary.contrastText,
						color: theme.palette.primary.dark,
						fontWeight: "bold",
						"& .MuiOutlinedInput-notchedOutline": {
							borderRadius: 0,
						},
					}}
				>
					{workTables.map((a) => (
						<MenuItem key={a.id} value={a.id}>
							{a.define.tableName}
						</MenuItem>
					))}
				</Select>
			</Box>

			<Divider sx={{ marginBlock: "1rem" }} />

			<Box>
				<DatabaseTable tableId={selectedTableId} />
			</Box>

			<Box>
				<EditorButton size="medium">Copy Markdown</EditorButton>
				<EditorButton size="medium">Copy SQL</EditorButton>
			</Box>
			<Box
				sx={{
					position: "fixed",
					zIndex: 999999,
					right: "3ch",
					//bottom: 0,
					top: 0,
					height: "25vh",
					width: "40vw",
					overflow: "auto",
					background: "gray",
					fontSize: 10,
					fontFamily: "monospace",
				}}
			>
				<pre>{JSON.stringify(workTables, undefined, 2)}</pre>
			</Box>
		</Box>
	);
};
