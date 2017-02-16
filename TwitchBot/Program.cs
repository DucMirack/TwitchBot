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
            irc.joinRoom("ducmirack");
            while (true)
            {
                string message = irc.readMessage();
                if (message.Contains("!hello"))
                {
                    irc.sendChatMessage("Yo yo");
                }
            }
        }
    }
}
