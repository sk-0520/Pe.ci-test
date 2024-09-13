import { Box, List, type SxProps, type Theme, Typography } from "@mui/material";
import type { FC } from "react";
import type {
	ChangelogContentKind,
	ChangelogContent as ChangelogContentType,
} from "../../types/changelog";
import { ChangelogContentItem } from "./ChangelogContentItem";
const KindMap: {
	[key in ChangelogContentKind]: {
		title: string;
		styles: { header: SxProps<Theme> };
	};
} = {
	note: {
		title: "メモ",
		styles: {
			header: {
				background: "#ffc",
			},
		},
	},
	features: {
		title: "機能",
		styles: {
			header: {
				background: "#cfc",
			},
		},
	},
	fixes: {
		title: "修正",
		styles: {
			header: {
				background: "#fcc",
			},
		},
	},
	developer: {
		title: "開発",
		styles: {
			header: {
				background: "#ccf",
			},
		},
	},
} as const;

interface ChangelogContentProps extends ChangelogContentType {}

export const ChangelogContent: FC<ChangelogContentProps> = (
	props: ChangelogContentProps,
) => {
	const { type, logs } = props;

	return (
		<Box>
			<Typography
				variant="h2"
				sx={{
					...KindMap[type].styles.header,
					padding: "0.2ex 0.5ex",
					marginBlock: "1rem",
					fontSize: "16pt",
					fontWeight: "bold",
					lineHeight: "1.25em",
				}}
			>
				{KindMap[type].title}
			</Typography>

			<List disablePadding sx={{ marginLeft: "30pt" }}>
				{logs.map((a, i) => (
					// biome-ignore lint/suspicious/noArrayIndexKey: キーがねぇ
					<ChangelogContentItem key={i} {...a} />
				))}
			</List>
		</Box>
	);
};

/*
		&.note {
			background: #ffc;
		}
		&.features {
			background: #cfc;
		}
		&.fixes {
			background: #fcc;
		}
		&.developer {
			background: #ccf;
		}
*/
