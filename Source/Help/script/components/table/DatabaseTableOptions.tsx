import { TextField } from "@mui/material";
import { useAtom } from "jotai";
import type { BaseSyntheticEvent, FC } from "react";
import { Controller, useForm } from "react-hook-form";
import { useWorkOptionsAtom, useWorkTableAtom } from "../../stores/TableStore";
import type { TableDefineProps } from "../../types/table";

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
	const  workOptionsAtom = useWorkOptionsAtom(tableId);
	const [workOptions, setWorkOptions] = useAtom(workOptionsAtom);

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
		setWorkOptions(state => ({
			...state,
			name: data.name
		}));
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
