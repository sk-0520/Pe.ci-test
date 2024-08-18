import { List, ListItemButton } from "@mui/material";
import React, { type FC, type MouseEvent } from "react";
import type { PageElement, PageKey } from "../../pages";

type SideMenuItemProps = {
	selectedPageKey: PageKey;
	handleSelectPageKey: (pageKey: PageKey) => void;
	page: PageElement;
	nestLevel: number;
};

export const SideMenuItem: FC<SideMenuItemProps> = (
	props: SideMenuItemProps,
) => {
	const { handleSelectPageKey, page, nestLevel } = props;

	function handleSelectMenu(event: MouseEvent): void {
		handleSelectPageKey(page.key);
	}

	return (
		<>
			<ListItemButton onClick={handleSelectMenu}>{page.title}</ListItemButton>
			{page.nodes && 0 < page.nodes.length && (
				<List disablePadding>
					{page.nodes.map((a) => (
						<SideMenuItem
							key={a.key}
							selectedPageKey={props.selectedPageKey}
							handleSelectPageKey={props.handleSelectPageKey}
							page={a}
							nestLevel={nestLevel + 1}
						/>
					))}
				</List>
			)}
		</>
	);
};
