using DSharpPlus;
using DSharpPlus.CommandsNext;
using VBotDiscord.commands;
using VBotDiscord.config;

namespace VBotDiscord
{
    internal class Program
    {
        private static DiscordClient Client { get; set; }
        private static CommandsNextExtension Commands {  get; set; }
        static async Task Main(string[] args)
        {
            var botConfig = ConfigReader.LoadConfig();

            var discordConfig = new DiscordConfiguration() {
                Intents = DiscordIntents.All,
                Token = botConfig.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true                
            };

            Client = new DiscordClient(discordConfig);

            Client.Ready += Client_Ready;

            var commandsConfig = new CommandsNextConfiguration() {
                StringPrefixes = new string[] { botConfig.Prefix },
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = false
            };

            Commands = Client.UseCommandsNext(commandsConfig);
            Commands.RegisterCommands<TestCommands>();

            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private static Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs args) {
            return Task.CompletedTask;
        }
    }
}
