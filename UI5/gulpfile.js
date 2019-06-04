var gulp = require('gulp');
var del = require('del');
var ui5preload = require('gulp-ui5-preload');
var uglify = require('gulp-uglify');
var minify = require('gulp-minify');
var prettydata = require('gulp-pretty-data');
var gulpif = require('gulp-if');
var replace = require('gulp-replace');
var rm = require('gulp-rimraf');
var gulps = require("gulp-series");
var prettify = require('gulp-prettify');

var project = 'ui5';

gulp.task('ui5preload',function () {
        return gulp.src(
            [
                './wwwroot/ui5/**/**.+(js|xml)',
                '!Component-preload.js',
                '!gulpfile.js',
                '!WEB-INF/web.xml',
                '!model/metadata.xml',
                '!node_modules/**',
                '!assets/**',
                '!resources/**'
            ]
        ).pipe(gulpif('./*.js', uglify())) 
         .pipe(ui5preload({
                base: './wwwroot/ui5/',
                namespace: your_project,
                fileName: 'Component-preload.js'
            })).pipe(gulp.dest('./wwwroot/ui5/'));
});

