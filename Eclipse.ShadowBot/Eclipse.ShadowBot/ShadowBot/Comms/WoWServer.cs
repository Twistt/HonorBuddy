using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Eclipse.ShadowBot;
using JSONSharp;
using Styx;

namespace Eclipse.Comms
{
    public class WoWServer
    {
        public static bool RunServer = true;
        public static List<TcpClient> Clients = new List<TcpClient>();
        public static List<WowMessage> Messages = new List<WowMessage>();
        public static List<WowCharacter> Characters = new List<WowCharacter>();
        public static List<String> RunningLog = new List<string>();
        public static int StartingPortNumber = 8002;
        public static string OK = new WowMessage() { Type = "Ok" }.ToJSON();
        public static bool UpdateUI = false;
        public static void Log(string Text)
        {
            RunningLog.Add(string.Format("{0}: {1} \r\n", DateTime.Now, Text));
        }

        public static string Proccess(string data, WoWClient client)
        {
            UpdateUI = true;
            WowMessage obj = JSON.Deserialize<WowMessage>(data);
            switch (obj.Type)
            {
                case "Broadcast":
                    var chr = Characters.Where(c => c.Name == obj.Name).FirstOrDefault();
                    if (chr != null)
                    {
                        chr.Name = obj.Name;
                        chr.Level = obj.Level;
                        chr.Location = new WowLocation() { X = obj.X, Y = obj.Y, Z = obj.Z };
                        chr.ZoneId = obj.ZoneId;
                        client.Character = chr;
                    }
                    else
                    {
                        WowCharacter wc = new WowCharacter() { Level = obj.Level, Location = new WowLocation() { X = obj.X, Y = obj.Y, Z = obj.Z }, ZoneId = obj.ZoneId, Name = obj.Name };
                        Characters.Add(wc);
                        client.Character = wc;
                    }
                    return new WowMessage() { Type = "Ok" }.ToJSON();
                case "GetChar":
                    var responseChar = Characters.Where(c => c.Name == obj.Name).FirstOrDefault();
                    if (responseChar == null)
                    {
                        return new WowMessage() { Type = "CharacterNotFound" }.ToJSON();
                    }
                    else
                    {
                        if (obj.Name == StyxWoW.Me.Name) return new WowMessage() { Type = "LocationCheckResponse", Name = EC.Me.Name, X = EC.Me.Location.X, Y = EC.Me.Location.Y, Z = EC.Me.Location.Z }.ToJSON();
                        else return new WowMessage() { Type = "LocationCheckResponse", Name = responseChar.Name, X = responseChar.Location.X, Y = responseChar.Location.Y, Z = responseChar.Location.Z }.ToJSON();
                    }
                case "LetsBeFriends":
                    try
                    {
                        //ToDo: Return a Port number to launch a server on the client side.
                        WowCharacter wchr = new WowCharacter() { Name = obj.Name };
                        Characters.Add(wchr);
                        return OK;
                    }
                    catch (Exception err)
                    {
                        return new WowMessage() { Type = err.ToString() }.ToJSON();
                    }

                case "Disconnect":
                    return OK;
                default:
                    return "";
            }

        }
        public void Start()
        {
            ThreadStart myThreadDelegate = new ThreadStart(DoWork);
            Thread myThread = new Thread(myThreadDelegate);
            myThread.Start();
        }
        public void DoWork()
        {

            try
            {
                // set the TcpListener on port 13000 
                int port = 8001;
                TcpListener server = new TcpListener(IPAddress.Any, port);

                // Start listening for client requests
                server.Start();
                WoWServer.Log("Server Listening on port 8001.");



                //Enter the listening loop 
                while (RunServer)
                {
                    WoWServer.Log("Waiting for a connection... ");

                    // Perform a blocking call to accept requests. 
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    WoWServer.Log("Connected!");

                    new Thread(() => HandleClient(client)).Start();

                    // Shutdown and end connection
                    //client.Close();
                }
            }
            catch (SocketException e)
            {
                WoWServer.Log(string.Format("SocketException: {0}", e));
            }
        }
        private void HandleClient(TcpClient client)
        {
            Clients.Add(client);
            // Buffer for reading data 
            byte[] bytes = new byte[1024];
            string data;
            // Get a stream object for reading and writing
            NetworkStream stream = client.GetStream();
            IPEndPoint remoteIpEndPoint = client.Client.RemoteEndPoint as IPEndPoint;
            int i;

            // Loop to receive all the data sent by the client.
            i = stream.Read(bytes, 0, bytes.Length);

            while (i != 0)
            {
                // Translate data bytes to a ASCII string.
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                WoWServer.Log(String.Format("Received: {0} from client {1}", data, client.Client.Handle));

                // Process the data sent by the client.
                //HANDLE MESSAGE
               
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(OK.ToJSON());

                // Send back a response.
                stream.Write(msg, 0, msg.Length);
                WoWServer.Log(String.Format("Sent: {0}", OK.ToJSON()));

                i = stream.Read(bytes, 0, bytes.Length);

            }
        }
        public void MessageClients(WowMessage message)
        {
            foreach (var client in WoWServer.Clients)
            {
                NetworkStream stream = client.GetStream();
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(message.ToJSON());

                // Send back a response.
                stream.Write(msg, 0, msg.Length);
                WoWServer.Log(String.Format("Sent: {0}", message.ToJSON()));
                EC.Log(String.Format("Sent: {0}", message.ToJSON()));
            }
        }

        internal void SendMessage(WowMessage message, WoWClient client)
        {

            NetworkStream stream = client.client.GetStream();
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(message.ToJSON());

            stream.Write(msg, 0, msg.Length);
            WoWServer.Log(String.Format("Sent: {0}", message.ToJSON()));
            String.Format("Sent: {0}", message.ToJSON());
        }
    }
}
