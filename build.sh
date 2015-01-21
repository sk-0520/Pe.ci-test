
VERSION_PATH='Pe/PeMain/Properties/AssemblyInfo.cs'

if [ `git status -s | wc -l` -ne 0 ] ; then
    git status
    echo "change file. please any key... "
    read
    exit 1
fi

# バージョン書き換え
VERSION_REV=`git rev-parse HEAD`
sed -E -i "s/^\[\s*assembly\s*:\s*\AssemblyInformationalVersion\s*\(\s*\"\s*(revision)\s*\"\s*\)\s*\]/[assembly: AssemblyInformationalVersion(\"$VERSION_REV\")]/" $VERSION_PATH
# <YEAR>書き換え
find -name 'AssemblyInfo.cs' -print0 | xargs -0 sed -E -i "s/<YEAR>/`date +%Y`/"

# ビルド
pushd Build
    cmd.exe //c build.bat
popd

# バージョン戻し
git reset --hard

if [ -z "${CI+x}" ] ; then
    echo "build success. please any key..."
    read
else
    echo "build success. CI mode."
fi

