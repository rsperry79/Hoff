// core js
const { series } = require('gulp');
const fs = require('fs');
const path = require('path');
const gulp = require('gulp');



// settings
const appDirectory = fs.realpathSync(process.cwd());
const resolveApp = relativePath => path.resolve(appDirectory, relativePath);

// task vars
const tsPath = resolveApp('./src/ts/*.ts');



const destTempPath = resolveApp('./temp/js');
const srcTempPath = resolveApp('./temp/js/*.js');

const destJsPath = resolveApp('./Resources/js');
// plugins
const ts = require('gulp-typescript');
const tsconf = resolveApp('./src/ts/tsconfig.json');

// tasks
function buildTs() {
    let tsProject = ts.createProject(tsconf);
    return gulp.src(tsPath)
        .pipe(tsProject(tsconf))
        .pipe(gulp.dest(destJsPath))
};






// export
const JsTasks = series(buildTs);
exports.Tasks = JsTasks;