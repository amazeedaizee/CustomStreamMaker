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
            DialogResult msg;
            string requestedAssemblyName = args.Name.Split(',')[0];
            string gamePath;
            if (!string.IsNullOrEmpty(Properties.Settings.Default.GameDirectory))
            {
                gamePath = Properties.Settings.Default.GameDirectory;
            }
            else if (Environment.Is64BitOperatingSystem)
            {

                if (RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 1451940", false).GetValue("InstallLocation") != null)
                    gamePath = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 1451940", false).GetValue("InstallLocation") + "\\Windose_Data\\Managed\\";
                else gamePath = null;
            }
            else
            {
                if (RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 1451940", false).GetValue("InstallLocation") != null)
                    gamePath = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 1451940", false).GetValue("InstallLocation") + "\\Windose_Data\\Managed\\";
                else gamePath = null;
            }
            try
            {
                if ((gamePath == null && string.IsNullOrEmpty(Properties.Settings.Default.GameDirectory)) || !Directory.Exists(gamePath))
                {
                    do
                    {
                        msg = MessageBox.Show("Could not find game path! \n\nTo continue, please open the folder containing the game executable. (NEEDY GIRL OVERDOSE)", "", MessageBoxButtons.OKCancel);
                        if (msg == DialogResult.Cancel)
                            Environment.Exit(0);
                    } while ((gamePath = OpenGamePath()) == null);
                    Properties.Settings.Default.GameDirectory = gamePath;
                    Properties.Settings.Default.Save();
                }
                if (requestedAssemblyName == "Assembly-CSharp")
                    return Assembly.LoadFrom(gamePath + requestedAssemblyName + ".dll");
                return Assembly.LoadFrom(Path.Combine(Directory.GetCurrentDirectory(), "Assemblies", requestedAssemblyName + ".dll"));
            }
            catch
            {
                return null;
            }
        }

        static string OpenGamePath()
        {
            var openNsoStream = new FolderBrowserDialog();
            openNsoStream.RootFolder = Environment.SpecialFolder.MyComputer;
            if (openNsoStream.ShowDialog() == DialogResult.OK)
            {
                if (!File.Exists(openNsoStream.SelectedPath + @"\Windose_Data\Managed\Assembly-CSharp.dll"))
                {
                    return null;
                }
                return openNsoStream.SelectedPath;
            }
            return null;
        }
    }
}
