using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace CustomStreamMaker
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var appDomain = AppDomain.CurrentDomain;
            appDomain.AssemblyResolve += OnAssemblyResolve;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new StreamEditor());
        }
        static Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            string requestedAssemblyName = args.Name.Split(',')[0];
            string gamePath;
            if (Environment.Is64BitOperatingSystem)
            {
                gamePath = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 1451940", false).GetValue("InstallLocation") + "\\Windose_Data\\Managed\\";
            }
            else
            {
                gamePath = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 1451940", false).GetValue("InstallLocation") + "\\Windose_Data\\Managed\\";
            }
            try
            {
                if (requestedAssemblyName == "Assembly-CSharp")
                    return Assembly.LoadFrom(gamePath + requestedAssemblyName + ".dll");
                return Assembly.LoadFrom(Path.Combine(Directory.GetCurrentDirectory(), "Assemblies", requestedAssemblyName + ".dll"));
            }
            catch
            {
                return null;
            }
        }
    }
}
