
module.exports = function (grunt) {
    "use strict";
    grunt.initConfig({
        uglify: {
            my_target: {
                files: {
                    'ui5/Component-preload.js': '*.js'
                }
            }
        }
    });
    grunt.loadNpmTasks("grunt-sapui5");
    grunt.config.merge({
        compatVersion: "edge",
        deploy_mode: "html_repo"
    });
    grunt.registerTask("default", [
        "clean",
        "lint",
        "build"
    ]);
    grunt.loadNpmTasks("grunt-sapui5-test");
    grunt.registerTask("unit_and_integration_tests", ["test"]);
    grunt.config.merge({
        coverage_threshold: {
            statements: 0,
            branches: 100,
            functions: 0,
            lines: 0
        }
    });
};