using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBotDiscord.commands {
    internal class InteractivityCommands : BaseCommandModule {
        private InteractivityExtension interactivity = Program.Client.GetInteractivity();

        [Command("message")]
        public async Task WaitForMessage(CommandContext ctx) {
            var messageToRetrieve = await interactivity.WaitForMessageAsync(message => message.Content == "Hello");

            if (messageToRetrieve.Result.Content == "Hello") {
                await ctx.Channel.SendMessageAsync($"{ctx.User.Username} said Hello");
            }
        }

        [Command("reaction")]
        public async Task WaitForReaction(CommandContext ctx) {
            var messageToReact = await interactivity.WaitForReactionAsync(message => message.Message.Id == 1368587896843534538);

            if (messageToReact.Result.Message.Id == 1368587896843534538) {
                await ctx.Channel.SendMessageAsync($"{ctx.User.Username} used the emoji with name {messageToReact.Result.Emoji.Name}");
            }
        }

        [Command("Poll")]
        public async Task Poll(CommandContext ctx, string option1, string option2, string option3, string option4, [RemainingText] string pollTitle) {
            DiscordEmoji[] emojiOptions = { DiscordEmoji.FromName(Program.Client, ":one:"),
                                            DiscordEmoji.FromName(Program.Client, ":two:"),
                                            DiscordEmoji.FromName(Program.Client, ":three:"),
                                            DiscordEmoji.FromName(Program.Client, ":four:")};

            string optionsDescription = $"{emojiOptions[0]} | {option1} \n" +
                                        $"{emojiOptions[1]} | {option2} \n" +
                                        $"{emojiOptions[2]} | {option3} \n" +
                                        $"{emojiOptions[3]} | {option4}";

            var pollMessage = new DiscordEmbedBuilder {
                Color = DiscordColor.Rose,
                Title = pollTitle,
                Description = optionsDescription,
            };

            var sentPoll = await ctx.Channel.SendMessageAsync(embed: pollMessage);
            foreach (var emoji in emojiOptions) {
                await sentPoll.CreateReactionAsync(emoji);
            }

            var pollTime = TimeSpan.FromSeconds(10);
            var totalReactions = await interactivity.CollectReactionsAsync(sentPoll, pollTime);
            int count1 = 0;
            int count2 = 0;
            int count3 = 0;
            int count4 = 0;

            foreach (var emoji in totalReactions) {
                if (emoji.Emoji == emojiOptions[0]) count1++;
                if (emoji.Emoji == emojiOptions[1]) count2++;
                if (emoji.Emoji == emojiOptions[2]) count3++;
                if (emoji.Emoji == emojiOptions[3]) count4++;
            }

            int totalVotes = count1 + count2 + count3 + count4;
            string resultsDescription = $"{emojiOptions[0]}: {count1} votes! \n" +
                                        $"{emojiOptions[1]}: {count2} votes! \n" +
                                        $"{emojiOptions[2]}: {count3} votes! \n" +
                                        $"{emojiOptions[3]}: {count4} votes! \n\n" +
                                        $"Total Votes: {totalVotes}";

            var resultEmbed = new DiscordEmbedBuilder {
                Color = DiscordColor.Green,
                Title = "Results of the poll!",
                Description = resultsDescription
            };

            await ctx.Channel.SendMessageAsync(embed: resultEmbed);
        }
    }
}
