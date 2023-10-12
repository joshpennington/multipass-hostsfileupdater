// See https://aka.ms/new-console-template for more information

using multipass_hostsfileupdater.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text.Json.Serialization;

internal class Program
{
    private static void Main(string[] args)
    {

        if(!IsWindows())
        {
            Console.WriteLine("Program runs on Windows only.");
            return;
        }

        if(!IsAdministrator())
        {
            Console.WriteLine("Program must be run as administrator.");
            return;
        }


        var multipassHostMappings = GetMultiPasshostsMappings();
        if(multipassHostMappings == null)
        {
            Console.WriteLine("No Multipass mappings found.");
            return;
        }

        var multipassList = GetMultiPassList();
        if(multipassList == null)
        {
            Console.WriteLine("No Multipass instances found.");
            return;
        }

        var hostsFilePath = @"C:\Windows\System32\drivers\etc\hosts";

        var hostsFileLines = File.ReadLines(hostsFilePath);
        
        var rowsOut = new List<string>();

        foreach (string hostFileLine in hostsFileLines)
        {
            var hostFileLineSplit = hostFileLine.Split(' ');
            var hostFileLineWritten = false;

            if (hostFileLineSplit.Length == 2)
            {
                foreach (var mapping in multipassHostMappings)
                {
                    var multipassInstances = multipassList.List.Where(y => y.Name != null && y.Name.Equals(mapping.Name)).ToList();
                    if (multipassInstances.Count > 0)
                    {
                        foreach (var multipassInstance in multipassInstances)
                        {
                            foreach (var uri in mapping.Uris)
                            {
                                if(hostFileLineSplit[1].Equals(uri))
                                {
                                    hostFileLineWritten = true;
                                    rowsOut.Add(multipassInstance.Ipv4[0] + " " + uri);
                                }
                            }
                        }
                    }
                }
            }

            if(!hostFileLineWritten)
            {
                rowsOut.Add(hostFileLine);
            }
        }

        var hostsFileBackupPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\.multipass-hosts-backup";

        File.Copy(hostsFilePath, hostsFileBackupPath, true);

        var newHostsFileContent = string.Join("\n", rowsOut);
        File.WriteAllText(hostsFilePath, newHostsFileContent);

        Console.WriteLine("Hosts file updated!");
    }

    public static MultipassListResult? GetMultiPassList()
    {
        Process proc = new Process();
        proc.StartInfo.FileName = "multipass";
        proc.StartInfo.Arguments = "list --format json";
        proc.StartInfo.UseShellExecute = false;
        proc.StartInfo.RedirectStandardOutput = true;
        proc.Start();

        string output = proc.StandardOutput.ReadToEnd();

        var multipassList = JsonConvert.DeserializeObject<MultipassListResult>(output);

        proc.WaitForExit();

        return multipassList;
    }

    public static bool IsWindows()
    {
        return Environment.OSVersion.Platform == PlatformID.Win32NT || Environment.OSVersion.Platform == PlatformID.Win32Windows;
    }

    public static bool IsAdministrator()
    {
        WindowsIdentity identity = WindowsIdentity.GetCurrent();
        var principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    public static List<HostFileMapping>? GetMultiPasshostsMappings()
    {
        var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\.multipass-hosts";
        if (!File.Exists(path))
        {
            Console.WriteLine("Error: File " + path + " not exist.");
            return null;
        }

        var hostsToMultipassInstance = File.ReadAllText(path);
        List<HostFileMapping> mappings = JsonConvert.DeserializeObject<List<HostFileMapping>>(hostsToMultipassInstance);
        return mappings;
    }
}