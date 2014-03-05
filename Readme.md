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

## File Filters

File Filters can be a combination of literal and wildcard characters, but do not support regular expressions.
The following wildcard specifiers are supported:

<table>
  <tr>
    <th>Specifier</th><th>Matches</th>
  </tr>
  <tr>
    <td>* (asterisk)</td><td>Zero or more characters in that position.</td>
  </tr>
  <tr>
    <td>? (question mark)</td><td>Exactly one character in that position.</td>
  </tr>
</table>

See [MSDN](http://msdn.microsoft.com/en-us/library/dd413233) for more info.

You can prefix a file filter with a - (minus symbol) to *exclude* files matching that wildcard pattern. For example:

        *.asp, -*.aspx, -*.svn\*, -*.git\*

will only search .asp files (not .aspx files) and will exclude files within subversion and git repos. 
		
## History

Grepper was originally written by [AZCoder](https://github.com/AZCoder) as a DLL/WinForms app. I ([Adam Jarret](https://github.com/adamjarret)) forked Grepper and ported the WinForms app to WPF. I also made a few changes to the DLL to support progress reporting and match range highlighting (and tweaked the WinForms app to be compatible). 
