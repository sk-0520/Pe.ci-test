
VERSION_PATH='Pe/PeMain/Properties/AssemblyInfo.cs'

# バージョン書き換え
#[assembly: AssemblyVersion("0.33.9.*")]
VERSION_REV=`git rev-parse HEAD`
sed -E -i "s/^\[\s*assembly\s*:\s*\AssemblyInformationalVersion\s*\(\s*\"\s*(revision)\s*\"\s*\)\s*\]/[assembly: AssemblyInformationalVersion(\"$VERSION_REV\")]/" $VERSION_PATH

# ビルド
cmd.exe //c  build.bat

# バージョン戻し
git reset --hard

read a

