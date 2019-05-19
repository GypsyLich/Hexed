#! /bin/sh
/Applications/Unity/Unity.app/Contents/MacOS/Unity -batchmode -createManualActivationFile -logfile

/Applications/Unity/Unity.app/Contents/MacOS/Unity -batchmode -manualLicenseFile Unity_v2018.x.ulf -logfile


project="Hexed"

echo "Attempting to build $project for OS X"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile $(pwd)/unity.log \
  -projectPath $(pwd) \
  -buildOSXUniversalPlayer "$(pwd)/Build/osx/$project.app" \
  -quit


echo 'Logs from build'
cat $(pwd)/unity.log