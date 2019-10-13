﻿using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace Discord_Bot.Events
{
    public static class Message_Received
    {
        public static void Load()
        {
            Program.Client.MessageReceived += Client_MessageReceived;
        }

        private async static Task Client_MessageReceived(SocketMessage message)
        {
            SocketUserMessage Message = message as SocketUserMessage;
            SocketCommandContext Context = new SocketCommandContext(Program.Client, Message);

            if (Context.Message == null || Context.Message.Content == "") return;
            if (Context.User.IsBot) return;

            string Prefix = Utils.GetPrefix(Context);

            int ArgPos = 0;
            if (!(Message.HasStringPrefix(Prefix, ref ArgPos) || Message.HasMentionPrefix(Program.Client.CurrentUser, ref ArgPos))) return;
            if (Context.Message.ToString() == Prefix) return;

            IResult Result = await Program.Commands.ExecuteAsync(Context, ArgPos, null);

            if (!Result.IsSuccess)
            {
                Console.WriteLine($"{DateTime.Now} at Commands] Something went wrong with a command. Message: {Context.Message.Content} | Error: {Result.ErrorReason}", System.Drawing.Color.DarkRed);
                string err = Result.ErrorReason;
                await Context.Channel.SendMessageAsync(err == "Unknown command." ? $"Unknown Command! Use {Prefix}Help to see the commands." : err);
            }
        }
    }
}
