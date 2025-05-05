using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using VBotDiscord.commands;
using VBotDiscord.commands.SlashCommands;
using VBotDiscord.config;

namespace VBotDiscord
{
    internal class Program
    {
        public static DiscordClient Client { get; private set; }
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

            Client.UseInteractivity(new InteractivityConfiguration() {
                Timeout = TimeSpan.FromMinutes(2)
            });

            Client.Ready += Client_Ready;
            //Client.MessageCreated += Client_MessageCreated;
            Client.VoiceStateUpdated += Client_VoiceStateUpdated;
            Client.MessageDeleted += Client_MessageDeleted;

            var commandsConfig = new CommandsNextConfiguration() {
                StringPrefixes = new string[] { botConfig.Prefix },
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = false
            };

            Commands = Client.UseCommandsNext(commandsConfig);

            //Register normal commands for bot
            Commands.RegisterCommands<TestCommands>();
            Commands.RegisterCommands<InteractivityCommands>();

            //Register slash commands for bot locally using the server ID
            //Registering slash command globally may take up to 1h to register
            var slashCommandsConfig = Client.UseSlashCommands();
            slashCommandsConfig.RegisterCommands<BasicSlashCommands>(517306024080834560);

            Commands.CommandErrored += Commands_CommandErrored;            

            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        //Catch the exception which is a cooldown for a command
        private static async Task Commands_CommandErrored(CommandsNextExtension sender, CommandErrorEventArgs e) {
            if (e.Exception is ChecksFailedException exception) {
                string timeLeft = string.Empty;

                foreach (var check in exception.FailedChecks) {
                    var cooldown = (CooldownAttribute)check;
                    timeLeft = cooldown.GetRemainingCooldown(e.Context).ToString(@"hh\:mm\:ss");
                }

                var cooldownMessage = new DiscordEmbedBuilder() {
                    Color = DiscordColor.Azure,
                    Title = "Please wait for the cooldown to end",
                    Description = $"Time: {timeLeft}"
                };

                await e.Context.Channel.SendMessageAsync(embed: cooldownMessage);
            }
        }

        private static async Task Client_MessageDeleted(DiscordClient sender, MessageDeleteEventArgs e) {
            var textChannel = await sender.GetChannelAsync(1366843547981971516);
            await textChannel.SendMessageAsync($"Someone deleted this message: {e.Message.Content}");
        }

        private static async Task Client_VoiceStateUpdated(DiscordClient sender, VoiceStateUpdateEventArgs e) {
            if (e.Before == null && e.Channel.Id == 517308747597611008) {
                var textChannel = await sender.GetChannelAsync(1366843547981971516);
                await textChannel.SendMessageAsync($"{e.User.Username} joined the voice channel!");
            }
        }

        //private static async Task Client_MessageCreated(DiscordClient sender, MessageCreateEventArgs e) {
        //    if (e.Author.IsBot) return;
        //    await e.Channel.SendMessageAsync("This event handler was triggered!");
        //}

        private static Task Client_Ready(DiscordClient sender, ReadyEventArgs args) {
            //var appCommands = await sender.GetGuildApplicationCommandsAsync(517306024080834560);
            //foreach (var command in appCommands) {
            //    await sender.DeleteGuildApplicationCommandAsync(517306024080834560, command.Id);
            //    Console.WriteLine($"Deleted global command {command.Name}");
            //}

            return Task.CompletedTask;
        }
    }
}
