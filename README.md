
# command-line-to-exe

Allows creating an executable that executes a defined command line.

Rename CommandLineToExe.exe for instance to example.exe

Create a text file with params extensions next to example.exe.
The text file name must match the executable name:

```
C:\folder\example.exe
C:\folder\example.params
```

The params ini text file supports the following options:

```
path = <exe or other files that support shell execute>
args = <arguments to pass, see macro section below>
hidden = yes  # hides console windows
working-directory = <working directory>
```

### Macros

%startup% gets the folder where the example executable is located.

%args% or %1%, %2%, %3% etc. are the arguments passed to the example executable.
