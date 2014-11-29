
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

# ビルド
cmd.exe //c  build.bat

# バージョン戻し
git reset --hard

echo "build success. please any key... "
read

