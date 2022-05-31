using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;

namespace Mr.Bot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        //A test command to check if the bot is online
        [Command("ping")]
        public async Task Ping()
        {
            ReplyAsync("pong");
        }

        //ban's the specified user
        [Command("ban")]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "you don't have the permission to ``ban_member``!")]
        public async Task BanMember(IGuildUser user = null, [Remainder] string reason = null)
        {
            if (user == null)
            {
                await ReplyAsync("please specify a user!");
                return;
            }
            if (reason == null) reason = "Not specified";

            await Context.Guild.AddBanAsync(user, 1, reason);

            //Embed for user unban log
            var EmbedBuilder = new EmbedBuilder()
            .WithDescription($":white_check_mark: {user.Mention} was banned\n**Reason** {reason}")
            .WithFooter(footer =>
            {
                footer
                .WithText("User Ban Log")
                .WithIconUrl("https://i.imgur.com/6Bi1783");
            });
            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);

            ITextChannel logChannel = Context.Client.GetChannel(973986221091061803) as ITextChannel;
            var EmbedBuilderLog = new EmbedBuilder()
            .WithDescription($"{user.Mention} was banned\n**Reason** {reason}\n**moderator** {Context.User.Mention}")
            .WithFooter(footer =>
            {
                footer
                .WithText("User Ban Log")
                .WithIconUrl("https://i.imgur.com/6Bi1783");
            });
            Embed embedLog = EmbedBuilderLog.Build();
            logChannel.SendMessageAsync(embed: embedLog);
        }

        //gives a link to Rick Astley's "Never Gonna Give You Up"
        [Command("best song")]
        async Task song()
        {
            await ReplyAsync("searching...");
            await ReplyAsync("https://bit.ly/gameee12");
        }

        //kicks the specified user
        [Command("kick")]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "you don't have the permission to ``kick_member``!")]
        async Task KickMember(IGuildUser user = null, [Remainder] string reason = null)
        {
            if (user == null)
            {
                await ReplyAsync("please specify a user!");
                return;
            }
            if (reason == null) reason = "Not specified";

            await user.KickAsync(reason);

            //Embed for user unban log
            var EmbedBuilder = new EmbedBuilder()
            .WithDescription($":white_check_mark: {user.Mention} was kicked")
            .WithFooter(footer =>
            {
                footer
                .WithText("User Kick Log")
                .WithIconUrl("https://i.imgur.com/6Bi1783");
            });
            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);

            ITextChannel logChannel = Context.Client.GetChannel(973986221091061803) as ITextChannel;
            var EmbedBuilderLog = new EmbedBuilder()
            .WithDescription($"{user.Mention} was kicked\n**moderator** {Context.User.Mention}")
            .WithFooter(footer =>
            {
                footer
                .WithText("User  Log")
                .WithIconUrl("https://i.imgur.com/6Bi1783");
            });
            Embed embedLog = EmbedBuilderLog.Build();
            logChannel.SendMessageAsync(embed: embedLog);
        }


        //Unban's the specified user
        [Command("unban")]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "you don't have the permission to ``unban_member``!")]
        async Task UnbanMember(IGuildUser user = null, [Remainder] string reason = null)
        {
            if (user == null)
            {
                await ReplyAsync("please specify a user!");
                return;
            }
            if (reason == null) reason = "Not specified";

            await Context.Guild.RemoveBanAsync(user);

            //Embed for user unban log
            var EmbedBuilder = new EmbedBuilder()
            .WithDescription($":white_check_mark: {user.Mention} was unbanned")
            .WithFooter(footer =>
            {
                footer
                .WithText("User Unban Log")
                .WithIconUrl("https://i.imgur.com/6Bi1783");
            });
            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);

            ITextChannel logChannel = Context.Client.GetChannel(973986221091061803) as ITextChannel;
            var EmbedBuilderLog = new EmbedBuilder()
            .WithDescription($"{user.Mention} was unbanned\n**moderator** {Context.User.Mention}")
            .WithFooter(footer =>
            {
                footer
                .WithText("User Unban Log")
                .WithIconUrl("https://i.imgur.com/6Bi1783");
            });
            Embed embedLog = EmbedBuilderLog.Build();
            logChannel.SendMessageAsync(embed: embedLog);
        }


        //get's the user name and ID
        [Command("userinfo")]
        [Summary("Returns info about the current user, or the user parameter, if one passed.")]
        [Alias("user", "whois")]
        public async Task UserInfoAsync(
        [Summary("The (optional) user to get info from")]
        SocketUser user = null)
        {
            var userInfo = user ?? Context.Client.CurrentUser;
            await ReplyAsync($"{userInfo.Username}#{userInfo.Discriminator}");
        }


        //repeat's whatever is said after the command(announce)
        [Command("announce")]
        [Summary("Echoes a message.")]
        public Task SayAsync([Remainder][Summary("The text to echo")] string echo)
        => ReplyAsync(echo);


        //give's the user the GitHub repository of the bot
        [Command("insider code")]
        async Task Code()
        {
            await ReplyAsync("Just a sec...\nfetching data...");
            await ReplyAsync("...");
            await ReplyAsync("https://github.com/beepboi12/Grey-god");
        }

        [Command("Watch")]
        public async Task Watch()
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle("Nobody better be breaking any rules")
            .WithDescription($"He's watching")
            .WithColor(Color.DarkPurple);

            await ReplyAsync("", false, builder.Build());
        }
        
        [Command("greetings")]
        public async Task greetings()
        {
            await ReplyAsync("** **");
            await ReplyAsync("I don't want to talk to you b!&*#");
        }

        [Command("del")]
        [Alias("clean")]
        [Summary("Downloads and removes X messages from the current channel.")]
        [RequireUserPermission(ChannelPermission.ManageMessages)]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task PurgeAsync(int amount)
        {
            // Check if the amount provided by the user is positive.
            if (amount <= 0)
            {
                await ReplyAsync("The amount of messages to remove must be positive.");
                return;
            }

            // Download X messages starting from Context.Message, which means
            // that it won't delete the message used to invoke this command.
            var messages = await Context.Channel.GetMessagesAsync(Context.Message, Direction.Before, amount).FlattenAsync();

            // Note:
            // FlattenAsync() might show up as a compiler error, because it's
            // named differently on stable and nightly versions of Discord.Net.
            // - Discord.Net 1.x: Flatten()
            // - Discord.Net 2.x: FlattenAsync()

            // Ensure that the messages aren't older than 14 days,
            // because trying to bulk delete messages older than that
            // will result in a bad request.
            var filteredMessages = messages.Where(x => (DateTimeOffset.UtcNow - x.Timestamp).TotalDays <= 14);

            // Get the total amount of messages.
            var count = filteredMessages.Count();

            // Check if there are any messages to delete.
            if (count == 0)
                await ReplyAsync("What should I delete idiot");

            else
            {
                // The cast here isn't needed if you're using Discord.Net 1.x,
                // but I'd recommend leaving it as it's what's required on 2.x, so
                // if you decide to update you won't have to change this line.
                await (Context.Channel as ITextChannel).DeleteMessagesAsync(filteredMessages);
                await ReplyAsync($"Done. Removed {count} {(count > 1 ? "messages" : "message")}.");
            }
        }
    }
}

       


    
