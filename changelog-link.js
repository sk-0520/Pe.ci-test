var targetName = "PE_BROWSER";
var issueLink = "https://github.com/sk-0520/Pe/issues/";
var revisionLink = "https://github.com/sk-0520/Pe/commit/";
// 0.84.0 以降はもうちっとまともにする
function makeChangelogLink() {
    makeAutoLink();
    makeIssueLink();
}
// import 周りを理解していないマン参上！
window.makeChangelogLink = makeChangelogLink;
function makeAutoLink() {
    var itemList = document.getElementsByTagName("li");
    for (var i = 0; i < itemList.length; i++) {
        var li = itemList[i];
        li.innerHTML = li.innerHTML.replace(/((http|https|ftp):\/\/[\w?=&./\-;#~%]+(?![\w\s?&./;#~%"=-]*>))/g, "<a href=\"$1\" href=\"".concat(targetName, "\">$1</a>")); //'"
    }
}
function makeIssueLink() {
    var itemList = document.getElementsByTagName("li");
    for (var i = 0; i < itemList.length; i++) {
        var li = itemList[i];
        var linkElements = li.getElementsByTagName("a");
        if (linkElements.length === 1 && linkElements[0].className === "revision") {
            var linkElement = linkElements[0];
            var rev = linkElement.innerHTML;
            var link = revisionLink + rev;
            linkElement.setAttribute("target", targetName);
            linkElement.setAttribute("href", link);
        }
        var text = li.innerHTML;
        text = text.replace(/#(\d+)/g, "<a href='".concat(issueLink, "$1' target='").concat(targetName, "'>#$1</a>"));
        li.innerHTML = text;
    }
}
