/**
最新のバージョンを抜き出す。
*/
var adSaveCreateOverWrite = 2;
var adWriteChar = 0;

var loadPath = "Pe\\PeMain\\doc\\changelog.js";
var scriptPath = "Pe\\PeMain\\etc\\script\\changelog.js";
var styleCommonPath = "Pe\\PeMain\\etc\\style\\common.css";
var styleChangelogPath = "Pe\\PeMain\\etc\\style\\changelog.css";
var saveRcPath = "Changelog\\update-rc.html";
var saveReleasePath = "Changelog\\update-release.html";

var typeMap = {
	'features':  '機能',
	'fixes':     '修正',
	'developer': '開発',
	'note':      'メモ'
};

//var xml = WScript.CreateObject('MSXML.DOMDocument');
//xml.load(loadPath);

var a = function() {
	var stream = createStream();
	stream.LoadFromFile(loadPath);
	return stream.ReadText;
}();
eval(a);


var updateVersions = [
	{
		isRc: true,
		path: saveRcPath
	},
	{
		isRc: false,
		path: saveReleasePath
	}
];

function isRc(update)
{
	return update.type == 'rc';
}

function isRcElement(node)
{
	return node.getAttribute('type') == 'rc';
}

function createStream()
{
	var stream = WScript.CreateObject('ADODB.Stream');
	stream.Mode = 3;
	stream.Type = 2;
	stream.Charset = 'UTF-8';
	stream.Open();

	return stream;
}

function writeLine(stream, s)
{
	stream.WriteText(s);
	stream.WriteText("\r\n");
}

function writeHead(stream)
{
	var scriptStream = createStream();
	scriptStream.LoadFromFile(scriptPath);
	var scriptText = scriptStream.ReadText;
	
	var styleCommonStream = createStream();
	styleCommonStream.LoadFromFile(styleCommonPath);
	var styleCommonText = styleCommonStream.ReadText;
	
	var styleChangelogStream = createStream();
	styleChangelogStream.LoadFromFile(styleChangelogPath);
	var styleChangelogText = styleChangelogStream.ReadText;
	
	writeLine(stream, "<!DOCTYPE html>\r\n");
	writeLine(stream, '<html>');
	writeLine(stream, '<head>');
	writeLine(stream, '<meta charset="utf-8">');
	writeLine(stream, '<title>Pe Update: 最新バージョン更新情報</title>');
	writeLine(stream, '<script>');
	writeLine(stream, scriptText);
	writeLine(stream, 'window.onload = function() { makeIssueLink(); }');
	writeLine(stream, '</script>');
	writeLine(stream, '<style>');
	writeLine(stream, styleCommonText);
	writeLine(stream, '</style>');
	writeLine(stream, '<style>');
	writeLine(stream, styleChangelogText);
	writeLine(stream, '</style>');
	writeLine(stream, '</head>');
	writeLine(stream, '<body>');
}

function writeFoot(stream)
{
	writeLine(stream, '</body>');
	writeLine(stream, '</html>');
}

function writeTitle(stream, log)
{
	var version = log['version'];
	var date    = log['date'];
	var title = '';
	if(log.isRc) {
		title =  date + ', ' + version + ' <em>RC版</em>';
	} else {
		title = date + ', ' + version;
	}
	writeLine(stream, '<h2>' + title + '</h2>');
}

function writeBlock(stream, log)
{
	writeLine(stream, '<dl class="changelog">');
	var groups = log['contents']
	for(var i = 0; i < groups.length; i++) {
		var group = groups[i];
		var type = group['type'];

		var notes = group['logs'];
		var list = [];
		for(var noteIndex = 0; noteIndex < notes.length; noteIndex++) {
			//var s = notes[noteIndex].text;
			var note = notes[noteIndex];
			if(note['subject'] && 0 < note['subject'].length) {
				list.push({
					className: note['class'],
					body: note['subject'],
					comments: function() {
						var result = [];
						var commentElements = note['comments'];
						if(commentElements) {
							for(var commentIndex = 0; commentIndex < commentElements.length; commentIndex++) {
								var comment = commentElements[commentIndex];
								if(comment.length) {
									result.push(comment);
								}
							}
						}
						return result;
					}(),
					rev: function() {
						var rev = note.revision;
						if(rev) {
							return '<a class="rev">' + rev + '</a>'
						}
						return '';
					}()
				});
			}
		}
		if(list.length) {
			writeLine(stream, "<dt class='" + type + "'>" + typeMap[type] + '</dt>');
			writeLine(stream, "<dd class='" + type + "'>");
			writeLine(stream, '<ul>');
			
			for(var listIndex = 0; listIndex < list.length; listIndex++) {
				//var text = list[listIndex].replace(/#([0-9]+)/g, '<a href="' + issueLink + '$1">#$1</a>');
				var item = list[listIndex];
				
				var listHead;
				if(item.className) {
					listHead = '<li class="' + item.className + '">';
				} else {
					listHead = '<li>';
				}
				writeLine(stream, listHead);
				writeLine(stream, item.body + '。' + item.rev);
				if(item.comments && item.comments.length) {
					writeLine(stream, '<ul class="comment">');
					for(var commentIndex = 0; commentIndex < item.comments.length; commentIndex++) {
						writeLine(stream, '<li>' + item.comments[commentIndex] + '。</li>');
					}
					writeLine(stream, '</ul>');
				}
				writeLine(stream, '</li>');
			}
			
			writeLine(stream, '</ul>');
			writeLine(stream, '</dd>');
		}
	}
	
	writeLine(stream, '</dl>');
}

//個別履歴出力
for(var i = 0; i < updateVersions.length; i++) {
	var update = updateVersions[i];

	var stream = createStream();
	writeHead(stream);

	var targetLog = null;
	for(var j = 0; j < changelogs.length; j++) {
		if(changelogs[j].isRc) {
			if(update.isRc) {
				targetLog = changelogs[j];
				break;
			}
		} else if(!update.isRc) {
			targetLog = changelogs[j];
			break;
		}
	}
	if(!targetLog) {
		continue;
	}

	writeTitle(stream, targetLog)

	writeBlock(stream, targetLog);
	
	writeFoot(stream);

	stream.SaveToFile(update.path, adSaveCreateOverWrite);
	stream.Close();
}



