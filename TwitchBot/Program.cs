using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot
{
    class Program
    {


        static void Main(string[] args)
        {
            IrcClient irc = new IrcClient("irc.twitch.tv", 6667, Identifiant.PSEUDO, Identifiant.OAUTHKEY);
            irc.joinRoom("tharsanhalo");

            //********* PARTIE MESSAGE PERIODIQUE ***************//
            var timer = new System.Threading.Timer((e) =>
            {
                irc.sendChatMessage("Bienvenue, Streamers !");
            }, null, 0, TimeSpan.FromMinutes(1).Minutes);

            while (true)
            {
                string message = irc.readMessage();
                if (message != null)
                {
                    if (message.Contains("!hello"))
                    {
                        irc.sendChatMessage("Yo yo");
                    }
                }
            }
        }
    }
}
