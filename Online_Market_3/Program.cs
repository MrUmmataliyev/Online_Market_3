using _25_lesson_TelegramBot_Basic;

try
{
    TelegramBot bot = new TelegramBot();
    await bot.MainFunction();
}
catch (NullReferenceException)
{
    throw new Exception("Ieeee ne ojidenaku");
}
catch (Exception)
{
    throw new Exception("Xe to'palan qimelar");
}

