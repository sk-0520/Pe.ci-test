import path from 'path';

import * as tune_module from './tune-module';

const workDirPath = path.resolve(__dirname, '..', '..', 'Output', 'help');

tune_module.main(workDirPath);
