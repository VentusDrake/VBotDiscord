using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using VBotDiscord.other;

namespace VBotDiscord.commands {
    public class TestCommands : BaseCommandModule {
        [Command("test")]
        public async Task MyFirstCommand(CommandContext ctx) {
            await ctx.Channel.SendMessageAsync($"Hello {ctx.User.Username}");
        }

        [Command("add")]
        public async Task Add(CommandContext ctx, int number1, int number2) {
            await ctx.Channel.SendMessageAsync($"{number1 + number2}");
        }

        [Command("embed")]
        public async Task EmbedMessage(CommandContext ctx) {
            var message = new DiscordEmbedBuilder {
                Title = "This is my first discord embed",
                Color = DiscordColor.White,
                Description = $"This command was executed by {ctx.User.Username}",
                Timestamp = DateTime.Now
            };

            await ctx.Channel.SendMessageAsync(embed: message);
        }

        [Command("cardgame")]
        public async Task CardGame(CommandContext ctx) {
            var userCard = new CardSystem();
            var userCardEmbed = new DiscordEmbedBuilder {
                Title = $"Your card is {userCard.SelectedCard}",
                Color = DiscordColor.Lilac
            };
            await ctx.Channel.SendMessageAsync(embed: userCardEmbed);

            var botCard = new CardSystem();
            var botCardEmbed = new DiscordEmbedBuilder {
                Title = $"Bot card is {botCard.SelectedCard}",
                Color = DiscordColor.Aquamarine
            };
            await ctx.Channel.SendMessageAsync(embed: botCardEmbed);

            await ctx.Channel.SendMessageAsync(embed: CheckCardGameWinner(userCard, botCard));
        }

        private DiscordEmbedBuilder CheckCardGameWinner(CardSystem userCard, CardSystem botCard) {
            if (userCard.SelectedNumber > botCard.SelectedNumber) {
                return new DiscordEmbedBuilder {
                    Title = "Congratulations! You win!",
                    Color = DiscordColor.Green
                };
            } else if (userCard.SelectedNumber < botCard.SelectedNumber) {
                return new DiscordEmbedBuilder {
                    Title = "You Lost!",
                    Color = DiscordColor.Red
                };
            } else {
                return new DiscordEmbedBuilder {
                    Title = "It's a draw!",
                    Color = DiscordColor.White
                };
            }
        }        
    }
}
