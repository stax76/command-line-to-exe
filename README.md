# WinAppLauncher

WinAppLauncher executes a command line.

Optional: Rename WinAppLauncher.exe for instance to myUtil.exe

Create a text file with params extensions next to WinAppLauncher.
The text file name must match the executable name:

```
C:\myFolder\myUtil.exe
C:\myFolder\myUtil.params
```

In the text file add:

```
path = path to a executable or any file like for instance a script
args = arguments to pass
hidden = yes
```

%startup% gets the folder where the myUtil executable is located.

%args% gets the arguments passed to myUtil.

hidden hides a console window.

Please click the star button if you ever find it useful.