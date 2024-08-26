import { Box, Link, Typography } from "@mui/material";
import type { FC } from "react";
import type * as changelog from "../../types/changelog";

interface ChangelogReplaceLinkProps {
	children: string;
}

export const ChangelogReplaceLink: FC<ChangelogReplaceLinkProps> = (
	props: ChangelogReplaceLinkProps,
) => {
	const { children } = props;
	return children;
};
