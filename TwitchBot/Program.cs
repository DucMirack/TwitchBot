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
            IrcClient irc = new IrcClient("irc.twitch.tv", 6667, "ducmirack", "oauth:qfcb61okru4jyynjjtlz2qvxe2t07e");
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
