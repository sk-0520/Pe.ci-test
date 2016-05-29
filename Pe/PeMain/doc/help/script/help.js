// 言語設定は一応考えるけど今のところ ja-JP で動けばそれでいい。テストもしてない。

var defaultLanguageKey = 'ja-JP';

var helpLanguage = {
	'ja-JP': { 
		title: ' : Pe ヘルプ',
		outLink: '外部リンク',
		hint: {
			icon: '📝',
			text: 'ヒント',
		},
		warning: {
			icon: '⚠',
			text: '注意'
		},
		bug: {
			icon: '🐞',
			text: 'バグ'
		},
		ref: {
			icon: '🔗',
			text: '参照'
		}
	}
};

var menuList = [
	{
		name: 'top',
		localize: true,
		level: 0,
		title: {
			'ja-JP': 'はじめに'
		}
	},
	{
		name: 'install_uninstall_data',
		localize: true,
		level: 1,
		title: {
			'ja-JP': 'インストール・アンインストール・保存データについて'
		}
	},
	{
		name: 'platform',
		localize: true,
		level: 2,
		title: {
			'ja-JP': '32/64bitについて'
		}
	},
	{
		name: 'privacy',
		localize: true,
		level: 1,
		title: {
			'ja-JP': 'ユーザー情報について'
		}
	},
	{
		name: 'internet',
		localize: true,
		level: 1,
		title: {
			'ja-JP': 'インターネット通信とその内容'
		}
	},
	{
		name: 'notifyarea',
		localize: true,
		level: 0,
		title: {
			'ja-JP': '通知領域'
		}
	},
	{
		name: 'launcher',
		localize: true,
		level: 0,
		title: {
			'ja-JP': 'ランチャー'
		}
	},
	{
		name: 'toolbar',
		localize: true,
		level: 1,
		title: {
			'ja-JP': 'ツールバー'
		}
	},
	{
		name: 'command',
		localize: true,
		level: 1,
		title: {
			'ja-JP': 'コマンド'
		}
	},
	{
		name: 'execute',
		localize: true,
		level: 1,
		title: {
			'ja-JP': '指定して実行'
		}
	},
	{
		name: 'stream',
		localize: true,
		level: 1,
		title: {
			'ja-JP': '標準入出力'
		}
	},
	{
		name: 'note',
		localize: true,
		level: 0,
		title: {
			'ja-JP': 'ノート'
		}
	},
	{
		name: 'clipboard',
		localize: true,
		level: 0,
		title: {
			'ja-JP': 'クリップボード'
		}
	},
	{
		name: 'template',
		localize: true,
		level: 0,
		title: {
			'ja-JP': 'テンプレート'
		}
	},
	{
		name: 'log',
		localize: true,
		level: 0,
		title: {
			'ja-JP': 'ログ'
		}
	},
	{
		name: 'setting',
		localize: true,
		level: 0,
		title: {
			'ja-JP': '設定'
		}
	},
	{
		name: 'setting-general',
		localize: true,
		level: 1,
		title: {
			'ja-JP': '本体'
		}
	},
	{
		name: 'setting-launcher',
		localize: true,
		level: 1,
		title: {
			'ja-JP': 'ランチャー'
		}
	},
	{
		name: 'setting-toolbar',
		localize: true,
		level: 1,
		title: {
			'ja-JP': 'ツールバー'
		}
	},
	{
		name: 'setting-command',
		localize: true,
		level: 1,
		title: {
			'ja-JP': 'コマンド'
		}
	},
	{
		name: 'setting-note',
		localize: true,
		level: 1,
		title: {
			'ja-JP': 'ノート'
		}
	},
	{
		name: 'setting-clipboard',
		localize: true,
		level: 1,
		title: {
			'ja-JP': 'クリップボード'
		}
	},
	{
		name: 'setting-template',
		localize: true,
		level: 1,
		title: {
			'ja-JP': 'テンプレート'
		}
	},
	{
		name: '*others',
		localize: true,
		level: 0,
		title: {
			'ja-JP': 'その他'
		}
	},
	{
		name: 'others-item-file-dd',
		localize: true,
		level: 1,
		title: {
			'ja-JP': 'ファイルのD&D'
		}
	},
	{
		name: 'others-item-filtering',
		localize: true,
		level: 1,
		title: {
			'ja-JP': 'フィルタリング'
		}
	},
	{
		name: 'others-icon-size',
		localize: true,
		level: 1,
		title: {
			'ja-JP': 'アイコンサイズ'
		}
	},
	{
		name: 'others-launcher-item-editor',
		localize: true,
		level: 1,
		title: {
			'ja-JP': 'ランチャーアイテム編集'
		}
	},
	{
		name: 'others-environment-variables-editor',
		localize: true,
		level: 1,
		title: {
			'ja-JP': '環境変数編集'
		}
	},
	{
		name: 'others-setting-data',
		localize: true,
		level: 1,
		title: {
			'ja-JP': '設定データ'
		}
	},
	{
		name: 'others-setting-data-app-config',
		localize: true,
		level: 2,
		title: {
			'ja-JP': '構成ファイル'
		}
	},
	{
		name: 'development',
		localize: true,
		level: 1,
		title: {
			'ja-JP': '開発について'
		}
	},
	{
		name: 'development-source',
		localize: true,
		level: 2,
		title: {
			'ja-JP': 'ソースコードについて'
		}
	},
	{
		name: 'changelog',
		localize: false,
		level: 0,
		title: {
			'ja-JP': '更新履歴'
		}
	}
	//
];
//----------------------------------------------------------------------


// http://stackoverflow.com/questions/19491336/get-url-parameter-jquery?answertab=votes#tab-top
function getParameter(key)
{
	var pageUri = decodeURIComponent(window.location.search.substring(1));
	var params = pageUri.split('&');

	for (var i = 0; i < params.length; i++) {
		var paramNames = params[i].split('=');

		if (paramNames[0] === key) {
			return paramNames[1] === undefined ? true : paramNames[1];
		}
	}
}

function getLanguageCode() {
	var lang = getParameter('lang');
	if (!lang || typeof lang !== 'string') {
		return defaultLanguageKey;
	}

	return lang;
}

function getPageName() {
	var pageUri = location.pathname.substring(1)
	var index = pageUri.lastIndexOf('/');
	var fileName = pageUri.substring(index + 1);
	return fileName.split('.')[0];
}

function getPageTitle(lang, menuItem) {
	var title = menuItem.title[lang];
	if (title) {
		return title;
	}

	return menuItem.title[defaultLanguageKey];
}

function createMenu(lang, pageName) {
	var $menu = $('#main-menu');

	var param = 'lang=' + lang;
	var $ul = $('<ul>');
	var $top = null;
	for (var i = 0 ; i < menuList.length; i++) {
		var menuItem = menuList[i];

		var $li = $('<li>');

		$li.addClass('item');
		$li.addClass('level-' + menuItem.level);
		
		var title = getPageTitle(lang, menuItem);
		var target = menuItem.name + (menuItem.localize ? '.' + lang: '') + '.html?' + param;
		if (menuItem.name == pageName) {
			$top = $li;
			$li.text(title);
			$('h1').text(title);
			$('title').text(title + helpLanguage[lang].title);
			$li.addClass('level-active');
		} else if (menuItem.name.charAt(0) == '*') {
			$li.text(title);
		} else {
			var $link = $('<a>');
			$link.text(title);
			$link.attr('href', target);
			$li.append($link);
		}

		$ul.append($li);
	}
	$menu.append($ul);
	
	if ($top) {
		if ($menu.height() < $top.offset().top) {
			var y = $top.offset().top - $menu.offset().top;
			$menu.scrollTop(y);
		}
	}
}

function createLink(lang) {
	var param = 'lang=' + lang;

	var $content = $('#content');
	$content.find('.page').each(function() {
		var $page = $(this);
		// TODO: 内部リンク
		var pageName = $page.text();
		for (var i = 0; i < menuList.length; i++) {
			var menuItem = menuList[i];
			if (menuItem.name == pageName) {
				var title = getPageTitle(lang, menuItem);
				var target = menuItem.name + '.' + lang + '.html?' + param;
				var $link = $('<a>');
				$link.text(title);
				$link.attr('href', target);
				$page.empty().append($link);
				break;
			}
		}
	});
	$content.find('.issue').each(function() {
		var $issue = $(this);
		var $link = $('<a>');
		var number = $issue.text();
		var target = issueLink + number;
		$link.text(number);
		$link.attr('href', target);
		$issue.empty().append($link);
	});
	$content.find('a').each(function() {
		var $link = $(this);
		var uri = $link.attr('href');
		if (uri.match(/https?:\/\//)) {
			var help = helpLanguage[lang];
			var $out = $('<span>')
				.text(help.outLink)
				.addClass('out-link')
			;
			$link.append($out);
		}
	});
}

function createComment(lang) {
	var $content = $('#content');
	var help = helpLanguage[lang];
	$content.find('.hint, .warning, .bug, .ref').each(function() {
		var $element = $(this);
		var className = $element.attr('class');
		var comment = help[className];
		var $title = $('<em>').addClass('title');
		var $icon = $('<span>').addClass('icon').text(comment.icon);
		var $text = $('<span>').addClass('text').text(comment.text);

		$title.append($icon);
		$title.append($text);
		$element.prepend($title);
	});
}

function setPadding() {
	var $content = $('#content');
	$content.append($('<div>').addClass('padding'));
}

$(function() {
	var lang = getLanguageCode();
	var pageName = getPageName();
	if (pageName == 'help') {
		return;
	}

	createMenu(lang, pageName);
	createLink(lang);
	createComment(lang);
	setPadding();
});

