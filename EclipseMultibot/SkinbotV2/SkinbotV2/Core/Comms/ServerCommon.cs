using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSONSharp;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using Styx;
using Styx.CommonBot;
using Eclipse;
namespace Eclipse.Comms
{
    public static class ServerCommon
    {
        public static List<WowClient> ListeningClients = new List<WowClient>();
        public static List<WowMessage> Messages = new List<WowMessage>();
        public static List<WowCharacter> Characters = new List<WowCharacter>();
        public static List<String> RunningLog = new List<string>();
        public static int StartingPortNumber = 8002;
        public static bool RunServer = true;
        public static string OK = new WowMessage() { Type = "Ok" }.ToJSON();
        public static bool UpdateUI = false;
        public static void StopServer()
        {
            RunServer = false;
        }
        public static void Log(string Text)
        {
            RunningLog.Add(string.Format("{0}: {1} \r\n", DateTime.Now, Text));
        }
        public static int GetNextPortNumber(){
            if (ListeningClients.Count > 0){
                int maxport = ListeningClients.Max(p=>p.PortNumber);
                return maxport + 1;
            } else {
                return StartingPortNumber;
            }
        }
        public static void StartServer()
        {
            ThreadStart myThreadDelegate = new ThreadStart(DoWork);
            Thread myThread = new Thread(myThreadDelegate);
            myThread.Start();
        }
        public static void DoWork()
        {

            try
            {
                // set the TcpListener on port 13000 
                int port = 8001;
                TcpListener server = new TcpListener(IPAddress.Any, port);

                // Start listening for client requests
                server.Start();
                EC.Log("Server Listening on port 8001.");

                // Buffer for reading data 
                byte[] bytes = new byte[1024];
                string data;

                //Enter the listening loop 
                while (RunServer)
                {
                    EC.Log("Waiting for a connection... ");

                    // Perform a blocking call to accept requests. 
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    EC.Log("Connected!");

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
                        EC.Log(String.Format("Received: {0}", data));

                        // Process the data sent by the client.
                        //HANDLE MESSAGE

                        string ReturnMessage = ServerCommon.Proccess(data, remoteIpEndPoint.Address);

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(ReturnMessage);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        EC.Log(String.Format("Sent: {0}", ReturnMessage));

                        i = stream.Read(bytes, 0, bytes.Length);

                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }
        public static string Proccess(string data, IPAddress ip)
        {
            UpdateUI = true;
            WowMessage obj = JSON.Deserialize<WowMessage>(data);
            WowClient client = ListeningClients.Where(l => l.IPAddress.ToString() == ip.ToString() && l.PortNumber == obj.Port).FirstOrDefault();
            if (client != null) client.Messages.Add(obj);
            switch (obj.Type)
            {
                case "Broadcast":
                    var chr = Characters.Where(c => c.Name == obj.Name).FirstOrDefault();
                    if (chr != null)
                    {
                        chr.Name = obj.Name;
                        chr.Level = obj.Level;
                        chr.Location = obj.Location;
                        chr.ZoneId = obj.ZoneId;
                        client.Character = chr;
                    }
                    else
                    {
                        WowCharacter wc = new WowCharacter() { Level = obj.Level, Location = obj.Location, ZoneId = obj.ZoneId, Name = obj.Name };
                        Characters.Add(wc);
                        client.Character = wc;
                    }
                    return new WowMessage() { Type="Ok" }.ToJSON();
                case "GetChar":
                    var responseChar = Characters.Where(c => c.Name == obj.Name).FirstOrDefault();
                    if (responseChar == null)
                    {
                        return new WowMessage() { Type = "CharacterNotFound" }.ToJSON();
                    }
                    else return responseChar.ToJSON();
                case "LetsBeFriends":
                    try
                    {
                        //ToDo: Return a Port number to launch a server on the client side.
                        int portNum = GetNextPortNumber();
                        WowCharacter wchr = new WowCharacter() { Name = obj.Name };
                        Characters.Add(wchr);
                        client = new WowClient() { IPAddress = ip, PortNumber = portNum, Character = wchr };
                        client.Messages.Add(obj);
                        ServerCommon.ListeningClients.Add(client);
                        Mount.OnMountUp += Mount_OnMountUp;
                        return new WowMessage() { Type = "CallbackPort", data = portNum.ToString() }.ToJSON();
                    }
                    catch (Exception err){
                        return new WowMessage() { Type = err.ToString() }.ToJSON();
                    }
                    //ToDo: Add client code to launch a listening server.
                default:
                    return "";
            }

        }

        static void Mount_OnMountUp(object sender, MountUpEventArgs e)
        {
            MessageClients(new WowMessage() {Type = "MountUp" });
        }
        public static void MessageClients(WowMessage msg)
        {
            List<WowClient> DCedClients = new List<WowClient>();
            foreach (var client in ListeningClients){
                try
                {
                    SendMessage(msg, client);
                }
                catch(Exception err){
                    DCedClients.Add(client);
                }
            }
            foreach (var client in DCedClients)
            {
                ListeningClients.Remove(client);
            }
        }
        public static string SendMessage(WowMessage mesg, WowClient client)
        {
            // Buffer for reading data 
            byte[] bytes = new byte[1024];
            string data;

            TcpClient tcpclnt = new TcpClient();
            Log("Connecting.....");

            tcpclnt.Connect(client.IPAddress, client.PortNumber);
            // use the ipaddress as in the server program

            Log("Connected");

            Stream stm = tcpclnt.GetStream();

            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes(mesg.ToJSON());
            Log("Transmitting.....");

            stm.Write(ba, 0, ba.Length);

            Log(String.Format("Sent: {0}", mesg.ToJSON()));
            //Proccess(recmessage);
            tcpclnt.Close();
            return mesg.ToJSON();
        }
    }
}
