using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Telegram.Bot.Types.ReplyMarkups;

namespace Surprisebot
{
    internal class TelegramBotHelper
    {
        private const string TEXT_1 = "Задание 1";
        private const string TEXT_2 = "Задание 2";
        private const string TEXT_3 = "Задание 3";
        private const string TEXT_4 = "Задание 4";
        private string _token;
        Telegram.Bot.TelegramBotClient _client;
        public TelegramBotHelper(string token)
        {
            this._token = token;
        }

        internal void GetUpdates()
        {
            _client = new Telegram.Bot.TelegramBotClient(_token);
            var me = _client.GetMeAsync().Result;
            if (me != null && !string.IsNullOrEmpty(me.Username))
            {
                while (true)
                {
                    try
                    {
                        var updates = _client.GetUpdatesAsync().Result;
                        if (updates != null && updates.Count() > 0)
                        {
                            foreach(var update in updates)
                            {
                                processUpdate(update);
                            }
                        }
                    }
                    catch(Exception ex) {   Console.WriteLine(ex.Message);  }

                    Thread.Sleep(1000);
                }
            }
        }

        private void processUpdate(Telegram.Bot.Types.Update update)
        {
            switch (update.Type)
            {
                case Telegram.Bot.Types.Enums.UpdateType.Message:
                    var text = update.Message.Text;
                    switch (text)
                    {
                        case TEXT_1:
                            _client.SendTextMessageAsync(update.Message.Chat.Id, "Текст задания 1", replyMarkup: GetButtons());
                            break;
                        case TEXT_2:
                            _client.SendTextMessageAsync(update.Message.Chat.Id, "Текст задания 2", replyMarkup: GetButtons());
                            break;
                        case TEXT_3:
                            _client.SendTextMessageAsync(update.Message.Chat.Id, "Текст зажания 3", replyMarkup: GetButtons());
                            break;
                        case TEXT_4:
                            _client.SendTextMessageAsync(update.Message.Chat.Id, "Текст задания 4", replyMarkup: GetButtons());
                            break;
                    }
                    break;
                default:
                    Console.WriteLine(update.Type + "не обрабатываемый тип сообщения");
                    break;
            }
        }

        private IReplyMarkup GetButtons()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton>{new KeyboardButton { Text = TEXT_1 }, new KeyboardButton { Text = TEXT_2 }, },
                        new List<KeyboardButton>{new KeyboardButton { Text = TEXT_3 }, new KeyboardButton { Text = TEXT_4 }, }
                  
                }
            };
        }
    }
}