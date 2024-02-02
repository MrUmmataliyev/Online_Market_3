using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

var botClient = new TelegramBotClient("6782181094:AAEUA1eaHfbIIF96HrQjvpU3cbYnvhTQOI0");

using CancellationTokenSource cts = new();

bool contact = false;

ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>() 
};

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();


cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    

    var handler = update.Type switch
    {
        UpdateType.Message => HandleMessageAsync(botClient, update, cancellationToken),
        UpdateType.CallbackQuery => HandleCallBackQueryAsync(botClient, update, cancellationToken),
        UpdateType.EditedMessage => HandleEditedMessageAsync(botClient, update, cancellationToken),
        
        _ => HandleUnknownUpdateType(botClient, update, cancellationToken),
    };






}
async Task HandleMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    var message = update.Message;
    var user = message.Chat.FirstName;
    var handler = message.Type switch
    {
        MessageType.Text => HandleTextMessageAsync(botClient, update, cancellationToken, user, contact),
        MessageType.Contact => HandleContactMessageAsync(botClient, update, cancellationToken, user),
        _ => HandleUnknownMessageTypeAsync(update, update, cancellationToken),
    };
}


async Task HandleTextMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string user, bool contact)
{
    if (contact != true)
    {
        var chatName = update.Message.Chat.FirstName;
        var messageText = update.Message.Text;
        ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
        {

           KeyboardButton.WithRequestContact("Contactni yuborish"),
        });
        Message sentMessage = await botClient.SendTextMessageAsync(
        chatId: update.Message.Chat.Id,
        text: "Assalomu aleykum botni ishga tushirsh uchun Contactingizni yuboring!",
        replyMarkup: replyKeyboardMarkup,
        cancellationToken: cancellationToken);
        Console.WriteLine($"Received a '{messageText}' message in chat {chatName}.");
    }



}


async Task HandleContactMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string user)
{

    Message sentMessage = await botClient.SendTextMessageAsync(
    chatId: update.Message.Chat.Id,
    text: "Davom etishingiz mumkun",
    replyMarkup: new ReplyKeyboardRemove(),
    cancellationToken: cancellationToken);
    contact = true;



}


async Task HandleCallBackQueryAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.CallbackQuery.Data != null)
    {
        string a = update.CallbackQuery.Data.ToString();
        await botClient.SendTextMessageAsync(
             chatId: update.CallbackQuery.From.Id,

             text: $"{a} UZS",

             cancellationToken: cancellationToken);

    }
}

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}


Task HandleEditedMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    throw new NotImplementedException();
}

async Task HandleUnknownMessageTypeAsync(Update update1, Update update2, CancellationToken cancellationToken)
{
    throw new NotImplementedException();
}



async Task HandleUnknownUpdateType(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    throw new NotImplementedException();
}


