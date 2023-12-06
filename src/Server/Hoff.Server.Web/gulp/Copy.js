// core js
const { series } = require('gulp');
const fs = require('fs');
const path = require('path');
const gulp = require('gulp');

// settings
const appDirectory = fs.realpathSync(process.cwd());
const resolveApp = relativePath => path.resolve(appDirectory, relativePath);

// task settings
const srcJsPath = resolveApp('./src/js/*.js');
const destJsPath = resolveApp('./build/js');
const srcModulePath = resolveApp('./src/modules/*.js');

const srcTemplatesPath = resolveApp('./src/html/*.*');
const destTemplatesPath = resolveApp('./Resources/html');

const srcImagesPath = resolveApp('./src/images/*.*');
const destImagesPath = resolveApp('./Resources/images');



//tasks
function copyTemplates() {
    return gulp
        .src(srcTemplatesPath)
        .pipe(gulp.dest(destTemplatesPath));
};

function copyImages() {
    return gulp
        .src(srcImagesPath)
        .pipe(gulp.dest(destImagesPath));
};


function copyJs() {
    return gulp
        .src(srcJsPath)
        .pipe(gulp.dest(destJsPath));
};


function copyJsModules() {
    return gulp
        .src(srcModulePath)
        .pipe(gulp.dest(destJsPath));
};

// export
const CopyTasks = series(copyTemplates, copyJs, copyJsModules, copyImages);
exports.Tasks = CopyTasks;