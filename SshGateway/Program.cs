using System.Diagnostics;
using System.Text.Json;

namespace SshGateway;

class Program
{
    static void Main(string[] args)
    {
        var sshExecutable = Environment.OSVersion.Platform == PlatformID.Win32NT
            ? "ssh.exe"
            : "ssh"; 
        
        var serversFilePath = args.Length > 0 ? args[0] : "servers.json";
        
        if(!File.Exists(serversFilePath))
        {
            Console.WriteLine(Path.GetFullPath(serversFilePath));
            var path = Path.Combine(Directory.GetCurrentDirectory(), serversFilePath);
            Console.WriteLine($"Error: The file '{path}' does not exist.");
            return;
        }

        Server[]? servers;
        try
        {
            servers = JsonSerializer.Deserialize<Server[]>(File.ReadAllText(serversFilePath));
        } catch(JsonException ex)
        {
            Console.WriteLine($"Error parsing JSON: {ex.Message}");
            return;
        }

        if (servers == null || servers.Length == 0)
        {
            Console.WriteLine("No servers found in the configuration file.");
            return;
        }
        
        Console.WriteLine("Chose a server:");
        for (int i = 0; i < servers.Length; i++)
        {
            Console.WriteLine($"{i + 1}: {servers[i].Name} ({servers[i].Host})");
        }

        Console.WriteLine("0: Something else");

        var input = Console.ReadLine();
        
        if (int.TryParse(input, out int choice) && choice > 0 && choice <= servers.Length)
        {
            var server = servers[choice - 1];
            List<string> sshArgs = [$"{server.User}@{server.Host}", "-p", server.Port.ToString()];
            if (!string.IsNullOrEmpty(server.PathToKey))
            {
                sshArgs.AddRange(["-i", $"\"{server.PathToKey}\""]);
            }
            Ssh(sshExecutable,sshArgs.ToArray());
        }
        else if (choice == 0)
        {
            Environment.Exit(0);
        }
        else
        {
            Console.WriteLine("Invalid choice.");
        }

    }

    private static void Ssh(string sshExecutable, string[]? args = null)
    {
        Console.WriteLine($"Executing: {sshExecutable} {string.Join(" ", args ?? [])}");
        var proc = Process.Start(sshExecutable, args ?? []);
        proc.WaitForExit();
    }
}