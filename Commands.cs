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

        [Command("")]
        async Task command()
        {

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
    }
}

       


    
