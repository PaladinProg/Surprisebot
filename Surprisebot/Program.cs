using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
//1823203185:AAHYgw1bZMLe6ZAoDYxfFYnC9HI9lvHEg38
namespace Surprisebot
{
    class Program
    {
        private const string TEXT_1 = "Задание 1";
        private const string TEXT_2 = "Задание 2";
        private const string TEXT_3 = "Задание 3";
        private const string TEXT_4 = "Конец";
        /*private const string TEXT_5 = "";
        private const string TEXT_6 = "Нашла (Третье задание)";
        private const string TEXT_7 = "Прислала (Третье задание)";
        private const string TEXT_8 = "Ссылка";*/

        static ITelegramBotClient botclient;//обьявление телеграм клиента
        static void Main(string[] args)
        {
            try
            {
                botclient = new TelegramBotClient("1823203185:AAHYgw1bZMLe6ZAoDYxfFYnC9HI9lvHEg38") { Timeout = TimeSpan.FromSeconds(10) };//таймаут нужен чтобы снизить нагрузку бота на телеграм, 
                                                                                                                                           //если не поставить этот параметр то бот будет создавать большую нагрузку и может быть заблокирован
                var me = botclient.GetMeAsync().Result;
                Console.WriteLine($"Bot ID: {me.Id} Bot Name: {me.FirstName}");//проверка работоспособости бота

                botclient.OnMessage += Bot_OnMessage; //добавляем метод в котором будет функционал
                botclient.StartReceiving();

                Console.ReadKey();
                botclient.StopReceiving();
            }
            catch (Exception ex) { }
        }

        public static async void Bot_OnMessage(object sender, MessageEventArgs e)//используем асинхронный метод чтобы поток данных не блокировался во время его получения
        {
            var text = e?.Message?.Text;//получение текста от пользователя
            var message = e.Message;
            string name = $"{message.From.FirstName} {message.From.LastName}";//получение имени пользователя
            if (message.Type == MessageType.Text)
            {//проверка на тип получаемого сообщения, он должен быть текстовым

                Console.WriteLine($"Поступила инормация: '{text}' из чата: '{e.Message.Chat.Id}' от пользователя: {name}");//проверка того что сообщения доходят до бота

                switch (message.Text)
                {
                    case "/start"://команда старта бота(запускается автоматичекски при первом обращении к боту)
                        await botclient.SendTextMessageAsync(
                            chatId: e.Message.Chat,
                            text: $"Сыграем в игру?", replyMarkup: GetButtons()
                        ).ConfigureAwait(false);
                        break;
                    case TEXT_1:
                        botclient.SendTextMessageAsync(e.Message.Chat.Id, "Первое задание: Найдт QR код и отсканируй. Вот координаты 59.984029, 30.192480. Затем пришли двоичный код с сайта и приступай ко второму заданию", replyMarkup: GetButtons());
                        break;
                    case TEXT_2:
                        botclient.SendTextMessageAsync(e.Message.Chat.Id, "Второе задание: Найди QR код и отсканируй. Вот координаты …  . Пришли двоичный код с сайта и приступай к третьему заданию", replyMarkup: GetButtons());
                        break;
                    case TEXT_3:
                        botclient.SendTextMessageAsync(e.Message.Chat.Id, "Третье задание: Найди QR код и отсканируй. Вот координаты … .Пришли двоичный код с сайта и жми на кнопку конец", replyMarkup: GetButtons());
                        break;
                    case TEXT_4:
                        botclient.SendTextMessageAsync(e.Message.Chat.Id, "Поздравляю! Ты прошла все задания. Теперь перейди по ссылке (ссылка) и переведи весь двоичный код в текст", replyMarkup: GetButtons());
                        break;
                    default:
                        botclient.SendTextMessageAsync(e.Message.Chat.Id, "Пожалуйста следуй инструкции", replyMarkup: GetButtons());
                        break;
                }
            }
        }

        private static IReplyMarkup GetButtons()
        { 
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton>{new KeyboardButton { Text = TEXT_1 }, },
                         new List<KeyboardButton>{new KeyboardButton { Text = TEXT_2 }, },
                            new List<KeyboardButton>{new KeyboardButton { Text = TEXT_3 }, },
                                new List<KeyboardButton>{new KeyboardButton { Text = TEXT_4 }, },
                        
                }
            };
        }   
    }
}
