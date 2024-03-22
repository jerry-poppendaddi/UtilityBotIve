using System.IO.IsolatedStorage;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


namespace UtilityBotIve.Controllers
{
    public class InlineKeyboardController
    {
        private readonly ITelegramBotClient _telegramClient;
        public InlineKeyboardController(ITelegramBotClient telegramBotClient)
        {
            _telegramClient = telegramBotClient;            
        }
        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            if (callbackQuery?.Data == null)
                return;           

            // Генерим информационное сообщение
            string textOrNum = callbackQuery.Data switch
            {
                "txt" => "Считать Буквы",
                "num" => "Сумма чисел",
                _ => String.Empty
            };

            // Отправляем в ответ уведомление о выборе
            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                $"<b>Вы выбрали - {textOrNum}.{Environment.NewLine}</b>" +
                $"{Environment.NewLine}Если желаете - можете поменять ваш выбор в главном меню.", cancellationToken: ct, parseMode: ParseMode.Html);
        }

    }
}
