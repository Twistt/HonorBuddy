
using JSONSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eclipse.Comms
{
    public static class CommsCommon
    {
        public static ClientCommon cc = null;
        public static WowMessage OK = new WowMessage() { Type = "Ok" };
        public static void Log(string p)
        {
            EC.Log(p, LogLevel.Comms);
        }
        internal static Dictionary<string, Func<WowMessage>> ClientCommsEvents = new Dictionary<string, Func<WowMessage>>();
        internal static Dictionary<string, Func<WowMessage>> ServerCommsEvents = new Dictionary<string, Func<WowMessage>>();
        public static void AddClientCommHandler(string name, Func<WowMessage> eve)
        {
            ClientCommsEvents.Add(name, eve);
        }
        public static WowMessage DoClientCommEvent(string name){
            return (WowMessage)ClientCommsEvents.Where(n => n.Key == name).FirstOrDefault().Value.DynamicInvoke();
            //callbackAccept.DynamicInvoke(); //alternatively - callback.DynamicInvoke(args);
        }
        public static void AddServerCommHandler(string name, Func<WowMessage> eve)
        {
            ClientCommsEvents.Add(name, eve);
        }
        public static void DoServerCommEvent(string name)
        {
            ClientCommsEvents.Where(n => n.Key == name).FirstOrDefault().Value.DynamicInvoke();
            //callbackAccept.DynamicInvoke(); //alternatively - callback.DynamicInvoke(args);
        }
    }
    //ToDo List:
    /* Sell everything but hearth stone
     * Fix mounting event handler
     * Done: On stop of bot - send message to Leader to remove them as a client. 
     * Remove Loot Distance based on follow distance
     * Fix Dead Behavior
     * If leader is dead, follow secondary target in group
     * If someone in part is dead and we are not in combat, rez them
     * IF bags are full dont try to loot
     * Add the rest of the armor classes.
     * 
     */
}
