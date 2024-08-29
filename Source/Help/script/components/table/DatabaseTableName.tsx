import { TextField } from "@mui/material";
import { useAtom } from "jotai";
import type { BaseSyntheticEvent, FC } from "react";
import { Controller, useForm } from "react-hook-form";
import { TableDefinesAtom } from "../../stores/TableStore";
import type { TableDefineProps } from "../../types/table";

interface InputValues {
	name: string;
}

interface DatabaseTableNameProps extends TableDefineProps {
	name: string;
}

export const DatabaseTableName: FC<DatabaseTableNameProps> = (
	props: DatabaseTableNameProps,
) => {
	const { name, tableDefine } = props;
	const [_, setTableDefines] = useAtom(TableDefinesAtom);
	const { control, handleSubmit } = useForm<InputValues>({
		mode: "onBlur",
		reValidateMode: "onChange",
		defaultValues: {
			name: name,
		},
	});

	function handleInput(
		data: InputValues,
		event?: BaseSyntheticEvent<object>,
	): void {
		setTableDefines((state) => {
			const index = state.indexOf(tableDefine);
			const current = state[index];
			current.name = data.name;
			return [...state];
		});
	}

	return (
		<Controller
			name="name"
			control={control}
			render={({ field, formState: { errors } }) => (
				<TextField
					label="テーブル名"
					{...field}
					onBlur={handleSubmit(handleInput)}
				/>
			)}
		/>
	);
};
