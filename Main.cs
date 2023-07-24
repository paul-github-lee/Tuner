using System;
using System.IO;
using System.Diagnostics;
using System.ServiceProcess;
using System.Collections.Generic;

public static class Tuner
{
    public static void Main(string[] args)
    {
        Console.Write("Optimizing pc. Please wait.\n");
        Temporary.deleteFiles();
        Temporary.deleteFolders();
        Drive.deleteEmptyFolders(@"C:\");
        Drive.deleteDownloadsFiles();
        Drive.deleteDownloadsFolders();
        WindowsSearchService.stop();
        WindowsSearchService.disable();
        Console.Write("\aTune complete. Restart required. Press any key to restart");
        Console.ReadKey();
        Computer.restart();
    }
}

public static class Temporary
{
    public static void deleteFiles()
    {
        foreach (var file in Directory.GetFiles(@"C:\Users\" +
                                                Environment.UserName +
                                                @"\AppData\Local\Temp"))
        {
            try
            {
                File.Delete(file);
            }
            catch (Exception)
            {
                ;
            }
        }
    }

    public static void deleteFolders()
    {
        foreach (var folder in Directory.GetDirectories(@"C:\Users\" +
                                                Environment.UserName +
                                                @"\AppData\Local\Temp"))
        {
            try
            {
                Directory.Delete(folder);
            }
            catch (Exception)
            {
                ;
            }
        }
    }
}

public static class Drive
{
    public static void deleteEmptyFolders(string folderPath)
    {
        try
        {
            string[] subFolders = Directory.GetDirectories(folderPath);
            foreach (string subFolder in subFolders)
            {
                deleteEmptyFolders(subFolder);
            }
            if (Directory.GetFiles(folderPath).Length == 0 && 
                Directory.GetDirectories(folderPath).Length == 0)
            {
                Directory.Delete(folderPath);
            }
        }
        catch (Exception ex)
        {
            ; 
        }
    }

    public static void deleteDownloadsFiles()
    {
        foreach (var file in Directory.GetFiles(@"C:\Users\" +
                                                Environment.UserName +
                                                @"\Downloads"))
        {
            try
            {
                File.Delete(file);
            }
            catch (Exception)
            {
                ;
            }
        }
    }

    public static void deleteDownloadsFolders()
    {
        foreach (var folder in Directory.GetDirectories(@"C:\Users\" +
                                                Environment.UserName +
                                                @"\Downloads"))
        {
            try
            {
                Directory.Delete(folder, true);
            }
            catch (Exception)
            {
                ;
            }
        }
    }
}

public static class WindowsSearchService
{
    // requires administrative privileges (app.manifest modified)
    public static void stop()
    {
        ServiceController service = new ServiceController("Wsearch");
        if (service.Status == ServiceControllerStatus.Running)
        {
            service.Stop();
            service.WaitForStatus(ServiceControllerStatus.Stopped,
                                  TimeSpan.FromSeconds(30));
        }
        else
        {
            ;
        }
    }

    // requires administrative privileges (app.manifest modified)
    public static void disable()
    {
        using (Microsoft.Win32.RegistryKey key =
                Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\Wsearch",
                                                                                                      true))
        {
            if (key != null)
            {
                key.SetValue("Start",
                         4,
                         Microsoft.Win32.RegistryValueKind.DWord);
            }
            else
            {
                ;
            }
        }
    }
}

public static class Computer
{
    public static void restart()
    {
        try
        {
            ProcessStartInfo process = new ProcessStartInfo
            {
                FileName = "shutdown",
                Arguments = "/r /f /t 0", 
                CreateNoWindow = true,
                UseShellExecute = false
            };
            Process.Start(process);
        }
        catch (Exception ex)
        {
            Console.Write("Restart your pc");
        }
    }
}
