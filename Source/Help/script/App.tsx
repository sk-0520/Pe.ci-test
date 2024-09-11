import {
	AppBar,
	Box,
	Divider,
	Drawer,
	Toolbar,
	Typography,
} from "@mui/material";
import { useAtom } from "jotai";
import { type FC, useEffect, useState } from "react";
import { PageContent } from "./components/layouts/PageContent";
import { SideMenu } from "./components/layouts/SideMenu";
import { type PageKey, Pages } from "./pages";
import { SelectedPageKeyAtom } from "./stores/SideMenuStore";
import { getPage, getPageKey, makeUrl } from "./utils/page";

const sidebarWidth = 240;

export const App: FC = () => {
	const [isLoading, setIsLoading] = useState(true);
	const [selectedPageKey, setSelectedPageKey] = useAtom(SelectedPageKeyAtom);

	// biome-ignore lint/correctness/useExhaustiveDependencies: 初回にイベント設定
	useEffect(() => {
		const handleHistory = (pathName: URLSearchParams) => {
			try {
				const pageKey = getPageKey(pathName);
				setSelectedPageKey(pageKey);
			} catch (ex) {
				console.warn(ex);
			}
		};

		window.addEventListener(
			"popstate",
			(ev) => {
				const url = new URL(location.href);
				handleHistory(url.searchParams);
			},
			false,
		);

		// トップっぽくなければ画面遷移
		const url = new URL(location.href);
		if (url.searchParams.has("page")) {
			try {
				const pageKey = getPageKey(url.searchParams);
				setSelectedPageKey(pageKey);
			} catch (ex) {
				console.warn(ex);
			}
		}
		setIsLoading(false);
	}, []);

	// タイトル変更
	useEffect(() => {
		const page = getPage(selectedPageKey, Pages);
		document.title = `${page.title} - Pe Help`;
	}, [selectedPageKey]);

	const handleSelectPageKey = (pageKey: PageKey) => {
		setSelectedPageKey(pageKey);
		const url = makeUrl(pageKey);
		history.pushState({}, "", url);
	};

	const currentPage = getPage(selectedPageKey, Pages);

	if (isLoading) {
		return <>loading...</>;
	}

	return (
		<Box sx={{ display: "flex" }}>
			<AppBar
				id="header"
				position="fixed"
				sx={{ zIndex: (theme) => theme.zIndex.drawer + 1 }}
			>
				<Toolbar>
					<Typography variant="h6" noWrap component="h1">
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
					selectedPageKey={selectedPageKey}
					handleSelectPageKey={handleSelectPageKey}
				/>
			</Drawer>

			<Box
				component="main"
				sx={{ flexGrow: 1, bgcolor: "background.default", p: 1 }}
			>
				<Toolbar />
				<PageContent
					selectedPageKey={selectedPageKey}
					callbackSelectPageKey={handleSelectPageKey}
					currentPage={currentPage}
				/>
			</Box>
		</Box>
	);
};
