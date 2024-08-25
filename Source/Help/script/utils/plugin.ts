export interface PluginDefine {
	id: string;
	name: string;
}

export const ReservedPluginIds: PluginDefine[] = [
	{ id: "00000000-0000-0000-0000-000000000000", name: "Pe" },
	{
		id: "4524FC23-EBB9-4C79-A26B-8F472C05095E",
		name: "Pe.Plugins.DefaultTheme",
	},
	// ------------------------------------------
	{
		id: "67F0FA7D-52D3-4889-B595-BE3703B224EB",
		name: "Pe.Plugins.Reference.ClassicTheme",
	},
	{
		id: "2E5C72C5-270F-4B05-AFB9-C87F3966ECC5",
		name: "Pe.Plugins.Reference.Clock",
	},
	{
		id: "799CE8BD-8F49-4E8F-9E47-4D4873084081",
		name: "Pe.Plugins.Reference.Eyes",
	},
	{
		id: "9DCF441D-9F8E-494F-89C1-814678BBC42C",
		name: "Pe.Plugins.Reference.FileFinder",
	},
	{
		id: "4FA1A634-6B32-4762-8AE8-3E1CF6DF9DB1",
		name: "Pe.Plugins.Reference.Html",
	},
] as const;

function convertInnerTextFromGuid(guid: string): string {
	return guid
		.trim()
		.replace(/[{\-}\s]/g, "")
		.toLowerCase();
}

export function isGuid(guid: string): boolean {
	return /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/.test(
		guid,
	);
}

// https://stackoverflow.com/questions/105034/how-to-create-a-guid-uuid/8809472#8809472
function generateUUID(): string {
	// Public Domain/MIT
	let d = new Date().getTime(); //Timestamp
	let d2 =
		(typeof performance !== "undefined" &&
			performance.now &&
			performance.now() * 1000) ||
		0; //Time in microseconds since page-load or 0 if unsupported
	return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, (c) => {
		let r = Math.random() * 16; //random number between 0 and 16
		if (d > 0) {
			//Use timestamp until depleted
			r = ((d + r) % 16) | 0;
			d = Math.floor(d / 16);
		} else {
			//Use microseconds since page-load if supported
			r = ((d2 + r) % 16) | 0;
			d2 = Math.floor(d2 / 16);
		}
		return (c === "x" ? r : (r & 0x3) | 0x8).toString(16);
	});
}

function existsPluginId(pluginId: string): boolean {
	return (
		ReservedPluginIds.map((i) => i.id)
			.map((i) => convertInnerTextFromGuid(i))
			.filter(
				(i) =>
					convertInnerTextFromGuid(pluginId) === convertInnerTextFromGuid(i),
			).length !== 0
	);
}

export function existsPluginName(pluginName: string): boolean {
	return (
		ReservedPluginIds.map((i) => i.name)
			.map((i) => i.toLowerCase())
			.filter((i) => pluginName.toLowerCase() === i).length !== 0
	);
}

export async function generatePluginId(): Promise<string> {
	let guid = "";
	do {
		try {
			//const uri = 'http://localhost/api/plugin/generate-plugin-id';
			const uri = "https://peserver.site/api/plugin/generate-plugin-id";
			const response = await fetch(uri);
			const json = await response.json();
			guid = json.data.plugin_id;
		} catch (ex) {
			guid = generateUUID();
		}
	} while (existsPluginId(guid));

	return guid;
}

export function makeParameter(
	projectDirectory: string,
	pluginId: string,
	pluginName: string,
	projectNamespace: string,
): string {
	const items = {
		ProjectDirectory: projectDirectory,
		PluginId: pluginId,
		PluginName: pluginName,
		DefaultNamespace: projectNamespace,
	};

	const targetItems = Object.entries(items).filter(
		([_, v]) => 0 < v.trim().length,
	);
	if (targetItems.length !== Object.keys(items).length) {
		return "";
	}

	return targetItems
		.map(([k, v]) => {
			const key = `-${k}`;
			const value = v.indexOf(" ") !== -1 ? `"${v}"` : v;

			return `${key} ${value}`;
		})
		.join(" ");
}
