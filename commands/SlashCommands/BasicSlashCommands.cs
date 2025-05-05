using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using VBotDiscord.other.Services;

namespace VBotDiscord.commands.SlashCommands {
    public class BasicSlashCommands : ApplicationCommandModule {
        [SlashCommand("animequote","Get the random quote from anime characters")]
        public async Task AnimeCharacterQuote(InteractionContext ctx, [Option("character", "Type in the anime character name")] string animeCharacter) {
            await ctx.DeferAsync();

            var quoteService = new AnimeQuoteService();
            var (quote, error) = await quoteService.GetRandomQuoteAsync(animeCharacter);

            if (error != null) {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent(error));
                return;
            }

            var embedMessage = new DiscordEmbedBuilder() {
                Color = DiscordColor.Azure,
                Title = $"\"{quote!.Quote}\"",
                Description = $"- {quote!.Character} - {quote!.Show}"
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
        }       
    }
}
