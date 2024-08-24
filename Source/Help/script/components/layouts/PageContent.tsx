import { type FC, type MouseEvent, type ReactNode, useEffect } from "react";
import { type PageElement, type PageKey, PageKeys, Pages } from "../../pages";
import { HelpCpuPage } from "../../pages/help/HelpCpuPage";
import { HelpIndexPage } from "../../pages/help/HelpIndexPage";
import { HelpInstallUninstallDataPage } from "../../pages/help/HelpInstallUninstallDataPage";
import { HelpPrivacyPage } from "../../pages/help/HelpPrivacyPage";
import type { PageProps } from "../../types/PageProps";
import { getPage } from "../../utils/page";

interface PageContentProps extends PageProps {}

const pageFactory: { [key in PageKey]: (props: PageProps) => ReactNode } = {
	"help.index": (props: PageProps) => <HelpIndexPage {...props} />,
	"help.install_uninstall_data": (props: PageProps) => (
		<HelpInstallUninstallDataPage {...props} />
	),
	"help.cpu": (props: PageProps) => <HelpCpuPage {...props} />,
	"help.privacy": (props: PageProps) => <HelpPrivacyPage {...props} />,
	"help.notifyarea": (props: PageProps) => {
		throw new Error("not impl");
	},
	"help.launcher": (props: PageProps) => {
		throw new Error("not impl");
	},
	"help.launcher_toolbar": (props: PageProps) => {
		throw new Error("not impl");
	},
	"help.launcher_command": (props: PageProps) => {
		throw new Error("not impl");
	},
	"help.launcher_extends_execute": (props: PageProps) => {
		throw new Error("not impl");
	},
	"help.note": (props: PageProps) => {
		throw new Error("not impl");
	},
	"help.others": (props: PageProps) => {
		throw new Error("not impl");
	},
	"help.others_appsettings": (props: PageProps) => {
		throw new Error("not impl");
	},
	"help.others_commandline": (props: PageProps) => {
		throw new Error("not impl");
	},
	"help.others_proxy": (props: PageProps) => {
		throw new Error("not impl");
	},
	"help.others_plugin": (props: PageProps) => {
		throw new Error("not impl");
	},
	"help.search": (props: PageProps) => {
		throw new Error("not impl");
	},
	"help.changelogs": (props: PageProps) => {
		throw new Error("not impl");
	},
	"dev.index": (props: PageProps) => {
		throw new Error("not impl");
	},
	"dev.build": (props: PageProps) => {
		throw new Error("not impl");
	},
	"dev.branch": (props: PageProps) => {
		throw new Error("not impl");
	},
	"dev.ci": (props: PageProps) => {
		throw new Error("not impl");
	},
	"dev.table_main": (props: PageProps) => {
		throw new Error("not impl");
	},
	"dev.table_large": (props: PageProps) => {
		throw new Error("not impl");
	},
	"dev.table_temporary": (props: PageProps) => {
		throw new Error("not impl");
	},
	"dev.plugin": (props: PageProps) => {
		throw new Error("not impl");
	},
	"dev.plugin_reference": (props: PageProps) => {
		throw new Error("not impl");
	},
	"dev.plugin_template": (props: PageProps) => {
		throw new Error("not impl");
	},
} as const;

export const PageContent: FC<PageContentProps> = (props: PageContentProps) => {
	const { selectedPageKey } = props;

	const factory = pageFactory[selectedPageKey];
	const page = factory(props);

	return page;
};
