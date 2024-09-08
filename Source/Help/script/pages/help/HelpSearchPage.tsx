import {
	Box,
	Button,
	Checkbox,
	FormControlLabel,
	Stack,
	TextField,
} from "@mui/material";
import { type FC, useId } from "react";
import ReactDOMServer from "react-dom/server";
import { Controller, useForm } from "react-hook-form";
import { PageContent } from "../../components/layouts/PageContent";
import { DevPageKeys, HelpPageKeys, type PageKey, Pages } from "../../pages";
import type { PageProps } from "../../types/page";
import { getPage } from "../../utils/page";

const TargetHelpPageKeys: PageKey[] = HelpPageKeys.filter(
	(a) => !["help.search"].includes(a),
);
const TargetDevPageKeys: PageKey[] = DevPageKeys.filter(
	(a) =>
		!["dev.table_main", "dev.table_large", "dev.table_temporary"].includes(a),
);

function handleSelectPageKey(pageKey: PageKey): void {
	//nop
}

interface SearchInput {
	query: string;
	/** 大文字と小文字を区別する */
	case: boolean;
	/** 正規表現 */
	regex: boolean;
	/** 開発ドキュメントを含める */
	dev: boolean;
}

export const HelpSearchPage: FC<PageProps> = (props: PageProps) => {
	const id = useId();
	const { control, handleSubmit } = useForm<SearchInput>({
		defaultValues: {
			query: "",
			case: false,
			regex: false,
			dev: false,
		},
	});

	function onSubmit(data: SearchInput) {
		let targetKeys = [...TargetHelpPageKeys];
		if (data.dev) {
			targetKeys = [...targetKeys, ...TargetDevPageKeys];
		}

		console.debug({ data, targetKeys });

		targetKeys.map((a, i) => {
			console.debug(a);
			const currentPage = getPage(a, Pages);
			const html = ReactDOMServer.renderToStaticMarkup(
				<PageContent
					selectedPageKey={a}
					currentPage={currentPage}
					callbackSelectPageKey={handleSelectPageKey}
				/>,
				{
					identifierPrefix: id,
				},
			);
			console.debug(html);
		});
	}

	return (
		<form onSubmit={handleSubmit(onSubmit)}>
			<Box>
				<Controller
					control={control}
					name="query"
					rules={{
						required: true,
					}}
					render={({ field, formState: { errors } }) => (
						<TextField
							{...field}
							label="検索文言"
							fullWidth
							error={!!errors.query}
						/>
					)}
				/>
				<Stack direction="row">
					<Controller
						control={control}
						name="case"
						render={({ field, formState: { errors } }) => (
							<FormControlLabel
								control={<Checkbox {...field} checked={field.value} />}
								label="大文字と小文字を区別する"
							/>
						)}
					/>
					<Controller
						control={control}
						name="regex"
						render={({ field, formState: { errors } }) => (
							<FormControlLabel
								control={<Checkbox {...field} checked={field.value} />}
								label="正規表現を使用する"
							/>
						)}
					/>
					<Controller
						control={control}
						name="dev"
						render={({ field, formState: { errors } }) => (
							<FormControlLabel
								control={<Checkbox {...field} checked={field.value} />}
								label="開発ドキュメントを含める"
							/>
						)}
					/>
				</Stack>
				<Box>
					<Button variant="contained" type="submit">
						検索
					</Button>
				</Box>
			</Box>
		</form>
	);
};
