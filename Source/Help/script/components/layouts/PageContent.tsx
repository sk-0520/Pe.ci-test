import type { FC, ReactNode } from "react";
import type { PageKey } from "../../pages";
import { DevIndexPage } from "../../pages/dev";
import {
	HelpCpuPage,
	HelpHistoryPage,
	HelpIndexPage,
	HelpInstallUninstallDataPage,
	HelpLauncherCommandPage,
	HelpLauncherExtendsExecutePage,
	HelpLauncherPage,
	HelpLauncherToolbarPage,
	HelpNotePage,
	HelpNotifyAreaPage,
	HelpOthersAppSettingsPage,
	HelpOthersCommandLinePage,
	HelpOthersPage,
	HelpOthersPluginPage,
	HelpOthersProxyPage,
	HelpPrivacyPage,
	HelpSearchPage,
} from "../../pages/help";

import { Typography } from "@mui/material";
import type { PageProps } from "../../types/PageProps";

interface PageContentProps extends PageProps {}

class NotImplError extends Error {}

const pageFactory: { [key in PageKey]: (props: PageProps) => ReactNode } = {
	"help.index": (props: PageProps) => <HelpIndexPage {...props} />,
	"help.install_uninstall_data": (props: PageProps) => (
		<HelpInstallUninstallDataPage {...props} />
	),
	"help.cpu": (props: PageProps) => <HelpCpuPage {...props} />,
	"help.privacy": (props: PageProps) => <HelpPrivacyPage {...props} />,
	"help.notify_area": (props: PageProps) => <HelpNotifyAreaPage {...props} />,
	"help.launcher": (props: PageProps) => <HelpLauncherPage {...props} />,
	"help.launcher_toolbar": (props: PageProps) => (
		<HelpLauncherToolbarPage {...props} />
	),
	"help.launcher_command": (props: PageProps) => (
		<HelpLauncherCommandPage {...props} />
	),
	"help.launcher_extends_execute": (props: PageProps) => (
		<HelpLauncherExtendsExecutePage {...props} />
	),
	"help.note": (props: PageProps) => <HelpNotePage {...props} />,
	"help.others": (props: PageProps) => <HelpOthersPage {...props} />,
	"help.others_appsettings": (props: PageProps) => (
		<HelpOthersAppSettingsPage {...props} />
	),
	"help.others_commandline": (props: PageProps) => (
		<HelpOthersCommandLinePage {...props} />
	),
	"help.others_proxy": (props: PageProps) => <HelpOthersProxyPage {...props} />,
	"help.others_plugin": (props: PageProps) => (
		<HelpOthersPluginPage {...props} />
	),
	"help.search": (props: PageProps) => <HelpSearchPage {...props} />,
	"help.changelogs": (props: PageProps) => <HelpHistoryPage {...props} />,
	"dev.index": (props: PageProps) => <DevIndexPage {...props} />,
	"dev.build": (props: PageProps) => {
		throw new NotImplError("not impl");
	},
	"dev.branch": (props: PageProps) => {
		throw new NotImplError("not impl");
	},
	"dev.ci": (props: PageProps) => {
		throw new NotImplError("not impl");
	},
	"dev.table_main": (props: PageProps) => {
		throw new NotImplError("not impl");
	},
	"dev.table_large": (props: PageProps) => {
		throw new NotImplError("not impl");
	},
	"dev.table_temporary": (props: PageProps) => {
		throw new NotImplError("not impl");
	},
	"dev.plugin": (props: PageProps) => {
		throw new NotImplError("not impl");
	},
	"dev.plugin_reference": (props: PageProps) => {
		throw new NotImplError("not impl");
	},
	"dev.plugin_template": (props: PageProps) => {
		throw new NotImplError("not impl");
	},
} as const;

export const PageContent: FC<PageContentProps> = (props: PageContentProps) => {
	const { selectedPageKey } = props;

	const factory = pageFactory[selectedPageKey];
	try {
		const page = factory(props);
		return page;
	} catch (ex: unknown) {
		if (ex instanceof NotImplError) {
			return (
				<Typography color="error">
					Not Impl: <strong>{selectedPageKey}</strong>
				</Typography>
			);
		}
		throw ex;
	}
};
