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

        //**** VARIABLE POUR LE TIRAGE AU SORT ********//
        private static List<string> users;
        private static bool isStartTirageSort = false;
        //*******//
        private static IrcClient Irc { get; set; }

        static void Main(string[] args)
        {
            using (Irc = new IrcClient("irc.twitch.tv", 6667, Identifiant.PSEUDO, Identifiant.OAUTHKEY))
            {
                Irc.joinRoom("tharsanhalo");

                //********* MESSAGE PERIODIQUE ***************//
                MessagePeriodique.initTimer(Irc);

                //********** SYSTEME DE VOTE ****************//
                SystemVote systemVote = new SystemVote(Irc);

                while (true)
                {
                    Message message = Message.Parse(Irc.readMessage());
                    if (message is ServerMessage) { //traitement pour un serverMessage
                        ServerMessage my_message = message as ServerMessage;
                    }
                    if (message is ChatMessage) //traitement pour un chatMessage
                    {
                        ChatMessage my_message = message as ChatMessage;

                        if (my_message.Text != null)
                        {
                            //**** SYSTEME DE VOTE
                            if (my_message.Text.Contains("!vote"))  // Test msg : !vote Quelle jeu aimez-vous ? witcher assassin pacman
                            {
                                string propositionAll = my_message.Text.Substring(my_message.Text.IndexOf('?') + 2);
                                systemVote.startVote(propositionAll);
                            }

                            if (systemVote.isStartVoting)
                            {
                                // Terminer le vote
                                if (my_message.Text.Contains("!endvote")){
                                    systemVote.endVote();
                                }

                                systemVote.setValProposition(my_message.Text);
                            }
                            //*******//
                            if (isStartTirageSort && users != null) {
                                if (users.BinarySearch(my_message.UserName) < 0) {
                                    users.Add(my_message.UserName);
                                }
                            }
                            
                            if (my_message.Text.Contains("!tiragesort")){
                                users = new List<string>();
                                isStartTirageSort = true;
                                Irc.sendChatMessage("/me Le tirage au sort commence !!! ");
                            }
                            if (my_message.Text.Contains("!endtiragesort"))
                            {
                                Random random = new Random();
                                int rand = random.Next(users.Count);
                                Irc.sendChatMessage("/me Le gagnant du tirage au sort  est : " + users[rand]);
                                isStartTirageSort = false;
                                users = null;
                            }
                        }
                    }
                }
            }
        }
    }
}
