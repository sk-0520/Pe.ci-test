import {
	List,
	ListItem,
	type SxProps,
	type Theme,
	Typography,
} from "@mui/material";
import type { FC } from "react";
import type * as changelog from "../../types/changelog";
import type { ChangelogContentItemType } from "../../types/changelog";
import { ChangelogReplaceLink } from "./ChangelogReplaceLink";
import { ChangelogRevision } from "./ChangelogRevision";

const TypeTitleMap: { [key in ChangelogContentItemType]: string } = {
	notice: "事前通知",
	compatibility: "互換性",
	nuget: "Nuget",
	myget: "MyGet",
	"plugin-compatibility": "プラグイン互換性",
};

const TypeStyleRootMap: { [key in ChangelogContentItemType]: SxProps<Theme> } =
	{
		notice: {},
		compatibility: {
			color: "red",
		},
		nuget: {},
		myget: {},
		"plugin-compatibility": {},
	};

const TypeStyleHeaderMap: {
	[key in ChangelogContentItemType]: SxProps<Theme>;
} = {
	notice: {
		color: "#f00",
	},
	compatibility: {},
	nuget: {
		color: "#0a0",
	},
	myget: {
		color: "#0a0",
	},
	"plugin-compatibility": {
		color: "#f74",
	},
};

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
				...(type && type in TypeStyleRootMap ? TypeStyleRootMap[type] : {}),
				listStyleType: "disc",
				display: "list-item",
			}}
		>
			<Typography variant="body1" component="span">
				{type && (
					<Typography
						component="span"
						sx={{
							...(type && type in TypeStyleHeaderMap
								? TypeStyleHeaderMap[type]
								: {}),
						}}
					>
						[{TypeTitleMap[type]}]
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

/*
	*.compatibility {
		color: #f00;
		&::before {
			content: "[互換性]";
		}
	}
	*.notice:before {
		content: "[事前通知]";
		color: #f00;
	}
	*.nuget:before {
		content: "[NuGet]";
		color: #0a0;
	}
	*.myget:before {
		content: "[myget]";
		color: #0a0;
	}
	*.plugin-compatibility:before {
		content: "[プラグイン互換性]";
		color: #f74;
	}
*/
