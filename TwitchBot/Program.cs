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

                while (true)
                {
                    string message = Irc.readMessage();
                    if (message != null)
                    {
                        Console.WriteLine(message);

                        if (message.Contains("!vote"))  // Test msg : !vote Quelle jeu aimez-vous ? witcher assassin pacman
                        {
                            string propositionAll = message.Substring(message.IndexOf('?') + 2);
                            if (propositionAll != "")
                            {
                                string[] msgPropos = propositionAll.Split(' ');
                                string sendPropo = "Vote : ";
                                foreach (string propo in msgPropos)
                                {
                                    sendPropo += "!" + propo + " ";
                                    propositions.Add("!" + propo, 0);
                                }

                                Irc.sendChatMessage(sendPropo);
                                isStartVoting = true;
                            }
                        }

                        if (isStartVoting)
                        {
                            // Terminer le vote
                            if (message.Contains("!endvote"))
                            {
                                isStartVoting = false;
                                Irc.sendChatMessage("Le vote est terminé !");
                                string recapMsg = "/me RECAP : ";
                                string gagnant = "";
                                int maxVal = 0;
                                foreach (KeyValuePair<string, int> prop in propositions) {
                                    if (prop.Value > maxVal) {
                                        gagnant = "/me LE GAGNANT EST : " + prop.Key;
                                    }
                                    recapMsg += prop.Key + " : " + prop.Value.ToString() + ", ";
                                    maxVal = prop.Value;
                                }
                                propositions.Clear();
                                Irc.sendChatMessage(recapMsg);
                                Irc.sendChatMessage(gagnant);
                            }

                            foreach (KeyValuePair<string, int> prop in propositions)
                            {
                                if (message.Contains(prop.Key)) {
                                    propositions[prop.Key] += 1;
                                    Irc.sendChatMessage(prop.Key + " : " + propositions[prop.Key].ToString());
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
