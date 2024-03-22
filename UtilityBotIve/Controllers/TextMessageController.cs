using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using Polly;

namespace UtilityBotIve.Controllers
{
    public class TextMessageController
    {
        private readonly ITelegramBotClient _telegramClient;

        public TextMessageController(ITelegramBotClient telegramBotClient)
        {
            _telegramClient = telegramBotClient;
        }
        bool afterCommand = false;                                         //я так делаю, в общем чтобы всё работало ниже =В
                                                                           //моя цель была чтобы бот выдавал ошибку если введено что то кроме команды старт
                                                                           //но чтобы после введения команды старт он распознавал текст
        public async Task Handle(Message message, CancellationToken ct)
        {
            if (message.Text.StartsWith("/start"))
            {
                afterCommand = true;
                // Объект, представляющий кнопки
                var buttons = new List<InlineKeyboardButton[]>();
                buttons.Add(new[]
                {
                     InlineKeyboardButton.WithCallbackData($"Считать Буквы" , $"txt"),
                     InlineKeyboardButton.WithCallbackData($"Сумма чисел" , $"num")
                });
                // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b>  Наш бот любит считать :) </b> {Environment.NewLine}" +
                    $"{Environment.NewLine}Может посчитать количество букв или сумму цифр.{Environment.NewLine}", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
            }  
            else if (afterCommand)
            {
                bool onlyLetters = true;
                bool onlyNumbers = true;
                bool isCommand = message.Text.StartsWith('/');

                foreach (char c in message.Text)
                {
                    if (!char.IsLetter(c))
                    { onlyLetters = false; }
                    if (!char.IsDigit(c))
                    { onlyNumbers = false; }
                }

                if (onlyLetters)
                {
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Длина сообщения: {message.Text.Length} знаков", cancellationToken: ct);
                }

                if (onlyNumbers)
                {
                    long number = Convert.ToInt64(message.Text);
                    long sum = 0;
                    while (number > 0)
                    {
                        long digit = number % 10;
                        sum += digit;
                        number /= 10;
                    }
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $" Сумма введённых цифр: {sum}", cancellationToken: ct);
                }

                if (!onlyLetters && !onlyNumbers && !isCommand)
                {
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $" Неправильный формат ввода", cancellationToken: ct);
                }
                afterCommand = false;
            }
            else
            {
                await _telegramClient.SendTextMessageAsync(message.Chat.Id, $" Неправильная команда", cancellationToken: ct);              
            }                                  
        }                   
    }         
}



