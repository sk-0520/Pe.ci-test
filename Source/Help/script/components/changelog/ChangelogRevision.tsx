import { Link } from "@mui/material";
import type { FC } from "react";

const revisionLink = "https://github.com/sk-0520/Pe/commit/";

interface ChangelogRevisionProps {
	revision: string;
}

export const ChangelogRevision: FC<ChangelogRevisionProps> = (
	props: ChangelogRevisionProps,
) => {
	const { revision } = props;

	return (
		<Link
			sx={{
				verticalAlign: "middle",
				display: "inline-block",
				margin: "0 1ex",
				border: "1px solid #888",
				borderRadius: "4px",
				padding: "1px",
				fontSize: "70%",
				lineHeight: "1em",
				whiteSpace: "nowrap",
				fontFamily: "Consolas",
			}}
			href={revisionLink + revision}
		>
			{revision.substring(0, 6)}…
		</Link>
	);
};
