# ReportDcPlugin: A Discord Report Plugin for Counter-Strike

This plugin allows you to report issues directly to a Discord webhook. It's as simple as typing a command ingame.

## Usage

For example, a player could type:

`!report Incident at location A, send admin immediately`

## Discord Output

The output in Discord would look like this:

`(Server Name) | (Player's Name) | (Player's SteamID) = (Report details)`

For example,

`Server1 | Player1 | 7777777777 = Incident at location A, send admin immediately`

## Configuration

You can add as many commands as you like into the `Commands` section of the JSON configuration file.

Here is an example configuration:

```json 
{
    "Prefix": "Prefix",
    "PlayerResponseNotEnoughInput": "Please provide more details for the report",
    "Commands": [
        {
            "CommandString": "report",
            "Endpoint": "https://discord.com/api/webhooks/****************/*************************",
            "MessageTemplate": "{HostName} | {Player.Name} | {Player.SteamID} = {Args}",
            "Description": "Report an issue"
        }
    ],
    "PlayerResponseSuccessfull": "Your report has been successfully sent",
    "ServerName": "Server1"
}
```


`PlayerResponseNotEnoughInput` is sent to the player if they enter an invalid report, like typing only '!report'. `PlayerResponseSuccessfull` is the message shown to the player upon a successful report.

## Getting a Discord Webhook
To get a Discord webhook, follow these steps:

1. Open the Discord channel where you want to receive notifications.
2. From the channel menu, select Edit Channel.
3. Select Integrations.
4. If there are no existing webhooks, select Create Webhook. If webhooks exist, select View Webhooks, then New Webhook.
5. Enter the name for the bot that will post the messages.
6. Optionally, you can edit the avatar.
7. Copy the URL from the WEBHOOK URL field.
8. Save your changes.

After obtaining the webhook URL, replace the `Endpoint` field for each command in the `Commands` section with the URL.

# Using Placeholders in MessageTemplate

The `MessageTemplate` field uses placeholders that will be replaced by corresponding values when a report is sent.

The placeholders you can use are:

- `{Player.Name}`: Replaced by the player's name.
- `{Player.SteamID}`: Replaced by the player's Steam ID.
- `{Args}`: Replaced by the details of the report.
- `{HostName}`: Replaced by your server's name.

Here's an example `MessageTemplate`:

`"{HostName} | {Player.Name} | {Player.SteamID} = {Args}"`

Now, let's say a player named "JohnDoe" sends a report on your server "Server1". His SteamID is 7777777777 and the report is about a "Hostage situation at bombsite B".

In Discord, you will then see:

`plaintext Server1 | JohnDoe | 7777777777 = Hostage situation at bombsite B`
