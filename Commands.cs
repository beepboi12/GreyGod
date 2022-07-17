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

        // Mute Command:
        [Command("mute")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task Mute(IGuildUser user, int duration, string reason)
        {
            var role = await Context.Guild.CreateRoleAsync("Muted");
            var Role = new GuildPermissions(false,
                                                         false,
                                                         false,
                                                         false,
                                                         false,
                                                         false,
                                                         false,
                                                         false,
                                                         false, // this one is for sendMessages
                                                         false,
                                                         false,
                                                         false,
                                                         false,
                                                         false,
                                                         true,
                                                         false,
                                                         false,
                                                         true,
                                                         false,
                                                         false,
                                                         false,
                                                         false,
                                                         false,
                                                         false,
                                                         false,
                                                         false,
                                                         false,
                                                         false,
                                                         false);
            await role.DeleteAsync();
        }

        //ban's the specified user
        [Command("ban")]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "you don't have the permission to ``ban_member``!")]
        [RequireBotPermission(GuildPermission.BanMembers, ErrorMessage = "Please give the permission to ``ban_member``!")]
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
        [RequireBotPermission(GuildPermission.BanMembers, ErrorMessage = "Please give the permission to ``kick_member``!")]
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
        [RequireBotPermission(GuildPermission.BanMembers, ErrorMessage = "Please give the permission to ``unban_member``!")]
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
        [RequireBotPermission(GuildPermission.BanMembers, ErrorMessage = "Please give the permission to ``announce``!")]
        public Task SayAsync([Remainder][Summary("The text to echo")] string echo)
        => ReplyAsync(echo);

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
            await ReplyAsync("**REALLY DUD**");
            await ReplyAsync("I DON'T WANT TO TALK TO YOU B!&*#");
        }
        
        [Command("play")]
        [Alias(";)")]
        public async Task play()
        {
            ReplyAsync("what do you wanna play, nothing sus tho");

        }
        
        [Command("commands")]
        public async Task cmd()
        {
            EmbedBuilder commands = new EmbedBuilder();

            commands.WithTitle("Commands")
            .WithDescription($" play\n greetings\n watch\n insider code\n announce\n userinfo\n unban,ban,kick(only available for admins)\n best song")
            .WithColor(Color.Orange);

            await ReplyAsync("", false, commands.Build());
    }
}
