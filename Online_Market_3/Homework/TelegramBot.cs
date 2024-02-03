using Newtonsoft.Json;
using Online_Market_3.Homework;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using io = System.IO;

namespace _25_lesson_TelegramBot_Basic

{
    public class TelegramBot
    {
        TelegramBotClient botClient = new TelegramBotClient("6782181094:AAEUA1eaHfbIIF96HrQjvpU3cbYnvhTQOI0");
        public bool IsEnter { get; set; } = false;
        public async Task MainFunction()
        {
            using CancellationTokenSource cts = new();
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
                try
                {

                    //string jsonAdminFilePath = @"D:\Najot ta'lim\N11\Online_Market_3\Online_Market_3\Adminsbase.json";
                    //var AdminDataList = io.File.ReadAllText(jsonAdminFilePath);

                    //List<Contact> AdminList = JsonConvert.DeserializeObject<List<Contact>>(AdminDataList);
                    //foreach (var itemA in AdminList)
                    //{
                    if (update.Message.Chat.Id== 1417765739)
                    {
                        Admins ToAdmin = new Admins();
                        await ToAdmin.adminsFunction(botClient, update, cancellationToken);
                        
                    }

                    //}



                    string jsonFilePath = @"D:\Najot ta'lim\N11\\Online_Market_3\Online_Market_3\users.json";
                    var dataList = io.File.ReadAllText(jsonFilePath);

                    List<Contact> list = JsonConvert.DeserializeObject<List<Contact>>(dataList);

                    foreach (var item in list)
                    {

                        if (item.UserId == update.Message.Chat.Id)
                        {
                            IsEnter = true;
                            break;
                        }
                        else
                        {
                            IsEnter = false;
                            if (update.Message.Contact is not null && item.PhoneNumber != update.Message.Contact.PhoneNumber)
                            {
                                list.Add(update.Message.Contact);

                                var data = io.File.ReadAllText(jsonFilePath);

                                using (StreamWriter sw = new StreamWriter(jsonFilePath))
                                {
                                    sw.WriteLine(JsonConvert.SerializeObject(list, Formatting.Indented));
                                }
                                IsEnter = true;
                                break;
                            }
                        }
                    }


                   

                    var handler = update.Type switch
                    {
                        UpdateType.Message => Message.MessageAsyncFunction(botClient, update, cancellationToken, IsEnter),
                        _ => Message.MessageAsyncFunction(botClient, update, cancellationToken, IsEnter),
                    };
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    var handler = update.Type switch
                    {
                        UpdateType.Message => Message.Unknown(botClient, update, cancellationToken),
                        _ => Message.Unknown(botClient, update, cancellationToken),
                    };
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
        }
    }
}
