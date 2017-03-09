using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.Functionalities
{
    public class VoteFunctionality : Functionality
    {
        private string propositionAll = "";
        private Dictionary<string, int> propositions = new Dictionary<string, int>();
        private bool isStartVoting = false;

        public VoteFunctionality(IrcClient bot) : base(bot)
        {
            bot.OnMessageReceived += Irc_OnMessageReceived;
        }

        private void Irc_OnMessageReceived(object sender, ChatMessage e)
        {
            if (!Enabled)
                return;
            
            if (e.Contains("!vote"))  // Test msg : !vote Quelle jeu aimez-vous ? witcher assassin pacman
            {
                this.propositionAll = e.Text.Substring(e.Text.IndexOf('?') + 2);
                OnStart();
            }
            
            if (isStartVoting)
            {
                // Terminer le vote
                if (e.Contains("!endvote")) {
                    OnStop();
                }
                this.setValProposition(e.Text);
            }
        }

        private void setValProposition(string message)
        {
            foreach (KeyValuePair<string, int> prop in this.propositions)
            {
                if (message.Contains(prop.Key))
                {
                    propositions[prop.Key] += 1;
                    Bot.sendChatMessage(prop.Key + " : " + propositions[prop.Key].ToString());
                    break;
                }
            }
        }

        protected override void OnStart()
        {
            if (this.propositionAll != "")
            {
                Console.WriteLine(this.propositionAll);
                string[] msgPropos = propositionAll.Split(' ');
                string sendPropo = "Vote : ";
                foreach (string propo in msgPropos)
                {
                    sendPropo += "!" + propo + " ";
                    this.propositions.Add("!" + propo, 0);
                }
                this.isStartVoting = true;
                Bot.sendChatMessage(sendPropo);
            }
        }

        protected override void OnStop()
        {
            this.isStartVoting = false;
            Bot.sendChatMessage("Le vote est terminé !");
            string recapMsg = "/me RECAP : ";
            string gagnant = "";
            int maxVal = 0;
            foreach (KeyValuePair<string, int> prop in propositions)
            {
                if (prop.Value > maxVal)
                {
                    gagnant = "/me LE GAGNANT EST : " + prop.Key;
                }
                recapMsg += prop.Key + " : " + prop.Value.ToString() + ", ";
                maxVal = prop.Value;
            }
            this.propositions.Clear();
            Bot.sendChatMessage(recapMsg);
            Bot.sendChatMessage(gagnant);
        }
    }
}
