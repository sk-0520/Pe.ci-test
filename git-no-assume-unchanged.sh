
git ls-files -v | grep '^[[:lower:]]'
for FILEPATH in `git ls-files -v | grep '^[[:lower:]]' | cut -d ' ' -f 2` ; do
	echo $FILEPATH
	git update-index --no-assume-unchanged ${FILEPATH}
done

echo "pause..."
read Wait

