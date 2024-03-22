using Telegram.Bot;
using Telegram.Bot.Types;

namespace UtilityBotIve.Controllers
{
    public class DefaultMessageController
    {
        private readonly ITelegramBotClient _telegramClient;

        public DefaultMessageController(ITelegramBotClient telegramBotClient)
        {
            _telegramClient = telegramBotClient;
        }
        public async Task Handle(Message message, CancellationToken ct)
        {
            Console.WriteLine($"Controller {GetType().Name} received a message");
            await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Данный формат ввода не поддерживается", cancellationToken: ct);
        }
    }

}