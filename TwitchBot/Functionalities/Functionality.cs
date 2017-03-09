using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.Functionalities
{
    public abstract class Functionality
    {
        public Functionality(IrcClient bot)
        {
            Bot = bot;
        }

        public IrcClient Bot { get; private set; }

        public bool Enabled { get; private set; }

        protected abstract void OnStart();

        protected abstract void OnStop();

        public void Start()
        {
            Enabled = true;
            OnStart();
        }

        public void Stop()
        {
            OnStop();
            Enabled = false;
        }
    }
}