using CommandLine;
using CommandLine.Text;
using MonoTanksClient.CommandLine;
using MonoTanksClient.Networking;

string host = "localhost";
string port = "5000";
string nickname = string.Empty;
string code = string.Empty;

var parser = new Parser(with =>
{
    with.CaseSensitive = false;
    with.HelpWriter = null;
});

var parserResult = parser.ParseArguments<CommandLineOptions>(args);

if (parserResult.Tag == ParserResultType.NotParsed)
{
    Console.WriteLine("[System] Invalid command line arguments. Here are available commands:\n");
    Console.WriteLine(HelpText.AutoBuild(parserResult, null, null));

    return;
}

_ = parserResult.WithParsed((opts) =>
{
    if (!string.IsNullOrEmpty(opts.Host))
    {
        host = opts.Host;
    }

    if (!string.IsNullOrEmpty(opts.Port))
    {
        port = opts.Port;
    }

    if (!string.IsNullOrEmpty(opts.Nickname))
    {
        nickname = opts.Nickname;
    }

    if (!string.IsNullOrEmpty(opts.Code))
    {
        code = opts.Code;
    }
});

_ = parserResult.WithNotParsed<CommandLineOptions>((err) =>
{
    // Handle errors (if any)
    foreach (var error in err)
    {
        Console.WriteLine(error);
    }
});

BotWebSocketClient client = new(host, port, nickname, code);

await client.ConnectAsync();
