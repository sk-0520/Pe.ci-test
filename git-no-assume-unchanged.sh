git ls-files -v | grep '^[[:lower:]]'
echo "exec? (y/n)"
read isExec
if [ $isExec = 'y' ] ; then
	for FILEPATH in `git ls-files -v | grep '^[[:lower:]]' | cut -d ' ' -f 2` ; do
		echo $FILEPATH
		#git update-index --no-assume-unchanged ${FILEPATH}
	done
	echo "pause..."
	read _
fi

