window.addEventListener('DOMContentLoaded', ev => {
    const anchorElements = document.getElementsByTagName('a');
    for (const anchorElement of anchorElements) {
        if (anchorElement.href.startsWith('http') && anchorElement.target == '') {
            // https も引っかかるのでOKよ
            anchorElement.target = '_blank';
        }
    }
});
