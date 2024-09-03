import DeleteIcon from "@mui/icons-material/Delete";
import { Box, IconButton, MenuItem, Stack, TableRow } from "@mui/material";
import React, {
	type BaseSyntheticEvent,
	type FC,
	type MouseEvent,
	useState,
} from "react";
import { Controller, useForm } from "react-hook-form";
import {
	useWorkColumns,
	useWorkIndex,
	useWorkIndexes,
} from "../../stores/TableStore";
import type { TableBaseProps } from "../../types/table";
import {
	EditorButton,
	EditorCell,
	EditorCheckbox,
	EditorSelect,
	EditorTextField,
} from "./editor";

interface InputValues {
	isUnique: boolean;
	name: string;
	columns: string[];
}

interface DatabaseTableIndexProps extends TableBaseProps {
	indexId: string;
}

export const DatabaseTableIndex: FC<DatabaseTableIndexProps> = (
	props: DatabaseTableIndexProps,
) => {
	const { tableId, indexId } = props;
	const { workColumns } = useWorkColumns(tableId);
	const { workIndexes } = useWorkIndexes(tableId);
	const { workIndex } = useWorkIndex(tableId, indexId);

	const { control, handleSubmit } = useForm<InputValues>({
		mode: "onBlur",
		reValidateMode: "onChange",
		defaultValues: {
			isUnique: workIndex.isUnique,
			name: workIndex.name,
			columns: workIndex.columns,
		},
	});

	function handleInput(
		data: InputValues,
		event?: BaseSyntheticEvent<object, unknown, unknown> | undefined,
	): void {
		console.debug(data);
	}

	function handleRemoveIndex(event: MouseEvent): void {
		throw new Error("Function not implemented.");
	}

	function handleRemoveColumn(event: MouseEvent, columnId: string): void {
		throw new Error("Function not implemented.");
	}

	return (
		<TableRow>
			<EditorCell>
				<IconButton onClick={handleRemoveIndex}>
					<DeleteIcon />
				</IconButton>
			</EditorCell>

			<EditorCell>
				<Controller
					name="isUnique"
					control={control}
					render={({ field, formState: { errors } }) => (
						<EditorCheckbox {...field} onBlur={handleSubmit(handleInput)} />
					)}
				/>
			</EditorCell>
			<EditorCell>
				<Controller
					name="name"
					control={control}
					render={({ field, formState: { errors } }) => (
						<EditorTextField
							fullWidth
							{...field}
							onBlur={handleSubmit(handleInput)}
						/>
					)}
				/>
			</EditorCell>
			<EditorCell>
				<Stack>
					{workIndex.columns.map((a) => {
						return (
							<Box
								key={a}
								sx={{
									display: "flex",
								}}
							>
								<EditorSelect>
									{workColumns.items.map((b) => {
										return <MenuItem key={b.id}>{b.physicalName}</MenuItem>;
									})}
								</EditorSelect>
								<IconButton onClick={(ev) => handleRemoveColumn(ev, b.id)}>
									<DeleteIcon />
								</IconButton>
							</Box>
						);
					})}
					<Box sx={{ textAlign: "center" }}>
						<EditorButton>カラム追加</EditorButton>
					</Box>
				</Stack>
			</EditorCell>
		</TableRow>
	);
	//return <pre>{JSON.stringify(props)}</pre>;
};
