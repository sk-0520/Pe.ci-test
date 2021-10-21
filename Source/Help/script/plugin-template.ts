window.addEventListener('DOMContentLoaded', ev => {
	initializePluginTemplate();
});

const ReservedPluginIds = [
	{ id: '4524FC23-EBB9-4C79-A26B-8F472C05095E', name: 'ContentTypeTextNet.Pe.Plugins.DefaultTheme' },
	// ------------------------------------------
	{ id: '67F0FA7D-52D3-4889-B595-BE3703B224EB', name: 'ContentTypeTextNet.Pe.Plugins.Reference.ClassicTheme' },
	{ id: '2E5C72C5-270F-4B05-AFB9-C87F3966ECC5', name: 'ContentTypeTextNet.Pe.Plugins.Reference.Clock' },
	{ id: '799CE8BD-8F49-4E8F-9E47-4D4873084081', name: 'ContentTypeTextNet.Pe.Plugins.Reference.Eyes' },
	{ id: '9DCF441D-9F8E-494F-89C1-814678BBC42C', name: 'ContentTypeTextNet.Pe.Plugins.Reference.FileFinder' },
	{ id: '4FA1A634-6B32-4762-8AE8-3E1CF6DF9DB1', name: 'ContentTypeTextNet.Pe.Plugins.Reference.Html' },
];

const InputIds = [
	'plugin-dir',
	'plugin-id',
	'plugin-name',
	'namespace',
];

function initializePluginTemplate() {
	document.getElementById('generator')!.addEventListener('submit', ev => {
		output();
	});

	for (const id of InputIds) {
		const element = document.getElementById(id) as HTMLInputElement;
		element.addEventListener('input', ev => output());
	}
}

function output() {
	const items = Array<{ key: string, value: string }>();
	let hasError = false;

	for (const id of InputIds) {
		const element = document.getElementById(id) as HTMLInputElement;
		const value = (element.value ?? '').trim();
		switch (id) {
			case 'plugin-dir': {
				if (value.length) {
					items.push({ key: '-ProjectDirectory', value: value });
					element.setCustomValidity('');
				} else {
					hasError = true;
					element.setCustomValidity('ディレクトリ未指定');
				}
				break;
			}

			case 'plugin-id': {
				if (value.length) {
					//TODO: ID重複チェック
					items.push({ key: '-PluginId', value: value });
					element.setCustomValidity('');
				}
				break;
			}

			case 'plugin-name': {
				if (value.length) {
					//TODO: 名前重複チェック
					items.push({ key: '-PluginName', value: value });
					element.setCustomValidity('');
				} else {
					hasError = true;
					element.setCustomValidity('プラグイン名未指定');
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
				return i.key + '=' + i.value
			}).join(' ')
		} else {
			outputElement.textContent = '未設定';
		}
	}
}
