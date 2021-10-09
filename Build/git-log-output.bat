chcp 65001
set LANG=ja_JP.UTF-8

echo -----------
echo %*
echo -----------

cd
git --no-pager log --pretty="format:%%H	%%cd	%%s" --date=iso "%1" > "%2"

