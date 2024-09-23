import {
	List,
	ListItem,
	type SxProps,
	type Theme,
	Typography,
} from "@mui/material";
import type { FC } from "react";
import type * as changelog from "../../types/changelog";
import { ChangelogReplaceLink } from "./ChangelogReplaceLink";
import { ChangelogRevision } from "./ChangelogRevision";

const TypeMap: {
	[key in changelog.ChangelogContentItemType]: {
		title: string;
		styles: { root: SxProps<Theme>; header: SxProps<Theme> };
	};
} = {
	notice: {
		title: "事前通知",
		styles: {
			root: {},
			header: {
				color: "#f00",
			},
		},
	},
	compatibility: {
		title: "互換性",
		styles: {
			root: {
				color: "red",
			},
			header: {},
		},
	},
	nuget: {
		title: "Nuget",
		styles: {
			root: {},
			header: {
				color: "#0a0",
			},
		},
	},
	myget: {
		title: "MyGet",
		styles: {
			root: {},
			header: {
				color: "#0a0",
			},
		},
	},
	"plugin-compatibility": {
		title: "プラグイン互換性",
		styles: {
			root: {},
			header: {
				color: "#f74",
			},
		},
	},
} as const;

interface ChangelogContentItemProps extends changelog.ChangelogContentItem {}

export const ChangelogContentItem: FC<ChangelogContentItemProps> = (
	props: ChangelogContentItemProps,
) => {
	const { revision, subject, comments } = props;
	const type = props.class;

	return (
		<ListItem
			disablePadding
			sx={{
				...(type && type in TypeMap ? TypeMap[type].styles.root : {}),
				listStyleType: "disc",
				display: "list-item",
			}}
		>
			<Typography variant="body1" component="span">
				{type && (
					<Typography
						component="span"
						sx={{
							...(type && type in TypeMap ? TypeMap[type].styles.header : {}),
							marginRight: "1ch",
						}}
					>
						[{TypeMap[type].title}]
					</Typography>
				)}
				<ChangelogReplaceLink>{subject}</ChangelogReplaceLink>
				{revision && <ChangelogRevision revision={revision} />}
			</Typography>
			{comments && (
				<List disablePadding>
					{comments.map((a, i) => (
						<ListItem
							// biome-ignore lint/suspicious/noArrayIndexKey: キーがねぇ
							key={i}
							disablePadding
							sx={{
								listStyleType: "disc",
								display: "list-item",
								marginLeft: "3ch",
							}}
						>
							<ChangelogReplaceLink>{a}</ChangelogReplaceLink>
						</ListItem>
					))}
				</List>
			)}
		</ListItem>
	);
};
