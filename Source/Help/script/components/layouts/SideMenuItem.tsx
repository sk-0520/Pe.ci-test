import { List, ListItemButton, Typography } from "@mui/material";
import type { FC, MouseEvent } from "react";
import type { PageElement, PageKey } from "../../pages";

interface SideMenuItemProps {
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
			<ListItemButton onClick={handleSelectMenu}>
				<Typography paddingLeft={nestLevel * 1.5}>{page.title}</Typography>
			</ListItemButton>
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
