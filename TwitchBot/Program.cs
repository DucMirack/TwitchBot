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

        //** Variable pour la partie System de vote
        private static bool isStartVoting = false;
        private static Dictionary<string, int> propositions = new Dictionary<string, int>();
        //****//
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
                    string message = Irc.readMessage();
                    if (message != null)
                    {
                        Console.WriteLine(message);

                        if (message.Contains("!vote"))  // Test msg : !vote Quelle jeu aimez-vous ? witcher assassin pacman
                        {
                            string propositionAll = message.Substring(message.IndexOf('?') + 2);
                            systemVote.startVote(propositionAll);
                        }

                        if (systemVote.isStartVoting)
                        {
                            // Terminer le vote
                            if (message.Contains("!endvote")) {
                                systemVote.endVote();
                            }

                            systemVote.setValProposition(message);
                        }
                    }
                }
            }
        }
    }
}
