using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("WinAppLauncher")]
[assembly: AssemblyProduct("WinAppLauncher")]
[assembly: AssemblyCopyright("Copyright © Frank Skare (stax76) 2019")]
[assembly: ComVisible(false)]
[assembly: Guid("4869e848-83d8-43ee-978e-01814646f0b3")]
[assembly: AssemblyVersion("1.1.0.0")]
[assembly: AssemblyFileVersion("1.1.0.0")]

namespace WinAppLauncher
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                var lines = File.ReadAllLines(Path.ChangeExtension(Application.ExecutablePath, "params"));
                string file = null, args = null, directory = null;
                bool hidden = false;
                var args2 = Environment.GetCommandLineArgs().Skip(1).ToArray();
                for (int i = 0; i < args2.Length; i++)
                    if (args2[i].Contains(" "))
                        args2[i] = "\"" + args2[i] + "\"";
                foreach (var line in lines)
                {
                    if (!line.Contains("=")) continue;
                    var left = line.Substring(0, line.IndexOf("=")).Trim().ToLower();
                    var right = SolveMacros(line.Substring(line.IndexOf("=") + 1).Trim());

                    switch (left)
                    {
                        case "path": file = right; break;
                        case "args": args = right; break;
                        case "directory": directory = right; break;
                        case "hidden": hidden = right.ToLower() == "yes"; break;
                    }
                }

                var p = new Process();
                if (hidden) p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.FileName = file;
                p.StartInfo.Arguments = args;
                if (!string.IsNullOrEmpty(directory)) p.StartInfo.WorkingDirectory = directory;
                p.Start();

                string SolveMacros(string val)
                {
                    return val.
                        Replace("%startup%", Application.StartupPath).
                        Replace("%args%", string.Join(" ", args2).Trim());
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), e.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}