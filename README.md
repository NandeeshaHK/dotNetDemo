# Prerequisites

## Install .NET 6 SDK (or later). On Windows use the installer; on Linux use distro packages.

## Install Python 3.8–3.11 (matching what pythonnet supports on your machine). Put Python on your PATH. (You do not need to pip install pythonnet for this scenario because .NET uses the NuGet pythonnet package; still Python must be installed so pythonnet can embed it.)


Steps (Windows / macOS / Linux)

1. Create folder and files exactly as above:
```Folder
DotNetPythonDemo/
├ DotNetPythonDemo.csproj
├ Program.cs
└ python/
    └ model_runner.py
```

2. From project root run:
```bash
dotnet restore
dotnet build
dotnet run
```
Output should be:

Python returned -> avg: 2.333, label: low

(Numbers depend on the example features array.)



If you get a libpython / DLL load error

Windows: set Runtime.PythonDLL path in Program.cs to the exact python3x.dll path (before PythonEngine.Initialize()), e.g.:

Runtime.PythonDLL = @"C:\Users\you\AppData\Local\Programs\Python\Python310\python310.dll";

Linux: install libpython3.x dev package (e.g. sudo apt install libpython3.10 or similar) and set Runtime.PythonDLL to /usr/lib/x86_64-linux-gnu/libpython3.10.so if autodetect fails.


Note about long-lived apps

If your .NET process will call Python many times, call PythonEngine.Initialize() once at startup and Shutdown() at the end (as shown). Avoid repeated initialize/shutdown loops.



---
# Testing & quick extension ideas (1-line each)

Send different arrays to predict from C# to test error handling.

Replace predict with a wrapper that loads a pickled scikit-learn model (only if you install numpy/scikit-learn in the Python environment).

Implement an async worker in .NET that calls Python on a dedicated thread (remember to acquire GIL each call).



---
# Troubleshooting checklist (common issues)

Python not found / import error: ensure python is on PATH and python --version works.

DLL/libpython load error: set Runtime.PythonDLL to the correct shared library path.

Type conversions failing: convert arrays explicitly (e.g., use double[] in C# and verify Python code accepts list).

Python exceptions swallowed: catch PythonException in C# and print it — it contains the stack trace.

