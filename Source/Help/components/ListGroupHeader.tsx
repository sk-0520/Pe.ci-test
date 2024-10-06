import {
	ListSubheader,
	type ListSubheaderProps,
	useTheme,
} from "@mui/material";
import React, { type FC } from "react";

export const ListGroupHeader: FC<ListSubheaderProps> = (props) => {
	const theme = useTheme();

	return (
		<ListSubheader
			{...props}
			sx={{
				background: theme.palette.primary.light,
				color: theme.palette.primary.contrastText,
				cursor: "default",
				userSelect: "none",
				fontWeight: "bold",
				...props.sx,
			}}
		/>
	);
};
