import { MenuItem, Stack, TableRow } from "@mui/material";
import { type BaseSyntheticEvent, type FC, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import {
	useWorkColumns,
	useWorkIndex,
	useWorkIndexes,
} from "../../stores/TableStore";
import type { TableBaseProps } from "../../types/table";
import {
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

	return (
		<TableRow>
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
							<EditorSelect key={a}>
								{workColumns.items.map((b) => {
									return <MenuItem key={b.id}>{b.physicalName}</MenuItem>;
								})}
							</EditorSelect>
						);
					})}
				</Stack>
			</EditorCell>
		</TableRow>
	);
	//return <pre>{JSON.stringify(props)}</pre>;
};
