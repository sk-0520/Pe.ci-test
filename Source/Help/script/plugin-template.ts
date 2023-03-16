window.addEventListener('DOMContentLoaded', _ => {
	initializePluginTemplate();
});

const ReservedPluginIds = [
	{ id: '00000000-0000-0000-0000-000000000000', name: 'Pe' },
	{ id: '4524FC23-EBB9-4C79-A26B-8F472C05095E', name: 'Pe.Plugins.DefaultTheme' },
	// ------------------------------------------
	{ id: '67F0FA7D-52D3-4889-B595-BE3703B224EB', name: 'Pe.Plugins.Reference.ClassicTheme' },
	{ id: '2E5C72C5-270F-4B05-AFB9-C87F3966ECC5', name: 'Pe.Plugins.Reference.Clock' },
	{ id: '799CE8BD-8F49-4E8F-9E47-4D4873084081', name: 'Pe.Plugins.Reference.Eyes' },
	{ id: '9DCF441D-9F8E-494F-89C1-814678BBC42C', name: 'Pe.Plugins.Reference.FileFinder' },
	{ id: '4FA1A634-6B32-4762-8AE8-3E1CF6DF9DB1', name: 'Pe.Plugins.Reference.Html' },
];

const InputIds = [
	'plugin-dir',
	'plugin-id',
	'plugin-name',
	'namespace',
];

function initializePluginTemplate() {
	document.getElementById('generator')!.addEventListener('submit', _ => {
		output();
	});

	for (const id of InputIds) {
		const element = document.getElementById(id) as HTMLInputElement;
		element.addEventListener('input', _ => output());
	}

	document.getElementById('set-auto-plugin-id')!.addEventListener('click', _ => {
		setAutoGeneratePluginId();
	})
}

function setAutoGeneratePluginId() {
	let guid;
	do {
		// https://stackoverflow.com/questions/105034/how-to-create-a-guid-uuid/8809472#8809472
		guid = (function generateUUID() { // Public Domain/MIT
			let d = new Date().getTime();//Timestamp
			let d2 = ((typeof performance !== 'undefined') && performance.now && (performance.now() * 1000)) || 0;//Time in microseconds since page-load or 0 if unsupported
			return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
				let r = Math.random() * 16;//random number between 0 and 16
				if (d > 0) {//Use timestamp until depleted
					r = (d + r) % 16 | 0;
					d = Math.floor(d / 16);
				} else {//Use microseconds since page-load if supported
					r = (d2 + r) % 16 | 0;
					d2 = Math.floor(d2 / 16);
				}
				return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
			});
		})();
	} while (existsPluginId(guid));
	(<HTMLInputElement>document.getElementById('plugin-id')!).value = guid;
	output();
}

function convertInnerTextFromGuid(guid: string): string {
	return guid.trim().replace(/[{\-}\s]/g, '').toLowerCase()
}

function isGuid(guid: string): boolean {
	return /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/.test(guid);
}

function existsPluginId(pluginId: string): boolean {
	return ReservedPluginIds
		.map(i => i.id)
		.map(i => convertInnerTextFromGuid(i))
		.filter(i => convertInnerTextFromGuid(pluginId) == convertInnerTextFromGuid(i))
		.length != 0
		;
}

function existsPluginName(pluginName: string): boolean {
	return ReservedPluginIds
		.map(i => i.name)
		.map(i => i.toLowerCase())
		.filter(i => pluginName.toLowerCase() == i)
		.length != 0
		;

}

function output() {
	const items = Array<{ key: string, value: string }>();
	let hasError = false;


	for (const id of InputIds) {
		const element = document.getElementById(id) as HTMLInputElement;
		const value = (element.value ?? '').trim();

		const setError = (message: string) => {
			hasError = true;
			element.setCustomValidity(message);
		}

		switch (id) {
			case 'plugin-dir': {
				if (value.length) {
					items.push({ key: '-ProjectDirectory', value: value });
					element.setCustomValidity('');
				} else {
					setError('ディレクトリ未指定');
				}
				break;
			}

			case 'plugin-id': {
				if (value.length) {
					if (!isGuid(value)) {
						setError('GUID不正');
					} else if (existsPluginId(value)) {
						setError('予約済みプラグインID');
					} else {
						items.push({ key: '-PluginId', value: value });
						element.setCustomValidity('');
					}
				}
				break;
			}

			case 'plugin-name': {
				if (value.length) {
					if (existsPluginName(value)) {
						setError('予約済みプラグイン名');
					} else {
						items.push({ key: '-PluginName', value: value });
						element.setCustomValidity('');
					}
				} else {
					setError('プラグイン名未指定');
				}
				break;
			}

			case 'namespace': {
				if (value.length) {
					items.push({ key: '-DefaultNamespace', value: value });
					element.setCustomValidity('');
				}
				break;
			}

			default:
				throw new Error(id);
		}
	}

	const outputElement = document.getElementById('output')!;
	if (hasError) {
		outputElement.textContent = 'エラーあり';
	} else {
		if (items.length) {
			outputElement.textContent = ".\\create-project.ps1 " + items.map(i => {
				return i.key + ' ' + (function () {
					if (i.value.indexOf(' ') != -1) {
						return `"${i.value}"`;
					} else {
						return i.value;
					}
				})();
			}).join(' ')
		} else {
			outputElement.textContent = '未設定';
		}
	}
}
