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
            using (Irc = new IrcClient("irc.twitch.tv", 6667, Identifiant.PSEUDO, Identifiant.OAUTHKEY))
            {
                Irc.joinRoom("tharsanhalo");

                //********* MESSAGE PERIODIQUE ***************//
                MessagePeriodique.initTimer(Irc);

                while (true)
                {
                    string message = Irc.readMessage();
                    if (message != null)
                    {
                        if (message.Contains("!hello"))
                        {
                            Irc.sendChatMessage("Yo yo");
                        }
                    }
                }
            }
        }
    }
}
