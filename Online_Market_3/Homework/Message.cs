using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace _25_lesson_TelegramBot_Basic
{
    public class Message
    {
        public static async Task MessageAsyncFunction(ITelegramBotClient botClient, Update update,
            CancellationToken cancellationToken, bool isEnter)
        {
            var message = update.Message;
            Console.WriteLine($"User Name: {message.Chat.Username}\nYou said: {message.Text}\nData: {DateTime.Now}\n");
            if (isEnter == true)
            {
                var handler = message.Type switch
                {
                    MessageType.Text => TextAsyncFunction(botClient, update, cancellationToken),


                    MessageType.Contact => ContactAsyncFunction(botClient, update, cancellationToken),
                    _ => OtherAsyncFunctiob(botClient, update, cancellationToken)
                };
            }
            else
            {
                Contact(botClient, update, isEnter).Wait();
            }
        }

        static async Task ContactAsyncFunction(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var message = update.Message;
            await botClient.SendTextMessageAsync
            (
                chatId: message.Chat.Id,
                text: $"Hush kelibsiz {message.Chat.FirstName}!",
                replyToMessageId: message.MessageId,
                replyMarkup: new ReplyKeyboardRemove()
            );
        }

        static async Task Contact(ITelegramBotClient botClient, Update update, bool isEnter)
        {
            Console.WriteLine(isEnter);
            ReplyKeyboardMarkup markup = new ReplyKeyboardMarkup
            (
                KeyboardButton.WithRequestContact("Kontact yuborish uchun tegining")
            );

            markup.ResizeKeyboard = true;
            await botClient.SendTextMessageAsync
            (
                    chatId: update.Message.Chat.Id,
                    text: "Iltimos oldin telefon raqamingizni yuboring!",
                    replyMarkup: markup
            );

        }
        static async Task TextAsyncFunction(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var message = update.Message;
            await botClient.SendTextMessageAsync
            (
                chatId: message.Chat.Id,
                replyToMessageId: message.MessageId,
                text: "Ehe salomlar",
                cancellationToken: cancellationToken
            );
        }



        static async Task OtherAsyncFunctiob(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var message = update.Message;
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                replyToMessageId: message.MessageId,
                text: "Nimadir xato ketdi.",
                cancellationToken: cancellationToken);
        }

        public static async Task Unknown(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var message = update.Message;
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                replyToMessageId: message.MessageId,
                text: "Spam bosma",
                cancellationToken: cancellationToken);
        }
    }
}
