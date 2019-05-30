#! /bin/sh

echo 'Downloading Unity 2018.3.8 pkg:'
curl --retry 5 -o Unity.pkg https://download.unity3d.com/download_unity/fc0fe30d6d91/MacEditorInstaller/Unity.pkg
if [ $? -ne 0 ]; then { echo "Download failed"; exit $?; } fi


##echo 'Downloading Unity 2018.3.8 Windows Build Support pkg:'
##curl --retry 5 -o Unity_win.pkg https://download.unity3d.com/download_unity/fc0fe30d6d91/MacEditorTargetInstaller/UnitySetup-Windows-Mono-Support-for-Editor-2018.3.8f1.pkg
##if [ $? -ne 0 ]; then { echo "Download failed"; exit $?; } fi

# Run installer(s)
echo 'Installing Unity.pkg'
sudo installer -dumplog -package Unity.pkg -target /
##echo 'Installing Unity_win.pkg'
##sudo installer -dumplog -package Unity_win.pkg -target /
