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
import { isCommonColumnName } from "../../utils/table";
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
}

interface DatabaseTableIndexProps extends TableBaseProps {
	indexId: string;
}

export const DatabaseTableIndex: FC<DatabaseTableIndexProps> = (
	props: DatabaseTableIndexProps,
) => {
	const { tableId, indexId } = props;
	const { workColumns } = useWorkColumns(tableId);
	const { workIndexes, updateWorkIndexes } = useWorkIndexes(tableId);
	const { workIndex, updateWorkIndex } = useWorkIndex(tableId, indexId);
	const [columnIds, setColumnIds] = useState(workIndex.columnIds);

	const { control, handleSubmit } = useForm<InputValues>({
		mode: "onBlur",
		reValidateMode: "onChange",
		defaultValues: {
			isUnique: workIndex.isUnique,
			name: workIndex.name,
		},
	});

	const getRawColumns = (columnIds: string[]): string[] => {
		return columnIds
			.map((a) => workColumns.items.find((b) => b.id === a))
			.filter((a) => a !== undefined)
			.map((a) => a.physicalName);
	};

	function handleInput(
		data: InputValues,
		event?: BaseSyntheticEvent<object, unknown, unknown> | undefined,
	): void {
		console.debug(data);

		updateWorkIndex({
			id: workIndex.id,
			isUnique: data.isUnique,
			name: data.name,
			columns: getRawColumns(columnIds),
			columnIds: columnIds,
		});
	}

	function handleAddColum(event: MouseEvent): void {
		columnIds.push(workIndex.columnIds[0]);
		setColumnIds([...columnIds]);
		updateWorkIndex({
			...workIndex,
			columns: getRawColumns(columnIds),
			columnIds: columnIds,
		});
	}

	function handleRemoveIndex(event: MouseEvent): void {
		const newItems = workIndexes.items.filter((a) => a.id !== indexId);
		updateWorkIndexes({
			...workIndexes,
			items: newItems,
		});
	}

	function handleRemoveColumn(event: MouseEvent, columnId: string): void {
		const index = columnIds.findIndex((a) => a === columnId);
		if (index === -1) {
			throw new Error(JSON.stringify({ columnId }));
		}
		columnIds.splice(index, 1);
		setColumnIds([...columnIds]);
		updateWorkIndex({
			...workIndex,
			columns: getRawColumns(columnIds),
			columnIds: columnIds,
		});
	}

	function handleChangeColumn(index: number, columnId: string): void {
		console.debug({ index, columnId });
		columnIds[index] = columnId;
		setColumnIds([...columnIds]);

		updateWorkIndex({
			...workIndex,
			columns: getRawColumns(columnIds),
			columnIds: columnIds,
		});
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
						<EditorCheckbox
							checked={field.value}
							{...field}
							onBlur={handleSubmit(handleInput)}
						/>
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
					{workIndex.columnIds.map((a, i) => {
						return (
							<Box
								key={a}
								sx={{
									display: "flex",
								}}
							>
								<EditorSelect
									value={columnIds[i]}
									onChange={(ev) =>
										handleChangeColumn(i, ev.target.value as string)
									}
									onBlur={handleSubmit(handleInput)}
								>
									{workColumns.items
										.filter((a) => !isCommonColumnName(a.physicalName))
										.map((b) => {
											return (
												<MenuItem key={b.id} value={b.id}>
													{b.physicalName}
												</MenuItem>
											);
										})}
								</EditorSelect>
								<IconButton onClick={(ev) => handleRemoveColumn(ev, a)}>
									<DeleteIcon />
								</IconButton>
							</Box>
						);
					})}
					<Box sx={{ textAlign: "center" }}>
						<EditorButton
							disabled={
								workColumns.items.filter(
									(a) => !isCommonColumnName(a.physicalName),
								).length <= columnIds.length
							}
							onClick={handleAddColum}
						>
							カラム追加
						</EditorButton>
					</Box>
				</Stack>
			</EditorCell>
		</TableRow>
	);
	//return <pre>{JSON.stringify(props)}</pre>;
};
