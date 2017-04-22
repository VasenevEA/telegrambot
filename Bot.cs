using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot;
using System.Threading;

namespace telegrambot
{
    public class Bot
    {
        private Api bot;
        private bool runnable = false;

        public Bot(string token)
        {

        }

        public void start()
        {
            var Bot = new Telegram.Bot.Api("your API access Token");
            var me = Bot.GetMe();
            loop();
        }

        public void stop()
        {

        }


        private void loop()
        {
            while (runnable)
            {

            }
        }




    }
}
