using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Eclipse.ShadowBot;
using JSONSharp;
using Styx.Pathing;

namespace Eclipse.Comms
{
    public class WoWClient
    {
        public List<WowMessage> Messages = new List<WowMessage>();
        public List<String> logText = new List<String>();
        public TcpClient client = new TcpClient();
        ThreadStart myThreadDelegate;
        Thread myThread;
        TcpClient tcpClient = null;
        public WowCharacter Character = null;
        public int PortNumber { get; set; }
        public string ServerIP { get; set; }
        public List<String> RunningLog = new List<string>();
        public bool IsLeader = false;
        public static WowMessage OK = new WowMessage() { Type = "Ok" };
        public static bool UpdateUI = false;
        public DateTime LastMessageSent;

        public void Log(string Text)
        {
            RunningLog.Add(string.Format("{0}: {1} \r\n", DateTime.Now, Text));
        }

        public WowMessage Proccess(string data)
        {
            UpdateUI = true;
            WowMessage obj = JSON.Deserialize<WowMessage>(data);

            if (obj != null)
            {
                Messages.Add(obj);
                switch (obj.Type)
                {
                    case "LocationCheckResponse":
                        EclipseShadowBot.navLoc = new Styx.WoWPoint(obj.X, obj.Y, obj.Z);
                        EclipseShadowBot.NavMode = true;
                        EC.Log(string.Format("Lost leader - Forcing nave to location {0}", EclipseShadowBot.navLoc.ToJSON()));
                        return null;
                    case "Broadcast":
                        return OK;
                    case "ReadyCheck":
                        return OK;
                    case "MountUp":
                        EclipseShadowBot.ShouldBeMounted = true;
                        return OK;
                    case "Dismount":
                        EclipseShadowBot.ShouldBeMounted = false;
                        Flightor.MountHelper.Dismount();
                        return OK;
                    case "Disconnect":
                        client.Close();
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
                    case "Ok":
                        return OK;
                    default:
                        return new WowMessage() { Type = "UnknownMessageType" };
                }
            }
            else return null;
        }
        private void Listen()
        {
            byte[] bytes = new byte[1024];
            string data;
            while (client.Connected)
            {
                NetworkStream stream = client.GetStream();
                IPEndPoint remoteIpEndPoint = client.Client.RemoteEndPoint as IPEndPoint;
                int i;

                // Loop to receive all the data sent by the client.
                i = stream.Read(bytes, 0, bytes.Length);

                while (i != 0)
                {
                    // Translate data bytes to a ASCII string.
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    //{ "Type":"Ok","Name":null,"Level":0,"X":0,"Y":0,"Z":0,"ZoneId":0,"data":null,"timestamp":"08/07/2016 07:53:51 PM","Port":0}
                    data = data.Replace("\\", "");
                    //data = data.Replace("'", "\"");
                    data = data.Replace("\"{","{");
                    data = data.Replace("}\"", "}");


                    EC.Log(String.Format("Received: {0} from SERVER {1}", data, client.Client.Handle));
                    Log(String.Format("Received: {0} from SERVER {1}", data, client.Client.Handle));
                    // Process the data sent by the client.
                    //HANDLE MESSAGE

                    //string ReturnMessage = ServerCommon.Proccess(data, remoteIpEndPoint.Address);
                    WowMessage ReturnMessage = Proccess(data);
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(ReturnMessage.ToJSON());

                    i = stream.Read(bytes, 0, bytes.Length);

                }
            }
        }
        public void Disconnect()
        {
            if (client.Connected)
            {
                myThread.Abort();
                client.Close();
            }
        }
        public void Connect()
        {
            if (!client.Connected)
            {
               Log("Connecting.....");

                client.Connect("127.0.0.1", 8001);

                // use the ipaddress as in the server program

               Log("Connected");
            }

            Stream stm = client.GetStream();

            ASCIIEncoding asen = new ASCIIEncoding();
            var mess = new WowMessage() { Type = "LetsBeFriends" }.ToJSON().Replace('"','\'');

            byte[] ba = asen.GetBytes(mess);
            stm.Write(ba, 0, ba.Length);
            EC.Log("Sent Message:" + mess);
            myThreadDelegate = new ThreadStart(Listen);
            myThread = new Thread(myThreadDelegate);

            myThread.Start();
        }
    }

}
