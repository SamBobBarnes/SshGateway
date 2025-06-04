using System.Diagnostics;

namespace SshGateway;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        var proc = Process.Start("ssh.exe", "hermes");
        proc.WaitForExit();
    }
}