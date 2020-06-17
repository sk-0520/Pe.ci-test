
const Pe_injectionStyleId = 'pe_injection_style';
var injectionStyleElement = document.getElementById(Pe_injectionStyleId);
if (injectionStyleElement == null) {
	injectionStyleElement = document.createElement('style');
	injectionStyleElement.innerHTML = `
		.PE-MOVE-AREA {
			border: 1px solid lime;
			cursor: move;
		}

		.PE-RESIZE-AREA {
			border: 1px solid red;
			cursor: nwse-resize;
		}
		.PE-RESIZE-AREA[data-pe-resize='ew'] {
			cursor: ew-resize;
		}
		.PE-RESIZE-AREA[data-pe-resize='ns'] {
			cursor: ns-resize;
		}
		.PE-RESIZE-AREA[data-pe-resize='nesw'] {
			cursor: nesw-resize;
		}
		.PE-RESIZE-AREA[data-pe-resize='nwse'] {
			cursor: nwse-resize;
		}
	`;
	injectionStyleElement.setAttribute('id', injectionStyleElement);
	document.body.appendChild(injectionStyleElement);
}


const moveAreaElements = document.querySelectorAll('.PE-MOVE-AREA');
const resizeAreaElements = document.querySelectorAll('.PE-RESIZE-AREA');
