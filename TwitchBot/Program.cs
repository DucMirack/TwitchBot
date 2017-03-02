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
            IrcClient irc = new IrcClient("irc.chat.twitch.tv", 6667, Identifiant.PSEUDO, Identifiant.OAUTHKEY);
            irc.joinRoom("ducmirack");
            while (true)
            {
                Message message = Message.Parse(irc.readMessage());
                if (message is ServerMessage)
                {
                    ServerMessage my_message = message as ServerMessage;
                    //traitement pour un serverMessage
                }
                if (message is ChatMessage)
                {
                    ChatMessage my_message = message as ChatMessage;
                    //traitement pour un chatMessage
                    Console.WriteLine(my_message.Text);
                }

                //if (message != null)
                //{
                //    Console.WriteLine(message.ToString());
                //}
            }
        }
    }
}
