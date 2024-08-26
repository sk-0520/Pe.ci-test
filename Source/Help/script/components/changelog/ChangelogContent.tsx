import {
	Box,
	List,
	ListItem,
	type SxProps,
	type Theme,
	Typography,
} from "@mui/material";
import type { FC } from "react";
import type * as changelog from "../../types/changelog";
import type { ChangelogContentKind } from "../../types/changelog";
import { ChangelogContentItem } from "./ChangelogContentItem";

const KindTitleMap: { [key in ChangelogContentKind]: string } = {
	note: "メモ",
	features: "機能",
	fixes: "修正",
	developer: "開発",
};

const KindStyleMap: { [key in ChangelogContentKind]: SxProps<Theme> } = {
	note: {
		background: "#ffc",
	},
	features: {
		background: "#cfc",
	},
	fixes: {
		background: "#fcc",
	},
	developer: {
		background: "#ccf",
	},
};

interface ChangelogContentProps extends changelog.ChangelogContent {}

export const ChangelogContent: FC<ChangelogContentProps> = (
	props: ChangelogContentProps,
) => {
	const { type, logs } = props;

	return (
		<Box>
			<Typography
				variant="h2"
				sx={{
					...KindStyleMap[type],
					padding: "0.2ex 0.5ex",
					marginBlock: "1rem",
					fontSize: "16pt",
					fontWeight: "bold",
					lineHeight: "1.25em",
				}}
			>
				{KindTitleMap[type]}
			</Typography>

			<List disablePadding sx={{ marginLeft: "30pt" }}>
				{logs.map((a, i) => (
					// biome-ignore lint/suspicious/noArrayIndexKey: キーがねぇ
					<ListItem
						key={i}
						sx={{ listStyleType: "disc", display: "list-item" }}
					>
						<ChangelogContentItem {...a} />
					</ListItem>
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
