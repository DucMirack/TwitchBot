using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace TwitchBot
{
    class Program
    {
        private static IrcClient Irc { get; set; }

        static void Main(string[] args)
        {
            //using (IrcClient bot = new IrcClient("irc.twitch.tv", 6667, Identifiant.PSEUDO, Identifiant.OAUTHKEY))
            //{
            //    //bot.Start();

            //    Console.ReadKey();
            //}

            using (Irc = new IrcClient("irc.twitch.tv", 6667, Identifiant.PSEUDO, Identifiant.OAUTHKEY))
            {
                Irc.joinRoom("tharsanhalo");

                while (true)
                {
                    Irc.readMessage();
                }
            }
        }
    }
}
