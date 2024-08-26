import { Box, List } from "@mui/material";
import type { FC } from "react";
import { ChangelogVersion } from "../../components/changelog/ChangelogVersion";
import type { PageProps } from "../../types/PageProps";
import { getChangelogs } from "../../utils/changelog";

const changelogs = getChangelogs();

export const HelpChangelogPage: FC<PageProps> = (props: PageProps) => {
	return (
		<Box>
			{changelogs.map((a) => (
				<ChangelogVersion key={a.version} {...a} />
			))}
		</Box>
	);
};
