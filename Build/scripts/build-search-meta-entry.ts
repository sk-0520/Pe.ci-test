import path from 'node:path';

import * as build_search_meta from './build-search-meta';

const sourceDirectoryPath = path.resolve(__dirname, '..', '..', 'Source', 'Help');
const outputFilePath = path.resolve(__dirname, '..', '..', 'Define', 'help-meta.json');

build_search_meta.main(sourceDirectoryPath, outputFilePath);
