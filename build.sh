
VERSION_PATH='Source/PeMain/Properties/AssemblyInfo.cs'

# CIで止まられるとなんもできん
if [ -z "${CI+x}" ] ; then
    if [ `git status -s | wc -l` -ne 0 ] ; then
        git status
        echo "There is changed files. Press Any key to exit ... "
        read
        exit 1
    fi
else
    echo "[CI] skip: status check"
fi

# バージョン書き換え
VERSION_REV=`git rev-parse HEAD`
sed -E -i "s/^\[\s*assembly\s*:\s*\AssemblyInformationalVersion\s*\(\s*\"\s*(xxxxxxxxxxxxxxDEVELOPMENTxxxxxxxxxxxxxxx)\s*\"\s*\)\s*\]/[assembly: AssemblyInformationalVersion(\"$VERSION_REV\")]/" $VERSION_PATH
# <YEAR>書き換え
find -name 'AssemblyInfo.cs' -print0 | xargs -0 sed -E -i "s/<YEAR>/`date +%Y`/"

# ビルド
pushd Build
    cmd.exe //c build.bat ${BUILD_TYPE} ${BUILD_OUTPUT}
popd

# バージョン戻し
git reset --hard

echo ""
if [ -f Build/@error ] ; then
    if [ -z "${CI+x}" ] ; then
        echo "!!!build failed!!! Press Any key to exit ..."
        read
    else
        echo "!!!build failed!!! CI mode."
    fi
else
    if [ -z "${CI+x}" ] ; then
        echo "build success. Press Any key to exit ..."
        read
    else
        echo "build success. CI mode."
    fi
fi

