import List from "@mui/material/List";
import type { FC } from "react";
import React from "react";
import { type PageKey, Pages } from "../../pages";
import { SideMenuItem } from "./SideMenuItem";

interface SideMenuProps {
	selectedPageKey: PageKey;
	handleSelectPageKey: (pageKey: PageKey) => void;
}

export const SideMenu: FC<SideMenuProps> = (props: SideMenuProps) => {
	const { selectedPageKey, handleSelectPageKey } = props;

	return (
		<List disablePadding>
			<span>{selectedPageKey}</span>
			{Pages.map((a) => (
				<SideMenuItem
					key={a.key}
					selectedPageKey={selectedPageKey}
					handleSelectPageKey={handleSelectPageKey}
					page={a}
					nestLevel={0}
				/>
			))}
		</List>
	);
};
