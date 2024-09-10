import { Typography } from "@mui/material";
import type { FC } from "react";
import type { PageKey } from "../../pages";
import { PageLink } from "../PageLink";

interface MdKeyboardProps {
	key: string;
}

export const MdKeyboard: FC<MdKeyboardProps> = (props: MdKeyboardProps) => {
	const { key } = props;

	return <Typography component="kbd">{key}</Typography>;
};
