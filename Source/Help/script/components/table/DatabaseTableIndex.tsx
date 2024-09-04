import DeleteIcon from "@mui/icons-material/Delete";
import {
	Box,
	IconButton,
	MenuItem,
	Stack,
	TableRow,
	Typography,
} from "@mui/material";
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

	function handleInput(
		data: InputValues,
		event?: BaseSyntheticEvent<object, unknown, unknown> | undefined,
	): void {
		console.debug(data);

		const columns = columnIds
			.map((a) => workColumns.items.find((b) => b.id === a))
			.filter((a) => a !== undefined)
			.map((a) => a.physicalName);

		updateWorkIndex({
			id: workIndex.id,
			isUnique: data.isUnique,
			name: data.name,
			columns: columns,
			columnIds: columnIds,
		});
	}

	function handleAddColum(event: MouseEvent): void {
		throw new Error("Function not implemented.");
	}

	function handleRemoveIndex(event: MouseEvent): void {
		throw new Error("Function not implemented.");
	}

	function handleRemoveColumn(event: MouseEvent, columnId: string): void {
		throw new Error("Function not implemented.");
	}

	function handleChangeColumn(index: number, columnId: string): void {
		console.debug({ index, columnId });
		columnIds[index] = columnId;
		setColumnIds([...columnIds]);
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
						<EditorButton onClick={handleAddColum}>カラム追加</EditorButton>
					</Box>
				</Stack>
			</EditorCell>
		</TableRow>
	);
	//return <pre>{JSON.stringify(props)}</pre>;
};
