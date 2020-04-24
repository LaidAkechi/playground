:; set -eo pipefail
:; ./build/ci-configuration/build.sh "$@"
:; exit $?

@ECHO OFF
powershell -ExecutionPolicy ByPass -NoProfile %0\..\build\ci-configuration\build.ps1 %*
