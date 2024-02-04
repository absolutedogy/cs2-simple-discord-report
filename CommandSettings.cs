namespace ReportDcPlugin;

public class CommandSettings
{
    /// <summary>
    /// The string for invoking the command, this will be added to the commands
    /// used to invoke the message to the provided endpoint
    /// </summary>
    public string CommandString { get; set; }
    /// <summary>
    /// The endpoint that will have the message posted to when executing the command
    /// </summary>
    public string Endpoint { get; set; }
    
    /// <summary>
    /// The template that will be used to generate the string that is sent
    /// to the webhook
    /// </summary>
    public string MessageTemplate { get; set; }
    public string Description { get; set; }

    public CommandSettings()
    {
        Description = string.Empty;
        CommandString = string.Empty;
        Endpoint = String.Empty;
        MessageTemplate = "{HostName} | {Player.Name} | {Player.SteamID} = {Args}";
    }
    
}