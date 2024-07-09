﻿using Microsoft.Win32;
using System;
using System.IO;

namespace CustomStreamMaker
{
    internal class GameLocation
    {
        internal static string InitializeValidGamePath()
        {
            string gamePath;
            try
            {
                if (!string.IsNullOrEmpty(Properties.Settings.Default.GameDirectory))
                {
                    gamePath = Properties.Settings.Default.GameDirectory;
                }
                else if (Environment.Is64BitOperatingSystem)
                {
                    gamePath = (string)RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 1451940", false).GetValue("InstallLocation");
                }
                else
                {
                    gamePath = (string)RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 1451940", false).GetValue("InstallLocation");
                }
                if (string.IsNullOrEmpty(Properties.Settings.Default.GameDirectory) && !string.IsNullOrEmpty(gamePath) && Directory.Exists(gamePath))
                {
                    Properties.Settings.Default.GameDirectory = gamePath;
                    Properties.Settings.Default.Save();
                }
                return gamePath;
            }
            catch
            {
                return null;
            }
        }

        private string InitializeValidSteamPath()
        {
            string steamPath;
            if (Environment.Is64BitOperatingSystem)
            {
                steamPath = (string)RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64).OpenSubKey(@"SOFTWARE\Valve\Steam", false).GetValue("SteamExe");
            }
            else
            {
                steamPath = (string)RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Valve\Steam", false).GetValue("SteamExe");
            }
            return steamPath;
        }

        internal static bool IsGameModded(out string modPath)
        {
            modPath = InitializeValidGamePath() + @"\BepInEx\plugins";
            if (!Directory.Exists(modPath))
            {
                modPath = null;
                return false;
            }
            return true;
        }
    }
}
