# Grepper

![Screen Shot](https://raw2.github.com/adamjarret/Grepper/master/GrepperWPF/ScreenShot.png)


Search the contents of text files in a directory.

## Features

* Supports regular expression and plain text searches
* Filter searched files using wildcard pattern matching
* Remembers recently searched terms, paths, extensions and window size/position
* Matched portions of a result appear in bold
* Optional recursive path search
* Optional shortcut in Explorer context menu (when right clicking on a folder)

## Explorer Context Menu

Grepper does not currently have an installer.

To **enable** the Explorer Context Menu Item,  run the app with the following parameters **as an Administrator**:

        C:\Path\To\GrepperWPF.exe /contextmenu=1

To **disable** the Explorer Context Menu Item,  run the app with the following parameters **as an Administrator**:

        C:\Path\To\GrepperWPF.exe /contextmenu=0

## History

Grepper was originally written by [AZCoder](https://github.com/AZCoder) as a DLL/WinForms app. I ([Adam Jarret](https://github.com/adamjarret)) forked Grepper and ported the WinForms app to WPF. I also made a few changes to the DLL to support progress reporting and match range highlighting (and tweaked the WinForms app to be compatible). 
