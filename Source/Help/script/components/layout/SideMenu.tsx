import List from '@mui/material/List';
import type { FC } from "react";
import React from "react";
import { Pages } from "../../pages";
import { SideMenuItem } from "./SideMenuItem";

export const SideMenu: FC = () => {
	return (
		<List>
			{Pages.map((a) => (
				<SideMenuItem key={a.key} page={a} />
			))}
		</List>
	);
};
