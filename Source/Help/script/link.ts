window.addEventListener("DOMContentLoaded", (_) => {
	const anchorElements = document.getElementsByTagName("a");
	for (const anchorElement of anchorElements) {
		const url = new URL(anchorElement.href);
		if (
			url.protocol.startsWith("http") &&
			!url.hostname.startsWith("localhost") &&
			anchorElement.target === ""
		) {
			anchorElement.target = "_blank";
		}
	}
});
