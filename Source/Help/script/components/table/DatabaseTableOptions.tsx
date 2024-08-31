import { TextField } from "@mui/material";
import { useAtom } from "jotai";
import type { BaseSyntheticEvent, FC } from "react";
import { Controller, useForm } from "react-hook-form";
import { WorkTableAtom, useWorkTable } from "../../stores/TableStore";

interface InputValues {
	name: string;
}

interface DatabaseTableOptionsProps {
	tableId: string;
}

export const DatabaseTableOptions: FC<DatabaseTableOptionsProps> = (
	props: DatabaseTableOptionsProps,
) => {
	const { tableId } = props;

	// const [workTable, setWorkTable] = useAtom(WorkTableAtom(tableId));
	// const workOptions = workTable.options;
	const {workTable, updateWorkTable: update} = useWorkTable(tableId);
	const workOptions = workTable.options;

	const { control, handleSubmit } = useForm<InputValues>({
		mode: "onBlur",
		reValidateMode: "onChange",
		defaultValues: {
			name: workOptions.tableName,
		},
	});

	function handleInput(
		data: InputValues,
		event?: BaseSyntheticEvent<object>,
	): void {
		update({
			...workTable,
			options: {
				id: workTable.options.id,
				tableName: data.name
			}
		});
	}

	return (
		<Controller
			name="name"
			control={control}
			render={({ field, formState: { errors } }) => (
				<TextField
					label="テーブル名"
					fullWidth
					{...field}
					onBlur={handleSubmit(handleInput)}
				/>
			)}
		/>
	);
};
