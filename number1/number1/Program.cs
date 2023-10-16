using Microsoft.VisualBasic;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

var botClient = new TelegramBotClient("5964376114:AAFaTZP6reVQJPzmFCObZ7q-fRo8mkmN1Qc");

using CancellationTokenSource cts = new();

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
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

// Send cancellation request to stop bot
cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)

{
    // Only process Message updates: https://core.telegram.org/bots/api#message
    if (update.Message is not { } message)
        return;
    // Only process text messages
    if (message.Text is not { } messageText)
        return;

    var chatId = message.Chat.Id;
    var asort = "2.Плюшевый мишка\r\n" + "3.Зайчик\r\n" + "4.Мартышка с котиком(продаются отдельно)\r\n" + "5.Игристая пчёлка\r\n";
    var price = "Плюшевый мишка 3000р\r\n" + "Зайчик 400р\r\n" + "мартышка 600р\r\n" + "Котик 750р\r\n"; //Переменные глобальные
    var contact = "Контакты продавца https://t.me/Djanetk";
    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
{
    new KeyboardButton[] { "Ассортимент", "Фотки игрушек", "Видео с витрины", "Контакты","Цены"}, //кнопки
})
    {
        ResizeKeyboard = true
    };

    Message sentMessage = await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: "❤️",
        replyMarkup: replyKeyboardMarkup,
        cancellationToken: cancellationToken);
    Message sentMessage1 = await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: "ㅤ",
        cancellationToken: cancellationToken);

    if (messageText == "Контакты")
    {
        await botClient.SendTextMessageAsync(chatId, contact, cancellationToken: cancellationToken); //вывод сообщения
    }
    if (messageText == "Ассортимент")
    {
        await botClient.SendTextMessageAsync(chatId, "В продаже:\r\n" + "1.Кукла Барби\r\n" + asort, cancellationToken: cancellationToken);
    }
    if (messageText == "Видео с витрины") //вывод видео
    {

        await botClient.SendVideoAsync(
        chatId: chatId,
            video: InputFile.FromUri("https://github.com/Esiw12/BOT/raw/main/karim%20photo's/videostudiya-infomult-2d-animaciya-dlya-magazina-igrusek-pyaterkin_(VIDEOMIN.NET).mp4"),

        thumbnail: InputFile.FromUri("https://github.com/Esiw12/BOT/raw/main/karim%20photo's/videostudiya-infomult-2d-animaciya-dlya-magazina-igrusek-pyaterkin_(VIDEOMIN.NET).mp4"),
        supportsStreaming: true,
        cancellationToken: cancellationToken);
    }
    if (messageText == "Фотки игрушек") //вывод фоток
    {
        //var foto = "https://github.com/Karim1905/pepo/blob/main/1644939663_1-fikiwiki-com-p-igrushki-krasivie-kartinki-1.jpg";
        //InputFileUrl inputOnlineFile = new(foto);
        //await botClient.SendPhotoAsync(chatId, inputOnlineFile);
        await botClient.SendMediaGroupAsync(
        chatId: chatId,
    media: new IAlbumInputMedia[]
    {
                    new InputMediaPhoto(
                    InputFile.FromUri("https://github.com/Karim1905/pepo/blob/main/1644939663_1-fikiwiki-com-p-igrushki-krasivie-kartinki-1.jpg?raw=true")),
                    new InputMediaPhoto(
                    InputFile.FromUri("https://github.com/Karim1905/pepo/blob/main/CB-BwY-CFw4.jpg?raw=true")),
    },
    cancellationToken: cancellationToken);
    }
    if (messageText == "Цены")
    {
        await botClient.SendTextMessageAsync(chatId, price, cancellationToken: cancellationToken);


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