import { List, ListItemButton, Typography, useTheme } from "@mui/material";
import type { FC, MouseEvent } from "react";
import type { PageElement, PageKey } from "../../pages";

interface SideMenuItemProps {
	selectedPageKey: PageKey;
	callbackSelectPageKey: (pageKey: PageKey) => void;
	page: PageElement;
	nestLevel: number;
}

export const SideMenuItem: FC<SideMenuItemProps> = (
	props: SideMenuItemProps,
) => {
	const { callbackSelectPageKey, page, nestLevel, selectedPageKey } = props;
	const theme = useTheme();

	const isSelected = selectedPageKey === page.key;

	function handleSelectMenu(event: MouseEvent): void {
		callbackSelectPageKey(page.key);
	}

	return (
		<>
			<ListItemButton
				onClick={handleSelectMenu}
				sx={{
					background: isSelected ? theme.palette.primary.light : undefined,
					color: isSelected ? theme.palette.primary.contrastText : undefined,
					"&:hover": {
						background: isSelected ? theme.palette.primary.main : undefined,
					},
				}}
			>
				<Typography
					sx={{
						fontWeight: isSelected ? "bold" : undefined,
						paddingLeft: nestLevel * 1.5,
					}}
				>
					{page.title}
				</Typography>
			</ListItemButton>
			{page.nodes && 0 < page.nodes.length && (
				<List disablePadding>
					{page.nodes.map((a) => (
						<SideMenuItem
							key={a.key}
							selectedPageKey={props.selectedPageKey}
							callbackSelectPageKey={props.callbackSelectPageKey}
							page={a}
							nestLevel={nestLevel + 1}
						/>
					))}
				</List>
			)}
		</>
	);
};
