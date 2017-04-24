using System;
using System.Threading;


namespace telegrambot
{
    class Program
    {
        static void Main(string[] args)
        {
            Bot.start(Service.readConfig().token);

            Console.ReadKey();
        }
    }
}
