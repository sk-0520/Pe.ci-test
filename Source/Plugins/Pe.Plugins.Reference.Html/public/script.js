
async function sampleSubmit(e) {
	e.preventDefault();

	const value = document.getElementById('to-plugin-value').value;
	const result = await Pe.extensions.sampleCallback(value);
	document.getElementById('from-plugin-value').textContent = result;
}

function receiveSample(timestamp, memory) {
	var element = document.getElementById('from-plugin-data');
	element.textContent = `[DATA]
timestamp: ${timestamp}
memory: ${memory}
`
}
