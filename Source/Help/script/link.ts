window.addEventListener('DOMContentLoaded', ev => {
	const anchorElements = document.getElementsByTagName('a');
	for (const anchorElement of anchorElements) {
		const url = new URL(anchorElement.href);
		if (!url.hostname.startsWith('localhost') && anchorElement.target == '') {
			anchorElement.target = '_blank';
		}
	}
});
