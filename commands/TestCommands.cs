using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace VBotDiscord.commands {
    public class TestCommands : BaseCommandModule {
        [Command("test")]
        public async Task MyFirstCommand(CommandContext ctx) {
            await ctx.Channel.SendMessageAsync($"Hello {ctx.User.Username}");
        }

        [Command("add")]
        public async Task Add(CommandContext ctx) {
            await ctx.Channel.SendMessageAsync("You need to put two numbers after the keyword add. Example: !add 1 1");
        }
        [Command("add")]
        public async Task Add(CommandContext ctx, int number1) {
            await ctx.Channel.SendMessageAsync("You need to put two numbers after the keyword add. Example: !add 1 1");
        }
        [Command("add")]
        public async Task Add(CommandContext ctx, int number1, int number2) {
            await ctx.Channel.SendMessageAsync($"{number1 + number2}");
        }
    }
}
