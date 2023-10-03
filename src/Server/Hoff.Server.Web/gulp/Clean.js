// core js
const { series } = require('gulp');
const fs = require('fs');
const path = require('path');

// settings
const appDirectory = fs.realpathSync(process.cwd());
const resolveApp = relativePath => path.resolve(appDirectory, relativePath);

// task vars
const buildPath = resolveApp('./Resources');
const binPath = resolveApp('./bin');

// tasks
function clean(cb) {
    const rimraf = require('rimraf');
    rimraf(buildPath, cb);
};

// export
const CleanTasks = series(clean);
exports.Tasks = CleanTasks;