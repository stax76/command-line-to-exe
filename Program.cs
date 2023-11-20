
using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("CommandLineToExe")]
[assembly: AssemblyProduct("CommandLineToExe")]
[assembly: AssemblyCopyright("Copyright (C) Frank Skare (stax76) 2023")]
[assembly: ComVisible(false)]
[assembly: Guid("4869e848-83d8-43ee-978e-01814646f0b3")]
[assembly: AssemblyVersion("2.0.0.0")]
[assembly: AssemblyFileVersion("2.0.0.0")]

namespace CommandLineToExe
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                string[] lines = File.ReadAllLines(Path.ChangeExtension(Application.ExecutablePath, "params"));
                string[] args = Environment.GetCommandLineArgs().Skip(1).ToArray();
                string file = null;
                string argsFromConf = null;
                string workingDirectory = null;
                bool hidden = false;
           
                foreach (string line in lines)
                {
                    if (!line.Contains("=") || line.Trim().StartsWith("#"))
                        continue;

                    string left = line.Substring(0, line.IndexOf("=")).Trim().ToLower();
                    string right = SolveMacros(line.Substring(line.IndexOf("=") + 1).Trim());

                    if (right.Contains("#"))
                        right = right.Substring(0, line.IndexOf("#")).Trim();

                    for (int i = 0; i < args.Length; i++)
                        if (right.Contains($"%{i + 1}%"))
                            right = right.Replace($"%{i + 1}%", args[i]);

                    switch (left)
                    {
                        case "path": file = right; break;
                        case "args": argsFromConf = right; break;
                        case "working-directory": workingDirectory = right; break;
                        case "hidden": hidden = right.ToLower() == "yes"; break;
                    }
                }

                using (var p = new Process())
                {
                    if (hidden)
                    {
                        p.StartInfo.UseShellExecute = false;
                        p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    }
                    else
                        p.StartInfo.UseShellExecute = true;

                    p.StartInfo.FileName = file;
                    p.StartInfo.Arguments = argsFromConf;

                    if (!string.IsNullOrEmpty(workingDirectory))
                        p.StartInfo.WorkingDirectory = workingDirectory;

                    p.Start();
                }

                string SolveMacros(string val)
                {
                    return val.
                        Replace("%startup%", Application.StartupPath).
                        Replace("%args%", string.Join(" ", args.Select(i => "\"" + i + "\"")).Trim());
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), e.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
