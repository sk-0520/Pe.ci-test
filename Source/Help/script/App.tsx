import {
	AppBar,
	Box,
	Divider,
	Drawer,
	ThemeProvider,
	Toolbar,
	Typography,
	createTheme,
} from "@mui/material";
import { useAtom } from "jotai";
import { type FC, useEffect } from "react";
import { PageContent } from "./components/layouts/PageContent";
import { SideMenu } from "./components/layouts/SideMenu";
import { type PageKey, PageKeys, Pages } from "./pages";
import { SideMenuStoreAtom } from "./stores/SideMenuStore";
import { convertPathToPageKey, getPage } from "./utils/page";
import { trim } from "./utils/string";

const sidebarWidth = 240;

export const App: FC = () => {
	const [sideMenuStoreAtom, setSideMenuStoreAtom] = useAtom(SideMenuStoreAtom);

	// biome-ignore lint/correctness/useExhaustiveDependencies: 初回にイベント設定
	useEffect(() => {
		const handleHistory = (pathName: string) => {
			try {
				const pageKey = convertPathToPageKey(pathName);
				setSideMenuStoreAtom({
					...sideMenuStoreAtom,
					selectedPageKey: pageKey,
				});
			} catch (ex) {
				console.warn(ex);
			}
		};

		window.addEventListener(
			"popstate",
			(ev) => {
				handleHistory(location.pathname);
			},
			false,
		);

		// トップっぽくなければ画面遷移
		if (location.pathname !== "/") {
			try {
				const pageKey = convertPathToPageKey(location.pathname);
				setSideMenuStoreAtom({
					...sideMenuStoreAtom,
					selectedPageKey: pageKey,
				});
			} catch (ex) {
				console.warn(ex);
			}
		}
	}, []);

	const handleSelectPageKey = (pageKey: PageKey) => {
		setSideMenuStoreAtom({
			...sideMenuStoreAtom,
			selectedPageKey: pageKey,
		});
		//TODO: ローカルファイルはクエリじゃないと無理そう
		history.pushState({}, "", pageKey);
	};

	const currentPage = getPage(sideMenuStoreAtom.selectedPageKey, Pages);

	return (
		<Box sx={{ display: "flex" }}>
			<AppBar
				position="fixed"
				sx={{ zIndex: (theme) => theme.zIndex.drawer + 1 }}
			>
				<Toolbar>
					<Typography variant="h6" noWrap component="div">
						{currentPage.title}
					</Typography>
				</Toolbar>
			</AppBar>
			<Drawer
				sx={{
					width: sidebarWidth,
					flexShrink: 0,
					"& .MuiDrawer-paper": {
						width: sidebarWidth,
						boxSizing: "border-box",
					},
				}}
				variant="permanent"
				anchor="left"
			>
				<Toolbar />
				<Divider />
				<SideMenu
					selectedPageKey={sideMenuStoreAtom.selectedPageKey}
					handleSelectPageKey={handleSelectPageKey}
				/>
			</Drawer>

			<Box
				component="main"
				sx={{ flexGrow: 1, bgcolor: "background.default", p: 1 }}
			>
				<Toolbar />
				<PageContent
					selectedPageKey={sideMenuStoreAtom.selectedPageKey}
					handleSelectPageKey={handleSelectPageKey}
					currentPage={currentPage}
				/>
			</Box>
		</Box>
	);
};
