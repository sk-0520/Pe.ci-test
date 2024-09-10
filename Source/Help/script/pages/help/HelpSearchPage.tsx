import {
	Box,
	Button,
	Checkbox,
	Divider,
	FormControlLabel,
	List,
	ListItem,
	Stack,
	TextField,
	Typography,
} from "@mui/material";
import { convert } from "html-to-text";
import { type FC, useState } from "react";
import ReactDOMServer from "react-dom/server";
import { Controller, useForm } from "react-hook-form";
import { PageLink } from "../../components/PageLink";
import { PageContent } from "../../components/layouts/PageContent";
import {
	DevPageKeys,
	HelpPageKeys,
	type PageElement,
	type PageKey,
	Pages,
} from "../../pages";
import type { PageProps } from "../../types/page";
import { getPage } from "../../utils/page";
import { escapeRegexPattern } from "../../utils/string";

type SearchPosition = {
	index: number;
	length: number;
};

type SearchLine = {
	content: string;
	positions: SearchPosition[];
};

interface SearchResult {
	element: PageElement;
	lines: Array<SearchLine>;
}

interface PageCache {
	element: PageElement;
	lines: ReadonlyArray<string>;
}

const HtmlNewLine = /\r?\n/g;

const TargetHelpPageKeys: PageKey[] = HelpPageKeys.filter(
	(a) => !["help.search", "help.changelog"].includes(a),
);
const TargetDevPageKeys: PageKey[] = DevPageKeys.filter(
	(a) =>
		!["dev.table_main", "dev.table_large", "dev.table_temporary"].includes(a),
);

function handleSelectPageKey(pageKey: PageKey): void {
	//nop
}

//TODO: 検索結果文字列長はここで結果に含めないといけない
function indexOf(
	haystack: string,
	needle: string,
	startIndex: number,
	regex: boolean,
	caseSensitive: boolean,
): number {
	if (regex) {
		const pattern = escapeRegexPattern(needle);
		const regex = new RegExp(pattern, caseSensitive ? "i" : "");
		return haystack.search(regex);
	}

	if (caseSensitive) {
		return haystack.indexOf(needle, startIndex);
	}

	return haystack.toUpperCase().indexOf(needle.toUpperCase(), startIndex);
}

const PageCacheMap = new Map<PageKey, PageCache>();

interface SearchInput {
	/** 検索文言 */
	query: string;
	/** 大文字と小文字を区別するか */
	caseSensitive: boolean;
	/** 検索文言を正規表現として扱うか */
	isRegex: boolean;
	/** 開発ドキュメントを含めるか */
	includeDevelopment: boolean;
}

export const HelpSearchPage: FC<PageProps> = (props: PageProps) => {
	const { control, handleSubmit } = useForm<SearchInput>({
		defaultValues: {
			query: "",
			caseSensitive: false,
			isRegex: false,
			includeDevelopment: false,
		},
	});
	const [searchItems, setSearchItems] = useState<SearchResult[]>();

	function onSubmit(data: SearchInput) {
		let targetKeys = [...TargetHelpPageKeys];
		if (data.includeDevelopment) {
			targetKeys = [...targetKeys, ...TargetDevPageKeys];
		}

		const { isRegex, caseSensitive, includeDevelopment, query } = data;

		for (const targetKey of targetKeys) {
			if (PageCacheMap.has(targetKey)) {
				continue;
			}

			const currentPage = getPage(targetKey, Pages);
			let html: string;

			const defaultError = console.error;
			try {
				const ignoreMessages = [
					"Warning: useLayoutEffect does nothing on the server, because its effect cannot be encoded into the server renderer's output format.",
					"Warning: Detected multiple renderers concurrently rendering the same context provider. This is currently unsupported.",
				];
				console.error = (message?: unknown, ...optionalParams: unknown[]) => {
					if (typeof message === "string") {
						if (ignoreMessages.some((a) => message.startsWith(a))) {
							return;
						}
					}
					defaultError(message, ...optionalParams);
				};

				html = ReactDOMServer.renderToStaticMarkup(
					<PageContent
						selectedPageKey={targetKey}
						currentPage={currentPage}
						callbackSelectPageKey={handleSelectPageKey}
					/>,
				);
			} finally {
				console.error = defaultError;
			}

			const text = convert(html, {
				wordwrap: false,
			});

			const cache: PageCache = {
				element: currentPage,
				lines: text.split(HtmlNewLine).filter((a) => a),
			};
			PageCacheMap.set(targetKey, cache);
		}

		const result: Array<SearchResult> = [];

		for (const [key, value] of PageCacheMap) {
			if (!includeDevelopment) {
				if (TargetDevPageKeys.includes(key)) {
					continue;
				}
			}

			const work: SearchResult = {
				element: value.element,
				lines: [],
			};

			let startIndex = 0;
			for (const line of value.lines) {
				const searchLine: SearchLine = {
					content: line,
					positions: [],
				};

				const index = indexOf(line, query, startIndex, isRegex, caseSensitive);
				if (index === -1) {
					continue;
				}

				searchLine.positions.push({
					index: index,
					length: query.length,
				});

				startIndex += query.length;

				work.lines.push(searchLine);
			}
			if (work.lines.length) {
				result.push(work);
			}
		}

		setSearchItems(result);
	}

	return (
		<Box>
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
								type="search"
								label="検索文言"
								fullWidth
								error={!!errors.query}
							/>
						)}
					/>
					<Stack direction="row">
						<Controller
							control={control}
							name="caseSensitive"
							render={({ field, formState: { errors } }) => (
								<FormControlLabel
									control={<Checkbox {...field} checked={field.value} />}
									label="大文字と小文字を区別する"
								/>
							)}
						/>
						<Controller
							control={control}
							name="isRegex"
							render={({ field, formState: { errors } }) => (
								<FormControlLabel
									control={<Checkbox {...field} checked={field.value} />}
									label="正規表現を使用する"
								/>
							)}
						/>
						<Controller
							control={control}
							name="includeDevelopment"
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
			{searchItems !== undefined && (
				<Box sx={{ marginBlock: "2em" }}>
					<Divider />
					<Typography>
						検索結果:
						<Typography component="code" sx={{ marginLeft: "1ch" }}>
							{searchItems.length}
						</Typography>
					</Typography>

					<List>
						{searchItems.map((a) => {
							return (
								<ListItem key={a.element.key} sx={{ display: "block" }}>
									<PageLink page={a.element.key} />
									<List sx={{ margin: 0, marginLeft: "2ch", padding: 0 }}>
										{a.lines.map((b, i) => (
											<ListItem
												// biome-ignore lint/suspicious/noArrayIndexKey: いやまぁ他にないし
												key={i}
												sx={{ padding: 0 }}
											>
												{b.content}
											</ListItem>
										))}
									</List>
								</ListItem>
							);
						})}
					</List>
				</Box>
			)}
		</Box>
	);
};
