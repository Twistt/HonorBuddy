using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Eclipse;
using System.IO;

namespace GatheringLegion.Models
{
    public class Settings
    {
        public List<Zone> GatheringZones = new List<Zone>();

        public List<Zone> AvoidZones = new List<Zone>();

        public bool AvoidElites { get; set; }
        public bool DontStopToFight { get; set; }
        public bool GatherChests { get; internal set; }
        public bool GatherManaNodes { get; internal set; }

        public bool SkinThings = false;

        public void SaveSettings()
        {
            System.IO.File.WriteAllText("EclipseSettings.es", JsonConvert.SerializeObject(EC.Settings));
        }
        public void LoadSettings()
        {
            if (File.Exists("EclipseSettings.es"))
            {
                var settingsraw = System.IO.File.ReadAllText("EclipseSettings.es");
                EC.Settings = JsonConvert.DeserializeObject<Settings>(settingsraw);
            } else
            {
                EC.Settings = new Settings();
                EC.Settings.AvoidElites = false;
                EC.Settings.DontStopToFight = true;
            }
        }
    }
}
