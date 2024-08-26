import { Box, Typography, useTheme } from "@mui/material";
import type { FC } from "react";
import type * as changelog from "../../types/changelog";
import { ChangelogContent } from "./ChangelogContent";

interface ChangelogVersionProps extends changelog.ChangelogVersion {}

export const ChangelogVersion: FC<ChangelogVersionProps> = (
	props: ChangelogVersionProps,
) => {
	const { date, contents, version } = props;
	const theme = useTheme();

	return (
		<Box>
			<Typography
				variant="h1"
				sx={{
					padding: "0.2ex 0.5ex",
					marginBlock: "1rem",
					background: theme.palette.primary.light,
					color: theme.palette.primary.contrastText,
					fontSize: "18pt",
					fontWeight: "bold",
					lineHeight: "1.5em",
				}}
			>
				{date}, {version}
			</Typography>

			{contents.map((a, i) => (
				// biome-ignore lint/suspicious/noArrayIndexKey: キーがねぇ
				<ChangelogContent key={i} {...a} />
			))}
		</Box>
	);
};
