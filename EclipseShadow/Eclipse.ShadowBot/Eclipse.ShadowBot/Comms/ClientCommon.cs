using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using JSONSharp;
using System.Net;
using Eclipse.ShadowBot;
using Styx;
namespace Eclipse.Comms
{
    public class ClientCommon
    {
        public int PortNumber {get;set;}
        public string ServerIP { get; set; }
        public List<String> RunningLog = new List<string>();
        public bool IsLeader = false;
        public bool RunServer = false;
        public static WowMessage OK = new WowMessage() { Type = "Ok" };
        public List<WowMessage> Messages = new List<WowMessage>();
        public static bool UpdateUI = false;
        public void Log(string Text)
        {
            RunningLog.Add(string.Format("{0}: {1} \r\n", DateTime.Now, Text));
        }
        public void DoWork(WowMessage cr)
        {
            SendMessage(cr);
        }
        public void StopServer()
        {
            SendMessage(new WowMessage() { Type="Disconnect", Name = StyxWoW.Me.Name });
            RunServer = false;
        }
        public void StartServer()
        {
            Task.Run(() =>
            {
                try
                {
                    // set the TcpListener on port 13000 
                    int port = PortNumber;
                    TcpListener server = new TcpListener(IPAddress.Any, PortNumber);

                    // Start listening for client requests
                    server.Start();


                    // Buffer for reading data 
                    byte[] bytes = new byte[2048];
                    string data;

                    //Enter the listening loop 
                    while (RunServer)
                    {
                        EC.Log("Waiting for a connection from leader... ");

                        // Perform a blocking call to accept requests. 
                        // You could also user server.AcceptSocket() here.
                        TcpClient client = server.AcceptTcpClient();
                        EC.Log("Leader Connected!");

                        // Get a stream object for reading and writing
                        NetworkStream stream = client.GetStream();

                            var i = stream.Read(bytes, 0, bytes.Length);
                            // Translate data bytes to a ASCII string.
                            data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                            EC.Log(String.Format("Received: {0}", data));

                            // Process the data sent by the client.
                            //HANDLE MESSAGE

                            string ReturnMessage = Proccess(data).ToJSON();

                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(ReturnMessage);

                            // Send back a response.
                            stream.Write(msg, 0, msg.Length);
                            EC.Log(String.Format("Sent: {0}", ReturnMessage));
                        

                        // Shutdown and end connection
                        client.Close();
                    }
                    if (!RunServer)
                    {
                        EC.Log(String.Format("Shutting down client-side server"));
                    }
                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);
                }
            });
        }
        public string SendMessage(WowMessage msg)
        {
            msg.Port = this.PortNumber;
            TcpClient tcpclnt = new TcpClient();
            EC.Log("Connecting.....");

            tcpclnt.Connect("127.0.0.1", 8001);
            // use the ipaddress as in the server program

            EC.Log("Connected");

            Stream stm = tcpclnt.GetStream();

            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes(msg.ToJSON());
            stm.Write(ba, 0, ba.Length);
            EC.Log("Sent Message" + msg.ToJSON());
            byte[] bb = new byte[1000];
            if (stm.CanRead)
            {
                int k = stm.Read(bb, 0, 1000);
                var recmessage = string.Empty;
                for (int i = 0; i < k; i++)
                {
                    recmessage += Convert.ToChar(bb[i]).ToString();
                }
                Log(recmessage);
                Proccess(recmessage);
                tcpclnt.Close();
                return recmessage;
            }
            tcpclnt.Close();
            return null;
        }
        public WowMessage Proccess(string data)
        {
            UpdateUI = true;
            WowMessage obj = JSON.Deserialize<WowMessage>(data);
            if (obj != null) Messages.Add(obj);
            switch (obj.Type)
            {
                case "Broadcast":
                    return OK;
                case "ReadyCheck":
                    return OK;
                case "MountUp":
                    EclipseShadowBot.ShouldBeMounted = true;
                    return OK;
                case "Disconnect":
                    RunServer = false;
                    return OK;
                case "ToMe":
                    EclipseShadowBot.navLoc = new Styx.WoWPoint(obj.X, obj.Y, obj.Z);
                    EclipseShadowBot.NavMode = true;
                    EC.Log(string.Format("Forcing nav to {0}", EclipseShadowBot.navLoc.ToJSON()));
                    return OK;
                case "ChangeLeader":
                    EclipseShadowBot.FollowName = obj.Name;
                    EclipseShadowBot.Leader = null;
                    if (obj.Name == Styx.StyxWoW.Me.Name) EclipseShadowBot.LeaderMode = true;
                    return OK;
                case "CallbackPort":
                    PortNumber = int.Parse(obj.data);
                    if (!RunServer)
                    {
                        RunServer = true;
                        StartServer();
                    }
                    return OK;
                default:
                    return new WowMessage() {Type="UnknownMessageType" };
            }
        }
    }
}
