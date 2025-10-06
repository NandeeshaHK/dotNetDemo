using System;
using System.IO;
using Python.Runtime; // from pythonnet

class Program
{
    static void Main(string[] args)
    {
        // 1) Optional: point to specific libpython if auto-detection fails.
        // On Windows this is usually not necessary if Python is installed and on PATH.
        // Example:
        // Runtime.PythonDLL = @"C:\Users\you\AppData\Local\Programs\Python\Python310\python310.dll";
        //
        // On Linux you might set: Runtime.PythonDLL = "/usr/lib/x86_64-linux-gnu/libpython3.10.so";

        // 2) Tell pythonnet where to find your python module(s)
        var baseDir = AppContext.BaseDirectory;
        var pythonFolder = Path.Combine(baseDir, "python");
        if (!Directory.Exists(pythonFolder))
        {
            Console.WriteLine($"ERROR: python folder not found at: {pythonFolder}");
            Console.WriteLine("Make sure the `python` folder with model_runner.py is next to the executable.");
            return;
        }

        // Add python folder to Python path before init (safer)
        Environment.SetEnvironmentVariable("PYTHONPATH", pythonFolder);

        // 3) Initialize embedded Python
        PythonEngine.Initialize();

        try
        {
            using (Py.GIL()) // acquire GIL (required for python calls)
            {
                dynamic model = Py.Import("model_runner"); // import python.model_runner
                // Example input array
                double[] features = new double[] { 2.4, 3.0, 1.6 };

                // Convert the C# array to a Python object automatically
                dynamic result = model.predict(features);

                // result is expected to be a Python dict-like object -> map to dynamic
                double avg = (double)result["avg"];
                string label = (string)result["label"];

                Console.WriteLine($"Python returned -> avg: {avg:F3}, label: {label}");
            }
        }
        catch (PythonException pex)
        {
            Console.WriteLine("PythonException thrown from embedded Python:");
            Console.WriteLine(pex);
        }
        catch (Exception ex)
        {
            Console.WriteLine("General exception:");
            Console.WriteLine(ex);
        }
        finally
        {
            // Shutdown python engine before program exit
            PythonEngine.Shutdown();
        }
    }
}
