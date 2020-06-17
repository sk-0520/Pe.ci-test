// Pe から引数を追加して強制実行処理
(function(styleSheet) {
	const injectionStyleId = 'Pe_injection-style';

	var injectionStyleElement = document.getElementById(injectionStyleId);
	if (injectionStyleElement == null) {
		injectionStyleElement = document.createElement('style');
		injectionStyleElement.innerHTML = styleSheet;
		injectionStyleElement.setAttribute('id', injectionStyleId);
		document.body.appendChild(injectionStyleElement);
	}


	const moveAreaElements = document.querySelectorAll('.Pe_move-area');
	const resizeAreaElements = document.querySelectorAll('.Pe_resize-area');
})
