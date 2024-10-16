using CommandLine;

namespace MonoTanksClient.CommandLine;

public class CommandLineOptions
{
    [Option('h', "host", Required = false, HelpText = "Set host.")]
    public string Host { get; set; }

    [Option('p', "port", Required = false, HelpText = "Set port.")]
    public string Port { get; set; }

    [Option('n', "nickname", Required = true, HelpText = "Set nickname.")]
    public string Nickname { get; set; }

    [Option('c', "code", Required = false, HelpText = "Set join code.")]
    public string Code { get; set; }
}