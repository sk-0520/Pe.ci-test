// Pe から引数を追加して強制実行処理
(function (styleSheet) {
	var callbacksTask = CefSharp.BindObjectAsync("pe_callbacks");
	var extensionsTask = CefSharp.BindObjectAsync("pe_extensions");

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

	Promise.all([callbacksTask, extensionsTask]).then(values => {
		window.Pe = {
			callbacks: pe_callbacks,
			extensions: pe_extensions
		};

		window.Pe.callbacks.completeInjection();
	})
})
