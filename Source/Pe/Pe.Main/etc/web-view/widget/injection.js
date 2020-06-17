// Pe から引数を追加して強制実行処理
(async function (styleSheet) {
	await CefSharp.BindObjectAsync("pe_callbacks");
	window.pe_callbacks = pe_callbacks;

	await CefSharp.BindObjectAsync("pe_extensions");
	window.pe_extensions = pe_extensions;

	var x = await pe_extensions.func(2,2);
	alert(x);

	/*async*/ function moveStartAsync(e) {
		if (e.button !== 0) {
			return;
		}

		e.preventDefault();

		//await CefSharp.BindObjectAsync("pe_callbacks");

		pe_callbacks.moveStart();
	}

	/*async*/ function resizeStartAsync(e) {
		if (e.button !== 0) {
			return;
		}

		e.preventDefault();

		//await CefSharp.BindObjectAsync("pe_callbacks");

		pe_callbacks.resizeStart(e.currentTarget.dataset.pe_resize);
	}

	const injectionStyleId = 'pe_injection-style';

	var injectionStyleElement = document.getElementById(injectionStyleId);
	if (injectionStyleElement == null) {
		injectionStyleElement = document.createElement('style');
		injectionStyleElement.innerHTML = styleSheet;
		injectionStyleElement.setAttribute('id', injectionStyleId);
		document.body.appendChild(injectionStyleElement);
	}


	const moveAreaElements = document.querySelectorAll('.pe_move-area');
	for (const moveAreaElement of moveAreaElements) {
		moveAreaElement.addEventListener('mousedown', moveStartAsync);
	}

	const resizeAreaElements = document.querySelectorAll('.pe_resize-area');
	for (const resizeAreaElement of resizeAreaElements) {
		resizeAreaElement.addEventListener('mousedown', resizeStartAsync);
	}
})
