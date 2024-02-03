using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using i= System.IO;

namespace Online_Market_3.Homework
{
    public class Admins
    {
        public async Task adminsFunction(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                replyToMessageId: update.Message.MessageId,
                text: "----------",
                cancellationToken: cancellationToken


                );
            string fileADDRESS = @"D:\Najot ta'lim\N11\Online_Market_3\Online_Market_3\CategoryFile.txt";
            var readText = i.File.ReadAllText(fileADDRESS);
            Console.WriteLine(readText);

        }
    }
}
