using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot
{
    public class SystemVote
    {
        public bool isStartVoting = false;
        private Dictionary<string, int> propositions = new Dictionary<string, int>();
        private IrcClient Irc;

        public SystemVote(IrcClient irc)
        {
            this.Irc = irc;
        }

        public void startVote(string propositionAll)
        {
            if (propositionAll != "")
            {
                string[] msgPropos = propositionAll.Split(' ');
                string sendPropo = "Vote : ";
                foreach (string propo in msgPropos)
                {
                    sendPropo += "!" + propo + " ";
                    this.propositions.Add("!" + propo, 0);
                }

                Irc.sendChatMessage(sendPropo);
                this.isStartVoting = true;
            }
        }

        public void setValProposition(string message)
        {
            foreach (KeyValuePair<string, int> prop in this.propositions)
            {
                if (message.Contains(prop.Key))
                {
                    propositions[prop.Key] += 1;
                    Irc.sendChatMessage(prop.Key + " : " + propositions[prop.Key].ToString());
                    break;
                }
            }
        }

        public void endVote()
        {
            this.isStartVoting = false;
            Irc.sendChatMessage("Le vote est terminé !");
            string recapMsg = "/me RECAP : ";
            string gagnant = "";
            int maxVal = 0;
            foreach (KeyValuePair<string, int> prop in propositions)
            {
                if (prop.Value > maxVal) {
                    gagnant = "/me LE GAGNANT EST : " + prop.Key;
                }
                recapMsg += prop.Key + " : " + prop.Value.ToString() + ", ";
                maxVal = prop.Value;
            }
            this.propositions.Clear();
            Irc.sendChatMessage(recapMsg);
            Irc.sendChatMessage(gagnant);
        }
    }
}
