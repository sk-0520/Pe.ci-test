import { TextField } from "@mui/material";
import type { BaseSyntheticEvent, FC } from "react";
import { Controller, useForm } from "react-hook-form";
import { useWorkDefine } from "../../stores/TableStore";
import type { TableBaseProps } from "../../types/table";

interface InputValues {
	name: string;
}

interface DatabaseTableDefineProps extends TableBaseProps {}

export const DatabaseTableDefine: FC<DatabaseTableDefineProps> = (
	props: DatabaseTableDefineProps,
) => {
	const { tableId } = props;
	const { workDefine, updateWorkDefine } = useWorkDefine(tableId);

	const defaultValues: InputValues = {
		name: workDefine.tableName,
	};
	const { control, handleSubmit } = useForm<InputValues>({
		mode: "onBlur",
		reValidateMode: "onChange",
		defaultValues: defaultValues,
		values: defaultValues,
	});

	console.debug(`title: ${workDefine.tableName}`);

	function handleInput(
		data: InputValues,
		event?: BaseSyntheticEvent<object>,
	): void {
		updateWorkDefine({
			...workDefine,
			tableName: data.name,
		});
		//reset();
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
