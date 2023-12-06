/// <binding BeforeBuild='default' Clean='clean' />
"use strict";
const gulp = require('gulp');
const { task, parallel } = require('gulp');

const CleanTasks = require("./gulp/Clean");
const ScssTasks = require("./gulp/Scss");
const JsTasks = require("./gulp/JavaScript");
const CopyTasks = require("./gulp/Copy");

const glob = require("./gulp/glob");
const yarn = require('gulp-yarn');

gulp.task('yarn', function () {
    return gulp.src(['./package.json'])
        .pipe(yarn());
});


task('build-Debug', gulp.series([parallel(ScssTasks.Tasks, JsTasks.Tasks, CopyTasks.Tasks)]));
task('default', gulp.series([ parallel(ScssTasks.Tasks, JsTasks.Tasks, CopyTasks.Tasks)]));
task('clean', gulp.series([CleanTasks.Tasks]));