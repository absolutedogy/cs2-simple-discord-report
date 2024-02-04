using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using CounterStrikeSharp.API.Modules.Cvars;

namespace ReportDcPlugin;

public class ReportDcPlugin : BasePlugin
{
    public override string ModuleName => "ReportDcPlugin";

    public override string ModuleVersion => "0.0.2";
    public override string ModuleAuthor => "Constummer";
    public override string ModuleDescription => "ReportDcPlugin";

    private static readonly HttpClient _httpClient;

    static ReportDcPlugin()
    {
        _httpClient = new HttpClient();
    }

    public class Config
    {
        public string Prefix { get; set; }
        public string PlayerResponseNotEnoughInput { get; set; }
        public List<CommandSettings> Commands { get; set; }
        public string PlayerResponseSuccessfull { get; set; }
        public string ServerName { get; internal set; }
    }

    private static Config? _config;

    public override void Unload(bool hotReload)
    {
        base.Unload(hotReload);
    }

    public override void Load(bool hotReload)
    {
        var configPath = Path.Join(ModuleDirectory, "Config.json");
        if (!File.Exists(configPath))
        {
            var data = new Config()
            {
                Prefix = "Prefix",
                PlayerResponseNotEnoughInput = "Not enough input",
                PlayerResponseSuccessfull = "Reported successfully",
                Commands = new List<CommandSettings>()
                {
                    new CommandSettings
                    {
                        CommandString = "report",
                        Endpoint = "discord.....",
                        MessageTemplate = "{HostName} | {Player.Name} | {Player.SteamID} = {Args}",
                        Description = "Something or other"
                    },
                },
                ServerName = "Server1"
            };
            File.WriteAllText(configPath, JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping }));
            _config = data;
        }
        else
        {
            var text = File.ReadAllText(configPath);

            _config = JsonSerializer.Deserialize<Config>(text);
        };

        if (_config?.Commands != null)
        {
            foreach (var command in _config.Commands)
            {
                AddCommand(command.CommandString, command.Description, (player, info) =>
                {
                    if (ValidateCallerPlayer(player) == false)
                    {
                        return;
                    }

                    if (info.ArgCount <= 1)
                    {
                        player!.PrintToChat(AddPrefixToTheMessage(_config.PlayerResponseNotEnoughInput, _config.Prefix));
                    };
                    
                    var hostName = ConVar.Find("hostname")?.StringValue ?? _config.ServerName ?? "N/A";
                    Dictionary<string, string> messageContext = new Dictionary<string, string>()
                    {
                        {"HostName", hostName},
                        {"Player.Name", player?.PlayerName ?? "N/A"},
                        // JSX looking ass
                        {"Player.SteamID", $"{(player?.SteamID is null ? "N/A" : 0)}"},
                        {"Args", info.ArgString}
                    };

                    var msg = MessageInterpolation.InterpolateString(command.MessageTemplate, messageContext);
                    
                    Server.NextFrame(async () =>
                    {
                        await PostAsync(command.Endpoint, msg);
                    });
                    
                    player?.PrintToChat(AddPrefixToTheMessage(_config.PlayerResponseSuccessfull, _config.Prefix));
                });
            }
        }
        base.Load(hotReload);
    }

    private static string AddPrefixToTheMessage(string message, string prefix)
    {
        if (string.IsNullOrWhiteSpace(prefix))
            return message;
        return $"[{prefix}]{message}";
    }

    private static bool ValidateCallerPlayer(CCSPlayerController? player)
    {
        return player != null
               && player is { IsValid: true, IsBot: false, PlayerPawn.IsValid: true }
               && player.PlayerPawn.Value != null
               && player.PlayerPawn.Value.IsValid;
    }

    private async Task PostAsync(string uri, string message)
    {
        try
        {
            var body = JsonSerializer.Serialize(new { content = message });
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage res = (await _httpClient.PostAsync($"{uri}", content)).EnsureSuccessStatusCode();
        }
        catch
        {
        }
    }
}