using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using TwitchBot.Functionalities;

namespace TwitchBot
{
    public class IrcClient : IDisposable
    {
        private string channel;
        private string userName;

        private TcpClient tcpClient;
        private StreamReader inputStream;
        private StreamWriter outputStream;

        private List<Functionality> functionalities; // A RAJOUTER tous ce qui est Functionality

        public event EventHandler<ChatMessage> OnMessageReceived = null;

        public IrcClient(string ip, int port, string userName, string password)
        {
            this.userName = userName;
            functionalities = new List<Functionality>();
            functionalities.Add(new PeriodicalMessageFunctionality(this, 60000));
            functionalities.Add(new VoteFunctionality(this));
            functionalities.Add(new TirageSortFunctionality(this));
            this.start(ip, port, userName, password);
        }

        private void connect(string ip, int port)
        {
            tcpClient = new TcpClient(ip, port);
            inputStream = new StreamReader(tcpClient.GetStream());
            outputStream = new StreamWriter(tcpClient.GetStream());
        }

        private void authenticate(string userName, string password)
        {
            outputStream.WriteLine("PASS " + password);
            outputStream.WriteLine("NICK " + userName);
            outputStream.WriteLine("USER " + userName + " 8 * :" + userName);
            outputStream.Flush();
        }

        private void start(string ip, int port, string userName, string password)
        {
            this.connect(ip, port);
            this.authenticate(userName, password);

            foreach(Functionality functionality in functionalities)
            {
                functionality.Start();
            }
        }

        public void joinRoom(string channel)
        {
            this.channel = channel;
            // "Join #" permets de rejoindre un channel
            outputStream.WriteLine("JOIN #" + channel);
            outputStream.Flush();
        }

        public void sendIrcMessage(string message)
        {
            outputStream.WriteLine(message);
            outputStream.Flush();
        }

        public void sendChatMessage(string value)
        {
            ChatMessage message = new ChatMessage(this.userName, ChatMessage.ChatMessageType.PRIVMSG, this.channel, value);
            sendIrcMessage(message.ToString());
        }

        public string readMessage()
        {
            string message = inputStream.ReadLine();

            Message msg = Message.Parse(message);

            if (msg is ServerMessage)
            { //traitement pour un serverMessage
                ServerMessage my_message = msg as ServerMessage;
            }
            if (msg is ChatMessage) //traitement pour un chatMessage
            {
                ChatMessage my_message = msg as ChatMessage;

                if (my_message.Text != null)
                {
                    OnMessageReceived?.Invoke(this, my_message);
                }
            }
            return message;
        }

        public void Dispose()
        {
            foreach (Functionality functionality in functionalities)
            {
                functionality.Stop();
            }

            if (inputStream != null) {
                inputStream.Close();
            }
        }
    }
}
