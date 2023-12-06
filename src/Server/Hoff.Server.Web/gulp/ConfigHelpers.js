const fs = require('fs');

function SetBuildEnv() {
    // this section sets the build env for minification, etc
    // requires prebuild exe setting of:
    // echo {"config" : "$(ConfigurationName)"} > "$(ProjectDir)buildConfig.json
    // https://stackoverflow.com/questions/31712324/detect-release-debug-in-gulp-using-visual-studio-2015
    var json = fs.readFileSync("./gulp/Config/buildConfig.json", "utf8");
    var host = JSON.parse(json.replace(/^\uFEFF/, ''));
    host.isDebug = host.config == "Debug";
    return host;
}


exports.SetBuildEnv = SetBuildEnv;
