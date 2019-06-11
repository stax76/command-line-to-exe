# WinAppLauncher

WinAppLauncher executes a command line.

Optional: Rename WinAppLauncher.exe for instance to myUtil.exe

Create a text file with params extensions next to WinAppLauncher.
The text file name must match the executable name:

C:\myFolder\myUtil.exe

C:\myFolder\myUtil.params

In the text file add:

```
path = path to a executable or any file like for instance a script
args = arguments to pass
hidden = yes
```

In path and args the macro %startup% resolves to the folder where the WinAppLauncher executable is located.

hidden hides a console window.

It's possible to pass arguments to myUtil.exe, they are appended to args.