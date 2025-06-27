cd $(dirname "$0")
working_dir=$(pwd)
sh DotnetPackage_All.sh
steamcmd +login romans_i_xvi_gaming +run_app_build "${working_dir}/app_build_1332800.vdf" +quit
