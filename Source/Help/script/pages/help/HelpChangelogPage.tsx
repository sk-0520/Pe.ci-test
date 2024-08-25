import { Box } from "@mui/material";
import type { FC } from "react";
import type { PageProps } from "../../types/PageProps";
import { getChangelogs } from "../../utils/changelog";

const changelogs = getChangelogs();

export const HelpChangelogPage: FC<PageProps> = (props: PageProps) => {
	return <Box>気が向いたらやるよ！</Box>;
};
