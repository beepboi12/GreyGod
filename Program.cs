using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace Mr.Bot
{ 
    class Program
    {
        static void Main (string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _Client;
        private CommandService _Commands;
        private IServiceProvider _services;

        public async Task RunBotAsync()
        {
            _Client = new DiscordSocketClient();
            _Commands = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(_Client)
                .AddSingleton(_Commands)
                .BuildServiceProvider();

            string token = "OTczOTg2ODcyMzQ5MDMyNTE4.Gg_tX0.0fIsGkPJ--sDvWZCzhC7zgP-KYqm02_XG9sA6U";

            _Client.Log += _Client_Log1;

            await RegisterCommandsAsync();

            await _Client.LoginAsync(TokenType.Bot, token);

            await _Client.StartAsync();

            await Task.Delay(-1);
     
        }

        private Task _Client_Log1(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            _Client.MessageReceived += HandleCommandsAsync;
            await _Commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommandsAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var Context = new SocketCommandContext(_Client, message);
            if (message.Author.IsBot) return;

            int argPos = 0;
            if(message.HasStringPrefix("plz ", ref argPos))
            {
                var result = await _Commands.ExecuteAsync(Context, argPos, _services);
                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
                if (result.Error.Equals(CommandError.UnmetPrecondition)) await message.Channel.SendMessageAsync(result.ErrorReason);
            }
        }
    }
}

