// core js
const { series } = require('gulp');
const fs = require('fs');
const path = require('path');
const gulp = require('gulp');
const cache = require('gulp-cached');

// settings
const appDirectory = fs.realpathSync(process.cwd());
const resolveApp = relativePath => path.resolve(appDirectory, relativePath);
const { SetBuildEnv } = require('./ConfigHelpers');
const host = SetBuildEnv();

// plugins
const sass = require('gulp-sass')(require('sass'));
const postcss = require('gulp-postcss');
const autoprefixer = require('autoprefixer');
const minifyCss = require('gulp-minify-css')
const stripInlineComments = require('postcss-strip-inline-comments');
const gutil = require('gulp-util');
const gulpStylelint = require('@movahhedi/gulp-stylelint');


// plugin config
var postCssPlugins = [
    stripInlineComments(),
    autoprefixer(),
];

// task vars
const scssPath = resolveApp('./src/scss/*.scss');
const destCssPath = resolveApp('./Resources/css');

// tasks
function lintCss() {
    return gulp
        .src(scssPath)
        .pipe(gulpStylelint({
            fix: true
        }))
};

function buildScss() {
    return gulp.src(scssPath)
        .pipe(sass().on('error', sass.logError))
        .pipe(postcss(postCssPlugins))
        .pipe(host.isDebug === true ? gutil.noop() : minifyCss())
        .pipe(gulp.dest(destCssPath));
};

// exports
const ScssTasks = series(lintCss, buildScss);
exports.Tasks = ScssTasks;